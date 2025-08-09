from fastapi import FastAPI
from pydantic import BaseModel
import google.generativeai as genai
from dotenv import load_dotenv
import os, json, re

load_dotenv()
genai.configure(api_key=os.getenv("GEMINI_API_KEY"))

model = genai.GenerativeModel(
    "models/gemini-1.5-flash-latest",
    generation_config={
        "temperature": 0.2,
        "top_p": 0.9,
        "top_k": 20,
        "max_output_tokens": 256,
        "response_mime_type": "application/json"
    }
)

app = FastAPI()

class PromptRequest(BaseModel):
    message: str

class LLMResult(BaseModel):
    intent: str
    categories: list[str]
    tags: list[str]

SYSTEM_PROMPT = """
Aşağıdaki kullanıcı mesajını e-ticaret bağlamında analiz et ve YALNIZCA şu JSON şemasında cevap ver:

{
  "intent": "<product_search | past_order_search | unknown>",
  "categories": ["<küçük harf kategoriler>"],
  "tags": ["<küçük harf, virgülsüz, atomik etiketler>"]
}

Kurallar:
- Sadece JSON döndür. Kod bloğu, açıklama, ek metin yok.
- Tüm değerleri küçük harfe çevir (tr-TR).
- "intent":
  - Kullanıcı "önceden aldığım / geçen sefer aldığım / tekrar almak istiyorum" gibi ifadeler kullanıyorsa: "past_order_search"
  - Ürün arıyorsa: "product_search"
  - Kararsızsan: "unknown"
- "categories" için şu listeyi tercih et (mevcutta veritabanınla uyumlu örnekler): 
  ["gıda", "mutfak gereçleri", "bilgisayar", "kişisel bakım", "ev elektroniği", "unlu mamuller"]
- "tags" SADECE atomik ve veritabanında olabilecek etiketlerden seçilmeli. Aşağıdaki sözlüğü KULLAN, liste dışında yeni uydurma tag üretme:
  # ürün/etiket sözlüğü (normalize edilir, küçük harf)
  synonyms_to_tags = {
    "pasta": ["pasta","un","yumurta","çikolata","mikser"],
    "pasta malzemeleri": ["pasta","un","yumurta","çikolata","mikser"],
    "tatlı": ["çikolata"],
    "ampul": ["ampul","elektrik"],
    "bilgisayar": ["bilgisayar","mouse","klavye"],
    "bilgisayar ekipmanları": ["bilgisayar","mouse","klavye"],
    "kişisel bakım": ["kişisel bakım","şampuan"],
    "mutfak": ["mutfak","tencere","mikser"],
    "temel gıda": ["gıda","un","yumurta","çikolata"]
  }

- Gelen kullanıcı mesajını önce küçük harfe çevir, yukarıdaki sözlükte eşleşen ifadeler varsa sözlükteki ATOMİK tag’leri döndür.
- Sözlükte yoksa, sadece şu atomik tag evreninden seç: 
  ["pasta","un","yumurta","çikolata","mikser","bilgisayar","mouse","klavye","ampul","elektrik","kişisel bakım","şampuan","tencere","mutfak"]
- Çok kelimeli genel ifadeleri ("pasta malzemeleri", "bilgisayar ekipmanları") ATOMİK tag’lere AYIR.
- Tag’lerde tekrarları kaldır.

Örnekler:
Cümle: "Pasta yapacağım"
Yanıt:
{
  "intent": "product_search",
  "categories": ["gıda","mutfak gereçleri"],
  "tags": ["pasta","un","yumurta","çikolata","mikser"]
}

Cümle: "Daha önce aldığım ampulü tekrar almak istiyorum"
Yanıt:
{
  "intent": "past_order_search",
  "categories": ["ev elektroniği"],
  "tags": ["ampul","elektrik"]
}

Cümle: "Bilgisayar ekipmanları lazım"
Yanıt:
{
  "intent": "product_search",
  "categories": ["bilgisayar"],
  "tags": ["bilgisayar","mouse","klavye"]
}

Cümle: "<<USER_MESSAGE>>"


"""

@app.post("/analyze/", response_model=LLMResult)
async def analyze_prompt(prompt: PromptRequest):
    user_message = prompt.message.strip()
    prompt_text = SYSTEM_PROMPT.replace("<<USER_MESSAGE>>", user_message)

    try:
        resp = model.generate_content([prompt_text])
        text = (resp.text or "").strip()

        # Kod bloğu gelirse temizle
        if text.startswith("```"):
            text = re.sub(r"^```(?:json)?\s*|\s*```$", "", text, flags=re.DOTALL).strip()

        # İlk JSON objesini yakala (emniyet kemeri)
        m = re.search(r"\{.*\}", text, flags=re.DOTALL)
        if not m:
            raise ValueError("JSON bulunamadı")

        payload = json.loads(m.group(0))

        # Güvenlik: zorunlu alanlar ve tipler
        intent = str(payload.get("intent", "unknown")).lower()
        categories = [str(c).lower() for c in payload.get("categories", [])]
        tags = [str(t).lower() for t in payload.get("tags", [])]

        return {"intent": intent, "categories": categories, "tags": tags}

    except Exception as e:
        # Servis stabil kalsın
        return {"intent": "unknown", "categories": [], "tags": []}

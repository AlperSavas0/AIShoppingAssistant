# AIShoppingAssistant

AIShoppingAssistant is a prototype of an AI-powered e-commerce assistant.  
It enables users to interact in natural language to:

- Search for products in their past orders
  (e.g., "Önceden aldığım saç kurutucuyu bulamıyorum"("I can't find the hair dryer I bought earlier") → returns the product you bought earlier)
- Find products on the site based on intent  
  (e.g., "Pasta yapmak istiyorum"("I want to bake a cake") → returns cake ingredients and tools)

## Features
- **ASP.NET Core MVC** web application
- **Python + Gemini API** for AI-powered text analysis
- MSSQL database for product and order data
- Modern, responsive UI built with Bootstrap
- Filters products and past orders based on AI-extracted tags and categories

## Project Structure
```
AIShoppingAssistantRepo/
│
├── ShoppingAssistantAI/      # MVC web application (.NET 8)
├── ai_service/               # Python AI service (Gemini API)
└── README.md
```

## Installation

### 1. MVC Project (ShoppingAssistantAI/)
```bash
cd ShoppingAssistantAI
dotnet restore
dotnet run
```

### 2. AI Service (ai_service/)
```bash
cd ai_service
python -m venv venv
source venv/bin/activate   # Windows: venv\Scripts\activate
pip install -r requirements.txt
cp .env.example .env       # Add your API keys
python main.py
```

## Environment Variables
In the `.env` file, set the following:
```
GEMINI_API_KEY=your_api_key_here
```

## How It Works
1. The MVC app sends user input to the AI service.
2. The AI service processes the input using Gemini API and extracts:
   - Relevant tags
   - Product categories
3. The MVC app filters the database and returns relevant products or past orders.

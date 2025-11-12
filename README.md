# 🤖 FullStack + AI Chat Uygulaması

##  Canlı Demolar
- ** Web Uygulama:** [Vercel'de deploy edilecek]
- ** Backend API:** [Render'da deploy edilecek] 
- ** Mobil APK:** [APK indirme linki]
- ** AI Servis:** [Hugging Face Spaces'te hazır]

##  Proje Özeti
Kullanıcıların gerçek zamanlı mesajlaşabildiği, yapay zeka destekli duygu analizi yapan modern web ve mobil uygulama.

##  Temel Özellikler
-  **Gerçek zamanlı mesajlaşma**
-  **AI duygu analizi** (Pozitif/Negatif/Nötr)
-  **Cross-platform** (Web + Android)
-  **Responsive tasarım**
-  **Türkçe dil desteği**
-  **Canlı bağlantı durumu takibi**

## Teknoloji Stack'i

## Frontend
- **Web:** React 18 + TypeScript + Vercel
- **Mobil:** React Native + TypeScript + Android
- **HTTP İstekleri:** Axios
- **State Yönetimi:** React Hooks

## Backend  
- **API:** .NET Core 8 + C#
- **Database:** SQLite + Entity Framework Core
- **Deployment:** Render
- **CORS:** Cross-Origin Resource Sharing

## AI Servis
- **Dil:** Python
- **Framework:** Transformers + Gradio
- **Model:** `savasy/bert-base-turkish-sentiment-cased`
- **Platform:** Hugging Face Spaces

## Proje Yapısı

fullstack-ai-chat/
├── 📂 backend/ # .NET Core API
│ ├── Controllers/
│ ├── Models/
│ └── Program.cs
├── 📂 frontend-web/ # React Web Uygulaması
│ ├── public/
│ └── src/
├── 📂 frontend-mobile/ # React Native Mobil Uygulama
│ ├── android/
│ └── src/
├── 📂 ai-service/ # Python AI Servisi
│ ├── app.py
│ └── requirements.txt
└── 📄 README.md


##  Kurulum ve Çalıştırma

## Backend (.NET Core)
```bash
cd backend
dotnet restore
dotnet run --urls="http://localhost:5000"

Web Frontend (React)

cd frontend-web
npm install
npm start

Mobil Frontend (React Native)

cd frontend-mobile
npm install
npx react-native run-android


AI Servis (Python)

cd ai-service
pip install -r requirements.txt
python app.py


 API Endpoints
GET /api/messages
Tüm mesajları getirir.
[
  {
    "id": 1,
    "username": "kullanici",
    "text": "Merhaba!",
    "sentiment": "positive",
    "sentimentScore": 0.85,
    "createdAt": "2024-01-01T10:00:00"
  }
]

POST /api/messages
Yeni mesaj gönderir.

{
  "username": "kullanici",
  "text": "Mesaj metni"
}

🤖 AI Entegrasyonu
Duygu Analizi Modeli
Model: savasy/bert-base-turkish-sentiment-cased

Dil: Türkçe

Çıktı: Pozitif / Negatif / Nötr

Doğruluk: Yüksek seviye Türkçe metin analizi

Analiz Örnekleri
"Harika bir gün!" → 🟢 Pozitif (%92)

"Çok kötü hissettiriyor" → 🔴 Negatif (%88)

"Normal bir durum" -> ⚪ Nötr (%65)



Web Arayüzü
Modern chat tasarımı

Gerçek zamanlı mesaj güncellemesi

Duygu durumu renk kodlaması

Responsive layout

Mobil Arayüzü
Native Android deneyimi

Touch-optimized arayüz

Offline mod desteği

Smooth animasyonlar

🚀 Deployment
Backend (Render)
GitHub repo'sunu bağla

Build Command: cd backend && dotnet publish -c Release -o ./publish

Start Command: cd backend/publish && ./ChatApi.exe

Web Frontend (Vercel)
GitHub repo'sunu bağla

Root Directory: frontend-web

Otomatik deploy

AI Servis (Hugging Face)
Yeni Space oluştur

SDK: Gradio seç

Dosyaları yükle

 Geliştirici
Gamze - FullStack Developer




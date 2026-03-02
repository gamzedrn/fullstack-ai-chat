import gradio as gr
from transformers import pipeline

# Duygu analizi modelini yükle
model = pipeline("sentiment-analysis")

# Bu fonksiyon, arayüze yazılan metni alıp modelden geçirecek
def analyze(text):
    # Modelden sonucu al
    result = model(text)[0]
    label = result['label']
    score = result['score']
    
    # Etiket İngilizce ise Türkçeleştir
    if label == 'POSITIVE':
        duygu = 'Pozitif 😊'
    elif label == 'NEGATIVE':
        duygu = 'Negatif ☹️'
    else:
        duygu = 'Nötr 😐'
        
    # Sonucu düzenli bir metin olarak döndür
    return f"Duygu: {duygu} (Güven: %{score*100:.2f})"

# Gradio arayüzünü oluştur
iface = gr.Interface(
    fn=analyze,  # Hangi fonksiyon çalışacak
    inputs=gr.Textbox(lines=2, placeholder="Mesajınızı buraya yazın..."),  # Giriş tipi
    outputs="text",  # Çıkış tipi
    title="Duygu Analizi Servisi",
    description="Bir metin yazın, duygusunu analiz edelim."
)

# Uygulamayı başlat (Hugging Face'te bu kısım otomatik çalışır)
iface.launch()
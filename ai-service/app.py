from transformers import pipeline # Hugging face transformers kütüphanesinden 'pipline' fonksiyonunu al
import gradio as gr  # Gradio arayüz kütüphaneisni içe aktar (web arayüz oluşturmak için)
import os  # işletim sistemleri için modül
from cProfile import label
from test.badsyntax_future3 import result
from test.test_lltrace import example
from test.test_smtplib import server

#modelin yüklendiğini bildiren mesaj yazdırır
print("Model yükleniyor ... Bu biraz zaman alabilir.")

#Türkçe duygu analizi modeli yüklenir
emotion_analyzer = pipeline(
    "text-classification",  # görev türü: metin sınıflandırma
    model = "savasy/bert-base-turkish-sentiment-cased" #kullanılan modelin Hugging Gace adresi
)
print("Model başarıyla yüklendi.")

def analyze_sentiment(text):
    try:
        # Eğer metin boşsa varsayılan olarak nötr dön
        if not text or text.strip() == "":
            return {"sentiment": "neutral", "score":0.5, "label": "Nötr"}

        #Modeli kullanarak duygu analizi yap
        result = emotion_analyzer(text)[0] # çıktı bir liste olduğundan ilk elemanı al
        label = result['label']   # Tahmin edilen duygu etiketi
        score = result['score']   # Modelin güven puanı (olasılık değeri)

        # Label mapping - Türkçe model çıktılarını İngilizceye çevir
        if label == "positive" or label == "pozitif":
            return  {"sentiment": "positive", "score": score, "label": "Pozitif"}
        elif label == "negative" or label == "negatif":
            return {"sentiment": "negative", "score": score, "label": "Negatif"}
        else:
            return {"sentiment": "neutral", "score": score, "label": "Nötr"}

        # Eğer hata olursa hatayı ekrana yaz ve nötr olarak dön
    except Exception as e:
        print(f"Analiz hatası: {e}")
        return {"sentiment": "neutral", "score": 0.5, "label":"Nötr", "error":str(e)}

    # Gradio arayüzünü oluştur
    demo = gr.Interface(
        fn = analyze_sentiment, #arayüzde çağrılacak fonksiyon
        inputs = gr.Textbox(   # Kullanıcıdan metin almak için bir metin kutusu
            label = "Mesaj",
            placeholder = "Analiz edilecek metni girin ...",
            lines = 2
        ),
        outputs = gr.JSON(label="Duygu Analizi Sonucu"),  # Çıktı olarak JSON formatında duygu sonucu
        title = "Duygu Analizi API - Türkçe",
        description = "Metnin duygu durumunu analiz eder (Pozitif/Negatif/Nötr)",
        examples = [
            ["Bugün harika bir gün!"],
            ["Çok kötü hissettiriyor"],
            ["Normal bir durum"]
        ]
    )

    # Hugging Face Space için gerekli
    if __name__ == "__main__":
        demo.launch(
            server_name="0.0.0.0",     # Sunucu tüm IP adreslerinden erişilebilir olsun
            server_port=7860,          # Uygulamanın çalışacağı port
            share=False                # Paylaşılabilir link oluşturma (True yaparsan dışarıdan da erişilir)
        )
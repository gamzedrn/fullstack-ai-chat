import React, { useState, useEffect, useRef } from "react";
import axios from "axios";
import "./App.css";

// Mesaj tipi
interface Message {
  id: number;
  username: string;
  text: string;
  sentiment: string;
  sentimentScore: number;
  createdAt: string;
}

// API URL - Backend local'de çalışıyor
const API_URL = "http://localhost:5000/api/messages";

function App() {
  const [messages, setMessages] = useState<Message[]>([]);
  const [username, setUsername] = useState("");
  const [newMessage, setNewMessage] = useState("");
  const [isConnected, setIsConnected] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);

  // Mesajları getir
  const fetchMessages = async () => {
    try {
      const response = await axios.get(API_URL);
      setMessages(response.data);
    } catch (error) {
      console.error("Mesajlar yüklenemedi:", error);
      setIsConnected(false);
    }
  };

  // Yeni mesaj gönder
  const sendMessage = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!newMessage.trim() || !username.trim()) return;

    try {
      const messageData = {
        username: username.trim(),
        text: newMessage.trim(),
      };

      await axios.post(API_URL, messageData);
      setNewMessage("");
      fetchMessages(); // Mesaj listesini güncelle
    } catch (error) {
      console.error("Mesaj gönderilemedi:", error);
      alert("Mesaj gönderilemedi! Backend çalışıyor mu?");
    }
  };

  // Sayfa yüklendiğinde ve her 2 saniyede bir mesajları güncelle
  useEffect(() => {
    fetchMessages();
    const interval = setInterval(fetchMessages, 2000);
    return () => clearInterval(interval);
  }, []);

  // Backend bağlantısını kontrol et
  useEffect(() => {
    checkConnection();
  }, []);

  const checkConnection = async () => {
    try {
      await axios.get(API_URL);
      setIsConnected(true);
    } catch (error) {
      setIsConnected(false);
    }
  };

  // Scroll'u en alta taşı
  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  // Duygu durumu renkleri
  const getSentimentColor = (sentiment: string) => {
    switch (sentiment) {
      case "positive":
        return "#10b981"; // Yeşil
      case "negative":
        return "#ef4444"; // Kırmızı
      default:
        return "#6b7280"; // Gri
    }
  };

  // Duygu durumu emojileri
  const getSentimentEmoji = (sentiment: string) => {
    switch (sentiment) {
      case "positive":
        return "😊";
      case "negative":
        return "😞";
      default:
        return "😐";
    }
  };

  return (
    <div className="App">
      <div className="chat-container">
        {/* Başlık ve Bağlantı Durumu */}
        <div className="chat-header">
          <h1>🤖 AI Duygu Analizli Sohbet</h1>
          <div
            className={`connection-status ${
              isConnected ? "connected" : "disconnected"
            }`}
          >
            {isConnected ? "✅ Backend Bağlı" : "❌ Backend Bağlantısı Yok"}
          </div>
        </div>

        {/* Kullanıcı adı girişi */}
        {!username && (
          <div className="username-section">
            <h3>Sohbete Katıl</h3>
            <input
              type="text"
              placeholder="Rumuzunuzu girin..."
              value={username}
              onChange={(e) => setUsername(e.target.value)}
              className="username-input"
              maxLength={20}
            />
            <small>Max 20 karakter</small>
          </div>
        )}

        {/* Hoş geldin mesajı */}
        {username && (
          <div className="welcome-message">
            👋 Hoş geldin, <strong>{username}</strong>!
          </div>
        )}

        {/* Mesaj listesi */}
        <div className="messages-container">
          {messages.length === 0 ? (
            <div className="no-messages">
              📝 Henüz mesaj yok. İlk mesajı sen gönder!
            </div>
          ) : (
            messages.map((message) => (
              <div
                key={message.id}
                className={`message ${
                  message.username === username ? "own-message" : ""
                }`}
              >
                <div className="message-header">
                  <strong>{message.username}</strong>
                  <span className="message-time">
                    {new Date(message.createdAt).toLocaleTimeString("tr-TR", {
                      hour: "2-digit",
                      minute: "2-digit",
                    })}
                  </span>
                </div>
                <div className="message-text">{message.text}</div>
                <div
                  className="sentiment-badge"
                  style={{
                    backgroundColor: getSentimentColor(message.sentiment),
                  }}
                >
                  <span className="sentiment-emoji">
                    {getSentimentEmoji(message.sentiment)}
                  </span>
                  <span className="sentiment-text">
                    {message.sentiment === "positive"
                      ? "Pozitif"
                      : message.sentiment === "negative"
                      ? "Negatif"
                      : "Nötr"}
                    ({(message.sentimentScore * 100).toFixed(0)}%)
                  </span>
                </div>
              </div>
            ))
          )}
          <div ref={messagesEndRef} />
        </div>

        {/* Mesaj gönderme formu */}
        {username && (
          <form onSubmit={sendMessage} className="message-form">
            <div className="input-group">
              <input
                type="text"
                value={newMessage}
                onChange={(e) => setNewMessage(e.target.value)}
                placeholder="Mesajınızı yazın..."
                disabled={!isConnected}
                className="message-input"
                maxLength={500}
              />
              <button
                type="submit"
                disabled={!newMessage.trim() || !isConnected}
                className="send-button"
              >
                📨 Gönder
              </button>
            </div>
            <small className="char-count">
              {newMessage.length}/500 karakter
            </small>
          </form>
        )}

        {/* Backend bağlantısı yoksa uyarı */}
        {!isConnected && (
          <div className="connection-warning">
            ⚠️ Backend bağlantısı yok. Lütfen backend'in çalıştığından emin
            olun.
            <br />
            <code>cd backend && dotnet run --urls="http://localhost:5000"</code>
          </div>
        )}
      </div>
    </div>
  );
}

export default App;

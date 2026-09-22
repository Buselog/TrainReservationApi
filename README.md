# Train Reservation API

Bu proje, tren seferlerinde vagon bazlı online bilet rezervasyon kurallarını yöneten bir RESTful Web API servisidir.

## 🚀 Kullanılan Teknolojiler & Mimari Yaklaşım
* **.NET 8 / ASP.NET Core Web API**
* **Dependency Injection & Interface Abstraction:** İş kuralları controller katmanından tamamen izole edilmiş, `IReservationService` arayüzü üzerinden soyutlanarak test edilebilir ve sürdürülebilir bir mimari kurulmuştur.
* **Hassas Matematiksel Hesaplama (`decimal`):** İkili sayı sistemi tabanlı veri tiplerinin (`double`/`float`) neden olabileceği hassasiyet kayıplarını ve yuvarlama tutarsızlıklarını önlemek adına kapasite hesaplamalarında `decimal` tipi ve `Math.Floor` kullanılmıştır.

## 📌 Temel İş Kuralları
1. **%70 Doluluk Limiti:** Bir vagonun online rezervasyon kabul edebileceği tavan kontenjan, fiziksel kapasitesinin %70'i ile sınırlıdır.
2. **Tek Vagon Rezervasyonu:** `KisilerFarkliVagonlaraYerlestirilebilir = false` ise tüm grup tek bir vagona sığmak zorundadır; bölünme yapılamaz.
3. **Dağıtımlı Rezervasyon:** `KisilerFarkliVagonlaraYerlestirilebilir = true` ise yolcular uygun vagonlara sırayla paylaştırılır.
4. **Atomiklik İlkesi:** Talep edilen yolcuların tamamı yerleştirilemiyorsa kısmi yerleşim yapılmaz; işlem iptal edilerek boş liste dönülür.

---

## 🧪 Örnek İstek ve Yanıt Senaryoları

### Senaryo 1: Farklı Vagonlara Dağıtımlı Rezervasyon (Başarılı)
**POST** `/api/Reservations`

**Request:**
```json
{
  "tren": {
    "ad": "Başkent Ekspres",
    "vagonlar": [
      { "ad": "Vagon 1", "kapasite": 100, "doluKoltukAdet": 68 },
      { "ad": "Vagon 2", "kapasite": 90, "doluKoltukAdet": 50 }
    ]
  },
  "rezervasyonYapilacakKisiSayisi": 15,
  "kisilerFarkliVagonlaraYerlestirilebilir": true
}
```

**Response:**
```json
{
  "rezervasyonYapilabilir": true,
  "yerlesimAyrinti": [
    { "vagonAdi": "Vagon 1", "kisiSayisi": 2 },
    { "vagonAdi": "Vagon 2", "kisiSayisi": 13 }
  ]
}
```

### Senaryo 2: Tek Vagon Kuralı & Kapasite Yetersizliği (Başarısız)
**POST** `/api/Reservations`

**Request:**
```json
{
  "tren": {
    "ad": "Ege Ekspresi",
    "vagonlar": [
      { "ad": "Vagon 1", "kapasite": 50, "doluKoltukAdet": 34 }
    ]
  },
  "rezervasyonYapilacakKisiSayisi": 4,
  "kisilerFarkliVagonlaraYerlestirilebilir": false
}
```

**Response:**
```json
{
  "rezervasyonYapilabilir": false,
  "yerlesimAyrinti": []
}
```


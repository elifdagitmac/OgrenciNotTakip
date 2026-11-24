# Öğrenci Not Takip Sistemi

Görsel Programlama dersi projesi olarak geliştirilmiş, C# Windows Forms masaüstü uygulaması. Veriler SQLite veritabanında saklanır.

## Özellikler

- Öğrenci ekleme, güncelleme ve silme
- Vize ve final notundan ortalama hesaplama (vize %40, final %60)
- Geçti / Kaldı durumunu otomatik gösterme (ortalama 50 ve üstü: Geçti)
- Giriş kontrolleri: ad ve soyad sadece harf, notlar 0-100 arası sayı olmalı
- Tablodan satır seçince bilgilerin kutulara gelmesi

## Kullanılan Teknolojiler

- C# / .NET 8
- Windows Forms
- SQLite (`Microsoft.Data.Sqlite`)

## Kurulum ve Çalıştırma

1. Projeyi indir veya klonla:
   ```
   git clone https://github.com/elifdagitmac/OgrenciNotTakip.git
   ```
2. `OgrenciNotTakip.csproj` dosyasını Visual Studio 2022 ile aç.
3. **F5** ile çalıştır. SQLite paketi otomatik yüklenir (internet gerekir).

Veritabanı dosyası (`ogrenci.db`) ilk çalıştırmada otomatik oluşturulur.

## Proje Yapısı

| Dosya | Açıklama |
|---|---|
| `Program.cs` | Uygulamanın giriş noktası |
| `Form1.cs` | Arayüz ve veritabanı işlemleri |
| `OgrenciNotTakip.csproj` | Proje ayarları ve paketler |

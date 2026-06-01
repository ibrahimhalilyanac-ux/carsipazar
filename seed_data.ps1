[CmdletBinding()]
Param()

# PowerShell konsolunu ve çıktılarını UTF-8'e zorla
[Console]::OutputEncoding = [System.Text.Encoding]::UTF8
$OutputEncoding = [System.Text.Encoding]::UTF8

$catalogUrl = "http://localhost:7070/api"

Write-Host "--- MULTISHOP VERI TOHUMLAMA (EŞSİZ ÜRÜNLER VE TÜRKÇE DESTEKLİ) ---" -ForegroundColor Cyan

# 1. Mevcut Verileri Temizle
Write-Host "Mevcut veriler temizleniyor..." -ForegroundColor DarkYellow
try {
    $categories = Invoke-RestMethod -Uri "$catalogUrl/categories" -Method Get
    foreach ($cat in $categories) {
        try {
            $products = Invoke-RestMethod -Uri "$catalogUrl/products/ProductListWithCategoryByCategoryId/$($cat.CategoryID)" -Method Get
            foreach ($p in $products) {
                Invoke-RestMethod -Uri "$catalogUrl/products?id=$($p.productId)" -Method Delete
            }
        } catch { }
        Invoke-RestMethod -Uri "$catalogUrl/categories?id=$($cat.CategoryID)" -Method Delete
    }
} catch {
    Write-Host "Veritabanı zaten boş veya temizleme atlandı."
}

# 2. Gerçekçi Kategoriler ve Tamamen Farklı Ürün Setleri Tanımla
$categoryDefinitions = @(
    @{
        CategoryName = "Giyim & Aksesuar"
        CategoryImage = "/images/categoryimages/clothes.png"
        Products = @(
            @{Name="Kışlık Örme Kazak"; Image="/images/featureproductimages/jumper.jpg"; Price=450},
            @{Name="Pamuklu Beyaz Tişört"; Image="/images/categoryimages/clothes.png"; Price=150},
            @{Name="Kadın Kışlık Mont"; Image="/images/categoryimages/clothes2.png"; Price=850},
            @{Name="Spor Kesim Pantolon"; Image="/images/categoryimages/clothes.png"; Price=320},
            @{Name="Erkek Deri Ceket"; Image="/images/featureproductimages/jumper.jpg"; Price=1200},
            @{Name="Yazlık V Yaka Tişört"; Image="/images/categoryimages/clothes2.png"; Price=120}
        )
    },
    @{
        CategoryName = "Elektronik & Teknoloji"
        CategoryImage = "/images/categoryimages/laptop.png"
        Products = @(
            @{Name="Gaming Laptop RTX 4060"; Image="/images/featureproductimages/laptop.jpg"; Price=25000},
            @{Name="Akıllı Telefon 128GB"; Image="/images/categoryimages/phone.png"; Price=12000},
            @{Name="Bluetooth Kablosuz Kulaklık"; Image="/images/categoryimages/electronics.jpg"; Price=850},
            @{Name="Tablet Bilgisayar 10 inç"; Image="/images/categoryimages/laptop.png"; Price=4500},
            @{Name="Ultra HD 4K Monitör"; Image="/images/featureproductimages/laptop.jpg"; Price=3200},
            @{Name="Oyun Konsolu 1TB"; Image="/images/categoryimages/electronics.jpg"; Price=18000}
        )
    },
    @{
        CategoryName = "Ev & Dekorasyon"
        CategoryImage = "/images/categoryimages/furniture.jpg"
        Products = @(
            @{Name="Modern 3'lü Koltuk"; Image="/images/categoryimages/furniture.jpg"; Price=4500},
            @{Name="Ahşap Mutfak Masası"; Image="/images/categoryimages/kitchen.jpg"; Price=2100},
            @{Name="Dekoratif Duvar Saati"; Image="/images/categoryimages/furniture.jpg"; Price=350},
            @{Name="Ortopedik Çift Kişilik Yatak"; Image="/images/categoryimages/furniture.jpg"; Price=6500},
            @{Name="TV Ünitesi ve Kitaplık"; Image="/images/categoryimages/furniture.jpg"; Price=1800},
            @{Name="Sıvı Sabunluk Seti"; Image="/images/featureproductimages/soap.jpg"; Price=120}
        )
    },
    @{
        CategoryName = "Ayakkabı & Çanta"
        CategoryImage = "/images/categoryimages/shoes.png"
        Products = @(
            @{Name="Günlük Spor Ayakkabı"; Image="/images/categoryimages/shoes.png"; Price=850},
            @{Name="Hakiki Deri Çizme"; Image="/images/categoryimages/shoes.png"; Price=1400},
            @{Name="Su Geçirmez Sırt Çantası"; Image="/images/categoryimages/shoes.png"; Price=450},
            @{Name="Kadın Omuz Çantası"; Image="/images/categoryimages/shoes.png"; Price=650},
            @{Name="Ortopedik Yürüyüş Ayakkabısı"; Image="/images/categoryimages/shoes.png"; Price=950},
            @{Name="Deri Evrak Çantası"; Image="/images/categoryimages/shoes.png"; Price=1200}
        )
    },
    @{
        CategoryName = "Kozmetik & Bakım"
        CategoryImage = "/images/categoryimages/cleanthings.jpg"
        Products = @(
            @{Name="Doğal Lavanta Sabunu"; Image="/images/featureproductimages/soap.jpg"; Price=45},
            @{Name="Ferahlatıcı Beyazlatıcı Diş Macunu"; Image="/images/featureproductimages/toothpaste.jpg"; Price=65},
            @{Name="Günlük Yüz Temizleme Jeli"; Image="/images/categoryimages/cleanthings.jpg"; Price=120},
            @{Name="Kırışıklık Karşıtı Gece Kremi"; Image="/images/categoryimages/cleanthings.jpg"; Price=250},
            @{Name="Leke Çıkarıcı Sıvı Deterjan"; Image="/images/featureproductimages/detergant.jpg"; Price=85},
            @{Name="Kepeğe Karşı Şampuan"; Image="/images/categoryimages/cleanthings.jpg"; Price=90}
        )
    },
    @{
        CategoryName = "Spor & Outdoor"
        CategoryImage = "/images/categoryimages/electronics.jpg"
        Products = @(
            @{Name="Nefes Alan Spor Tişört"; Image="/images/categoryimages/clothes.png"; Price=150},
            @{Name="Profesyonel Koşu Ayakkabısı"; Image="/images/categoryimages/shoes.png"; Price=1100},
            @{Name="4 Kişilik Kamp Çadırı"; Image="/images/categoryimages/clothes2.png"; Price=1450},
            @{Name="Kaymaz Yoga Matı"; Image="/images/categoryimages/cleanthings.jpg"; Price=220},
            @{Name="Dambıl Seti 10kg"; Image="/images/categoryimages/electronics.jpg"; Price=350},
            @{Name="Termos 1 Litre"; Image="/images/categoryimages/kitchen.jpg"; Price=280}
        )
    },
    @{
        CategoryName = "Kitap & Hobi"
        CategoryImage = "/images/categoryimages/toy.png"
        Products = @(
            @{Name="Eğitici Ahşap Oyuncak"; Image="/images/categoryimages/toy.png"; Price=120},
            @{Name="Dünya Klasikleri Roman Seti"; Image="/images/categoryimages/toy.png"; Price=240},
            @{Name="1000 Parça Doğa Puzzle"; Image="/images/categoryimages/toy.png"; Price=85},
            @{Name="Akrilik Boya Fırça Seti"; Image="/images/categoryimages/toy.png"; Price=150},
            @{Name="Tarih Ansiklopedisi"; Image="/images/categoryimages/toy.png"; Price=350},
            @{Name="Peluş Oyuncak Ayı"; Image="/images/categoryimages/toy.png"; Price=180}
        )
    },
    @{
        CategoryName = "Anne & Bebek"
        CategoryImage = "/images/categoryimages/clothes2.png"
        Products = @(
            @{Name="Katlanabilir Bebek Arabası"; Image="/images/categoryimages/toy.png"; Price=3500},
            @{Name="Yenidoğan Hastane Çıkışı Set"; Image="/images/categoryimages/clothes2.png"; Price=450},
            @{Name="Uyku Arkadaşı Ayıcık"; Image="/images/categoryimages/toy.png"; Price=150},
            @{Name="Çok Gözlü Bebek Bakım Çantası"; Image="/images/categoryimages/clothes2.png"; Price=350},
            @{Name="Organik Pamuk Bebek Battaniyesi"; Image="/images/categoryimages/clothes.png"; Price=280},
            @{Name="Bebek Şampuanı 500ml"; Image="/images/categoryimages/cleanthings.jpg"; Price=75}
        )
    },
    @{
        CategoryName = "Süpermarket"
        CategoryImage = "/images/categoryimages/fruit.png"
        Products = @(
            @{Name="Taze Karışık Meyve Paketi"; Image="/images/categoryimages/fruit.png"; Price=120},
            @{Name="Organik Sebze Sepeti"; Image="/images/categoryimages/vegetables.png"; Price=140},
            @{Name="Renk Koruyucu Çamaşır Deterjanı"; Image="/images/featureproductimages/detergant.jpg"; Price=95},
            @{Name="Dondurulmuş Tavuk Nugget"; Image="/images/categoryimages/fastfood.png"; Price=85},
            @{Name="Zeytinyağlı Doğal Sabun"; Image="/images/featureproductimages/soap.jpg"; Price=45},
            @{Name="Tam Buğday Unu 2kg"; Image="/images/categoryimages/vegetables.png"; Price=35}
        )
    },
    @{
        CategoryName = "Mutfak Gereçleri"
        CategoryImage = "/images/categoryimages/kitchen.jpg"
        Products = @(
            @{Name="7 Parça Granit Tencere Seti"; Image="/images/categoryimages/kitchen.jpg"; Price=1250},
            @{Name="Otomatik Filtre Kahve Makinesi"; Image="/images/categoryimages/kitchen.jpg"; Price=1800},
            @{Name="24 Parça Porselen Yemek Takımı"; Image="/images/categoryimages/kitchen.jpg"; Price=2100},
            @{Name="Çelik Çatal Bıçak Takımı 72 Parça"; Image="/images/categoryimages/kitchen.jpg"; Price=1500},
            @{Name="Cam Baharatlık Seti"; Image="/images/categoryimages/kitchen.jpg"; Price=250},
            @{Name="Leke Tutmaz Mutfak Önlüğü"; Image="/images/categoryimages/clothes.png"; Price=110}
        )
    }
)

foreach ($catDef in $categoryDefinitions) {
    Write-Host "Kategori ekleniyor: $($catDef.CategoryName)" -ForegroundColor Green
    
    $catBody = @{
        CategoryName = $catDef.CategoryName
        ImageUrl = $catDef.CategoryImage
    } | ConvertTo-Json -Compress
    
    $catBytes = [System.Text.Encoding]::UTF8.GetBytes($catBody)
    Invoke-RestMethod -Uri "$catalogUrl/categories" -Method Post -Body $catBytes -ContentType "application/json; charset=utf-8"
    
    $allCats = Invoke-RestMethod -Uri "$catalogUrl/categories" -Method Get
    $currentCat = $allCats | Where-Object { $_.categoryName -eq $catDef.CategoryName } | Select-Object -First 1
    
    if ($currentCat) {
        Write-Host "   -> $($catDef.CategoryName) eşsiz ürünleri ekleniyor..." -ForegroundColor Gray
        foreach ($prodDef in $catDef.Products) {
            $prodBody = @{
                ProductName = $prodDef.Name
                ProductPrice = [decimal]$prodDef.Price
                ProductImageUrl = $prodDef.Image
                ProductDescription = "$($catDef.CategoryName) kategorisine özel, stoklarımızda yer alan en çok tercih edilen ürün."
                CategoryId = $currentCat.CategoryID
            } | ConvertTo-Json -Compress
            
            $prodBytes = [System.Text.Encoding]::UTF8.GetBytes($prodBody)
            Invoke-RestMethod -Uri "$catalogUrl/products" -Method Post -Body $prodBytes -ContentType "application/json; charset=utf-8"
        }
    }
}

Write-Host "`nKUSURSUZ BAŞARI: Tüm veriler Türkçe karakter desteğiyle ve EŞSİZ gerçekçi ürünlerle güncellendi." -ForegroundColor Black -BackgroundColor Green

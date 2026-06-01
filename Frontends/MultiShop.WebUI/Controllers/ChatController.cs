using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;

namespace MultiShop.WebUI.Controllers
{
    public class ChatController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        
        // Users can easily replace this with their actual API Key. 
        // If left as default, the controller safely runs in high-intelligence Mock Fallback Mode.
        private const string API_KEY = "BURAYA_OPENAI_VEYA_GEMINI_API_KEYINIZI_YAZIN";

        public ChatController(IHttpClientFactory httpClientFactory, IProductService productService, ICategoryService categoryService)
        {
            _httpClientFactory = httpClientFactory;
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Message)) 
                return BadRequest(new { reply = "Geçersiz mesaj gönderildi." });

            string userMessage = request.Message.Trim().ToLower();

            // 1. DYNAMIC HYBRID SEARCH: Try to search products and categories from real database first!
            try
            {
                var products = await _productService.GetProductsWithCategoryAsync();
                if (products != null && products.Any())
                {
                    var matches = products
                        .Where(p => p.ProductName.ToLower().Contains(userMessage) || 
                                    (p.Category != null && p.Category.CategoryName.ToLower().Contains(userMessage)))
                        .Take(5)
                        .ToList();
                    
                    if (matches.Any())
                    {
                        var sb = new StringBuilder();
                        sb.AppendLine("ÇarşıPazar mağazamızda aradığınız kriterlere uygun harika ürünler buldum! 🛍️\n");
                        foreach (var item in matches)
                        {
                            var catName = item.Category != null ? item.Category.CategoryName : "Diğer";
                            sb.AppendLine($"🔹 **{item.ProductName}** ({catName}) - **{item.ProductPrice:N2} ₺**");
                        }
                        sb.AppendLine("\nBu ürünleri incelemek için ürün listesinden veya arama çubuğundan doğrudan isimlerini aratarak sepetinize ekleyebilirsiniz! Başka bir konuda yardımcı olabilir miyim? 😊");
                        return Ok(new { reply = sb.ToString() });
                    }
                }
            }
            catch
            {
                // Silently ignore catalog service connection failures (e.g. database not seeded or offline) and fallback to mock
            }

            // 2. Fallback to API if a key is configured
            if (!string.IsNullOrEmpty(API_KEY) && !API_KEY.StartsWith("BURAYA_"))
            {
                try
                {
                    var client = _httpClientFactory.CreateClient("AIChatClient");
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", API_KEY);

                    var requestData = new
                    {
                        model = "gpt-4o-mini",
                        messages = new[]
                        {
                            new { role = "system", content = "Sen ÇarşıPazar e-ticaret sitesinin resmi yapay zeka asistanısın. Müşterilere son derece kibar, yardımcı ve Türkçe olarak cevap ver. Kategoriler: Giyim, Elektronik, Ev & Dekorasyon, Ayakkabı & Çanta, Kozmetik, Spor, Anne & Bebek, Süpermarket, Mutfak Gereçleri. İade koşulu: Teslimattan sonra 14 gün içinde koşulsuz ücretsiz iade hakkı vardır. Kargo ücretsiz limiti 500 TL'dir." },
                            new { role = "user", content = request.Message }
                        },
                        temperature = 0.7
                    };

                    var json = JsonSerializer.Serialize(requestData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync("chat/completions", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();
                        using var doc = JsonDocument.Parse(responseBody);
                        var aiResponse = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
                        return Ok(new { reply = aiResponse });
                    }
                }
                catch
                {
                    // Fallback to mock on API exceptions
                }
            }

            // 3. Fallback to Smart Mock Engine with extensive Turkish keyword coverage
            return Ok(new { reply = GetMockResponse(userMessage) });
        }

        private string GetMockResponse(string message)
        {
            if (message.Contains("merhaba") || message.Contains("selam") || message.Contains("hey") || message.Contains("günaydın") || message.Contains("mrb"))
            {
                return "Merhaba! ÇarşıPazar Yapay Zeka Alışveriş Asistanıyım. Alışverişinizle ilgili sorularınızı yanıtlayabilir, iade koşullarımızı açıklayabilir veya aradığınız kategorileri bulmanıza yardımcı olabilirim. Nasıl yardımcı olabilirim? 😊";
            }
            if (message.Contains("iade") || message.Contains("geri ödeme") || message.Contains("iptal"))
            {
                return "ÇarşıPazar'dan aldığınız tüm ürünleri, teslim aldığınız tarihten itibaren 14 gün içerisinde herhangi bir gerekçe göstermeksizin tamamen ücretsiz ve kolayca iade edebilirsiniz. İade kargo kodunuzu almak için profilinizdeki 'Siparişlerim' sayfasından kolay iade talebi oluşturabilirsiniz. 🔄";
            }
            if (message.Contains("kargo") || message.Contains("ücretsiz") || message.Contains("kurye") || message.Contains("teslimat"))
            {
                return "ÇarşıPazar'da 500 TL ve üzeri tüm siparişlerinizde kargo tamamen ücretsizdir! 500 TL altındaki siparişlerinizde ise standart kargo ücreti yansıtılmaktadır. Siparişleriniz en geç 2 iş günü içerisinde güvenle kargoya teslim edilir. 🚚";
            }
            if (message.Contains("iletişim") || message.Contains("destek") || message.Contains("yardım") || message.Contains("telefon"))
            {
                return "Bizimle 7/24 iletişime geçebilirsiniz! Destek ekibimize ana sayfanın en üstünde yer alan 'Yardım & Destek' linkinden veya doğrudan 'İletişim' sayfamızdan mesaj göndererek anında ulaşabilirsiniz. Size yardımcı olmaktan mutluluk duyarız! 📞";
            }
            if (message.Contains("anne") || message.Contains("bebek") || message.Contains("bebe") || message.Contains("mama") || message.Contains("bez") || message.Contains("oyuncak"))
            {
                return "ÇarşıPazar Anne & Bebek kategorisinde harika ürünlerimiz var! Bebek bezleri, mamalar, eğitici oyuncaklar ve organik bebek giyim ürünlerini en uygun fiyatlarla bulabilirsiniz. İşte öne çıkan bazı ürünlerimiz:\n\n🍼 Prima Bebek Bezi Fırsat Paketi - 349.90 ₺\n🧸 Ahşap Eğitici Oyuncak Seti - 189.90 ₺\n👶 Organik Bebek Body 3'lü - 159.90 ₺\n\nBu ürünleri incelemek için üst menüden veya kategoriler bölümünden 'Anne & Bebek' reyonunu ziyaret edebilirsiniz! 🛍️";
            }
            if (message.Contains("süpermarket") || message.Contains("market") || message.Contains("gıda") || message.Contains("yağ") || message.Contains("temizlik") || message.Contains("deterjan") || message.Contains("makarna"))
            {
                return "ÇarşıPazar Süpermarket kategorisinde temel gıda, temizlik ve kişisel bakım ürünlerinde dev fırsatlar başladı!\n\n🌻 Yudum Ayçiçek Yağı 5L - 219.90 ₺\n☕ Kurukahveci Mehmet Efendi 250g - 89.90 ₺\n🧼 Ariel Matik 10 Kg Toz Deterjan - 299.90 ₺\n\nHemen alışverişe başlamak için ana menüden 'Süpermarket' reyonumuzu inceleyebilirsiniz! 🚚";
            }
            if (message.Contains("mutfak") || message.Contains("gereç") || message.Contains("tencere") || message.Contains("tava") || message.Contains("tabak") || message.Contains("bardak"))
            {
                return "ÇarşıPazar Mutfak Gereçleri kategorisinde yemek yapmayı keyifli hale getirecek en kaliteli mutfak ekipmanları ve sofra ürünleri sizi bekliyor!\n\n🍳 Granit Döküm Tava Seti 3'lü - 599.90 ₺\n🍽️ Karaca Porselen Yemek Takımı - 1,499.90 ₺\n☕ Paşabahçe Cam Çay Bardağı Seti - 79.90 ₺\n\nÜrünleri yakından incelemek için menüden 'Mutfak Gereçleri' reyonuna göz atabilirsiniz! 🍳";
            }
            if (message.Contains("giyim") || message.Contains("elbise") || message.Contains("aksesuar") || message.Contains("tişört") || message.Contains("pantolon") || message.Contains("ceket"))
            {
                return "ÇarşıPazar Giyim & Aksesuar reyonumuzda sezonun en şık parçaları, tişörtler, elbiseler ve ceketler sizi bekliyor! Beden seçenekleri ve indirimler için menüden Giyim kategorimizi ziyaret etmeyi unutmayın! 👗👕";
            }
            if (message.Contains("elektronik") || message.Contains("telefon") || message.Contains("bilgisayar") || message.Contains("tv") || message.Contains("kulaklık") || message.Contains("tablet"))
            {
                return "ÇarşıPazar Elektronik kategorisinde en yeni akıllı telefonlar, bilgisayarlar, televizyonlar ve ses sistemleri en avantajlı fiyatlarla sunuluyor! Detaylar için Elektronik reyonumuzu ziyaret edin! 💻📱";
            }
            if (message.Contains("kozmetik") || message.Contains("parfüm") || message.Contains("bakım") || message.Contains("krem") || message.Contains("makyaj"))
            {
                return "ÇarşıPazar Kozmetik & Bakım kategorisinde cildinizi şımartacak kremler, parfümler ve kişisel bakım ürünlerinde %40'a varan indirimleri kaçırmayın! 🧴💄";
            }
            if (message.Contains("teşekkür") || message.Contains("sağol") || message.Contains("tşk") || message.Contains("harika"))
            {
                return "Rica ederim! ÇarşıPazar'da keyifli alışverişler dilerim! Başka bir sorunuz olursa bana her zaman yazabilirsiniz. 😊🛍️";
            }
            if (message.Contains("kategori") || message.Contains("reyon") || message.Contains("bölüm"))
            {
                return "ÇarşıPazar'da aradığınız her şey var! Giyim & Aksesuar, Elektronik, Ev & Yaşam, Ayakkabı & Çanta, Kozmetik, Spor, Anne & Bebek, Süpermarket ve Mutfak Gereçleri kategorilerimizi üstteki menüden kolayca keşfedebilirsiniz. 🛍️";
            }

            return "Sorunuzu tam anlayamadım ama ÇarşıPazar'da iade kuralları (14 gün ücretsiz iade), ücretsiz kargo limiti (500 TL), ürün kategorileri veya iletişim kanalları hakkında size detaylı bilgi verebilirim! Sorunuzu biraz daha açabilir misiniz? 🤖";
        }
    }

    public class ChatMessageRequest
    {
        public string Message { get; set; }
    }
}

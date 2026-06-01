using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.IdentityDtos.RegisterDtos;
using Newtonsoft.Json;
using System.Text;

namespace MultiShop.WebUI.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public RegisterController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreateRegisterDto createRegisterDto)
        {
            if (createRegisterDto.Password != createRegisterDto.ConfirmPassword)
            {
                ModelState.AddModelError("", "Şifreler uyuşmuyor. Lütfen şifrenizi tekrar kontrol edin.");
                return View(createRegisterDto);
            }

            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(createRegisterDto);
            StringContent stringContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("http://127.0.0.1:5001/api/Registers", stringContent);

            if (responseMessage.IsSuccessStatusCode)
            {
                var responseContent = await responseMessage.Content.ReadAsStringAsync();
                if (responseContent.Contains("başarıyla eklendi"))
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ModelState.AddModelError("", responseContent);
                }
            }
            else
            {
                var errorMsg = await responseMessage.Content.ReadAsStringAsync();
                ModelState.AddModelError("", !string.IsNullOrEmpty(errorMsg) ? errorMsg : "Kayıt işlemi başarısız oldu. Lütfen bilgilerinizi kontrol edin.");
            }

            return View(createRegisterDto);
        }
    }
}

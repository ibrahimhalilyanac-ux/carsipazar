using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiShop.DtoLayer.BasketDtos
{
    public class BasketTotalDto
    {
        public string UserId { get; set; } = string.Empty;
        public string DiscountCode { get; set; } = string.Empty;
        public int DiscountRate { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
        public decimal TotalPrice { get => BasketItems.Sum(x => x.Price * x.Quantity); }
    }
}

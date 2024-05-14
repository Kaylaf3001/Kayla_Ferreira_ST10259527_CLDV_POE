using Microsoft.AspNetCore.Mvc;

namespace POE_CloudDev.ViewModels
{
    public class ProductCardViewModel
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public bool Availability { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}

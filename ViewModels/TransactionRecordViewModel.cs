namespace POE_CloudDev.ViewModels
{
    public class TransactionRecordViewModel
    {
        public int TransactionID { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImagePath { get; set; }
        public string PurchaseDateTime { get; set; }
    }
}

namespace OzonEdu.MerchandiseService.HttpModels
{
    public sealed class MerchItemResponse
    {
        public long ItemId { get; set; }
        
        public string ItemName { get; set; }
        
        public int Quantity { get; set; }
    }
}
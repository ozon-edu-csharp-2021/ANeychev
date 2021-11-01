namespace OzonEdu.MerchandiseService.HttpModels
{
    public sealed class StockItemResponse
    {
        public long ItemId { get; set; }
        
        public string ItemName { get; set; }
        
        public int Quantity { get; set; }
    }
}
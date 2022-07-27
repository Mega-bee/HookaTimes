namespace HookaTimes.BLL.ViewModels.Concession
{
    public partial class Stock_VM
    {
        // public int Id { get; set; }
        public string Product { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
    }


    public partial class Orders_VM
    {
        // public int Id { get; set; }
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public string OrderDescription { get; set; }
        public string OrderStatus { get; set; }
    }
}

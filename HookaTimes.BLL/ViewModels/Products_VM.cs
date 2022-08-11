namespace HookaTimes.BLL.ViewModels
{
    public partial class ProductCategories_VM
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Decription { get; set; }
    }


    public partial class Product_VM
    {
        public int Id { get; set; }
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public decimal? CustomerInitialPrice { get; set; }
    }


}

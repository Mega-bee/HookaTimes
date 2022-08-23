namespace HookaTimes.BLL.ViewModels.Website
{
    public partial class Buddy_VM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Rating { get; set; }
        public string Image { get; set; }
        public string Profession { get; set; }
        public string Address { get; set; }
    }


    public partial class NavBuddy_VM
    {
        public string FirstName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
    }

    public partial class OrderHistoryMVC_VM
    {
        public int? Id { get; set; }
        public string Date { get; set; }
        public string Status { get; set; }
        public decimal Total { get; set; }
    }

}

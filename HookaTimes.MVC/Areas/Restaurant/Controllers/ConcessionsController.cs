using HookaTimes.BLL.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc;

namespace HookaTimes.MVC.Areas.Restaurant.Controllers
{
    [Area("Restaurant")]
    public class ConcessionsController : Controller
    {
        public static List<Concession_VM> concessions = new List<Concession_VM>();

        #region Sales List
        private void FillSales()
        {
            concessions.Add(new Concession_VM
            {
                Id = 718208,
                Date = "27 June 2022",
                CustomerName = "Hasan Bdeir",
                Description = "2x Apple Hooka Witty",
                Total = "USD30"
            });
            concessions.Add(new Concession_VM
            {
                Id = 718207,
                Date = "26 June 2022",
                CustomerName = "Imad Al Riz",
                Description = "3x Lemon&Mint Hooka Witty",
                Total = "USD45"
            });
            concessions.Add(new Concession_VM
            {
                Id = 718206,
                Date = "25 June 2022",
                CustomerName = "Mariam Meait",
                Description = "2x Liquorice Hooka Witty",
                Total = "USD30"
            });
            concessions.Add(new Concession_VM
            {
                Id = 718205,
                Date = "24 June 2022",
                CustomerName = "Badih Abou Hasan",
                Description = "2x Apple Hooka Witty",
                Total = "USD30"
            });
            concessions.Add(new Concession_VM
            {
                Id = 718204,
                Date = "24 June 2022",
                CustomerName = "Abir Mansour",
                Description = "3x Lemon&Mint Hooka Witty",
                Total = "USD45"
            });
            concessions.Add(new Concession_VM
            {
                Id = 718203,
                Date = "23 June 2022",
                CustomerName = "Hasan Bdeir",
                Description = "1x Lemon&Mint Hooka Witty ",
                Total = "USD15"
            });
        }
        #endregion

        public IActionResult Indoor()
        {
            FillSales();
            List<ConcessionsList_VM> concessionList = concessions.Select(x => new ConcessionsList_VM
            {
                Id = x.Id,
                Date = x.Date,
                CustomerName = x.CustomerName,
                Description = x.Description,
                Total = x.Total,
            }).ToList();

            return View("~/Areas/Restaurant/Views/Pages/Concessions/Indoor.cshtml", concessionList);
        }

        public IActionResult Outdoor()
        {
            FillSales();
            List<ConcessionsList_VM> concessionList = concessions.Select(x => new ConcessionsList_VM
            {
                Id = x.Id,
                Date = x.Date,
                CustomerName = x.CustomerName,
                Description = x.Description,
                Total = x.Total,
            }).ToList();

            return View("~/Areas/Restaurant/Views/Pages/Concessions/Outdoor.cshtml", concessionList);
        }
    }
}

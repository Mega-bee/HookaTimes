using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HookaTimes.BLL.IServices
{
    public interface IProductBL
    {
        Task<ResponseModel> GetAllCategories(HttpRequest request);
        Task<ResponseModel> GetProductsByCategoryId(int id, HttpRequest request);
        Task<List<Product_VM>> GetAllProductsMVC(int userBuddyId, HttpRequest request, string sessionWishlistId, int take = 0);
        Task<ViewHookaProduct_VM> GetCategoryProductsMVC(int categoryId);
    }
}
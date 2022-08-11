using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HookaTimes.BLL.Service
{
    public class ProductBL : BaseBO, IProductBL
    {
        public ProductBL(IUnitOfWork unit, IMapper mapper, NotificationHelper notificationHelper) : base(unit, mapper, notificationHelper)
        {
        }
        #region Categories
        public async Task<ResponseModel> GetAllCategories(HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();

            List<ProductCategories_VM> categories = await _uow.ProductCategoryRepository.GetAll().Select(x => new ProductCategories_VM
            {
                Decription = x.Description,
                Id = x.Id,
                Title = x.Title,
                Image = $"{request.Scheme}://{request.Host}/{x.Image}"

            }).ToListAsync();

            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = categories, Message = "" };
            return responseModel;

        }
        #endregion

        #region Products
        public async Task<ResponseModel> GetProductsByCategoryId(int id, HttpRequest request)
        {
            ResponseModel responseModel = new ResponseModel();

            bool productExist = await _uow.ProductRepository.CheckIfExists(x => x.ProductCategoryId == id && x.IsDeleted == false);
            if (!productExist)
            {
                responseModel.ErrorMessage = "Products was not Found";
                responseModel.StatusCode = 404;
                responseModel.Data = new DataModel { Data = "", Message = "" };
                return responseModel;

            }
            List<Product_VM> products = await _uow.ProductRepository.GetAll(x => x.ProductCategoryId == id && x.IsDeleted == false).Select(p => new Product_VM
            {
                Category = p.ProductCategory.Title,
                Title = p.Title,
                CustomerInitialPrice = p.CustomerInitialPrice,
                Description = p.Description,
                Id = p.Id,
                Image = $"{request.Scheme}://{request.Host}/{p.Image}",
            }).ToListAsync();

            responseModel.ErrorMessage = "";
            responseModel.StatusCode = 200;
            responseModel.Data = new DataModel { Data = products, Message = "" };
            return responseModel;

        }
        #endregion




    }
}

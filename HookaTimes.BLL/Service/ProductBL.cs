﻿using AutoMapper;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL.ViewModels.Website;
using HookaTimes.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<List<Product_VM>> GetAllProductsMVC(int userBuddyId, HttpRequest request, int take = 0)
        {
            var query =  _uow.ProductCategoryRepository.GetAll(x => x.IsDeleted == false);
            List<Product_VM> products = Array.Empty<Product_VM>().ToList();
            if(take == 0)
            {
                products = await query.Select(c => new Product_VM
                {
                    Category = c.Title,
                    IsInCart = c.Products.Where(p => p.IsDeleted == false).FirstOrDefault()!.Carts.Any(ci => ci.BuddyId == userBuddyId),
                    IsInWishlist = false,
                    Title = c.Title,
                    CustomerInitialPrice = c.Products.Where(p => p.IsDeleted == false).Select(p => p.CustomerFinalPrice).FirstOrDefault(),
                    Description = c.Description,
                    Id = c.Id,
                    Image = $"{request.Scheme}://{request.Host}/{c.Products.Where(p => p.IsDeleted == false).Select(p => p.Image).FirstOrDefault()}",
                }).ToListAsync();
            } else
            {
                products = await query.Select(c => new Product_VM
                {
                    Category = c.Title,
                    IsInCart = c.Products.Where(p => p.IsDeleted == false).FirstOrDefault()!.Carts.Any(ci => ci.BuddyId == userBuddyId),
                    IsInWishlist = false,
                    Title = c.Title,
                    CustomerInitialPrice = c.Products.Where(p => p.IsDeleted == false).Select(p => p.CustomerFinalPrice).FirstOrDefault(),
                    Description = c.Description,
                    Id = c.Id,
                    Image = $"{request.Scheme}://{request.Host}/{c.Products.Where(p => p.IsDeleted == false).Select(p => p.Image).FirstOrDefault()}",
                }).Take(take).ToListAsync();
            }
          
            return products;
        }

        public async Task<ViewHookaProduct_VM> GetCategoryProductsMVC(int categoryId)
        {
            ViewHookaProduct_VM product = await _uow.ProductCategoryRepository.GetAll(x => x.Id == categoryId && x.IsDeleted == false).Select(c => new ViewHookaProduct_VM
            {
                CategoryId = c.Id,
                CategoryTitle = c.Title,
                 CategoryImage = c.Image,
                Products = c.Products.Where(p => p.IsDeleted == false).Select(p => new HookaProduct_VM
                {
                    Title = p.Title,
                    CustomerFinalPrice = p.CustomerFinalPrice,
                    Id = p.Id,
                    Image = p.Image,
                     Description = p.Description
                }).ToList()
            }).FirstOrDefaultAsync();
            return product;
        }
        #endregion




    }
}

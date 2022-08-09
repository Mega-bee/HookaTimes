using AutoMapper;
//using HookaTimes.DAL.Models;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Service;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.Utilities.ActionFilters;
using HookaTimes.DAL;
using HookaTimes.DAL.HookaTimesModels;
using HookaTimes.DAL.Repos;
using HookaTimes.DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HookaTimes.BLL
{
    public class ServiceInjector
    {
        private readonly IServiceCollection _services;
        public ServiceInjector(IServiceCollection services)
        {
            _services = services;
        }

        public void Render()
        {
            _services.AddScoped<BaseBO>();

            _services.AddScoped<IAuthBO, AuthBO>();
            _services.AddScoped<IHookaPlaceBL, HookaPlaceBL>();
            _services.AddScoped<IHookaBuddyBL, HookaBuddyBL>();
            _services.AddScoped<IInvitationBL, InvitationBL>();
            _services.AddScoped<IProductBL, ProductBL>();
            _services.AddScoped<IOfferBL, OfferBL>();
            _services.AddScoped<IUnitOfWork, UnitOfWork>();
            //_services.AddScoped<IGenericRepos<AccProfile>, GenericRepos<AccProfile>>();
            _services.AddScoped<IGenericRepos<AspNetUser>, GenericRepos<AspNetUser>>();
            _services.AddScoped<IGenericRepos<PlacesProfile>, GenericRepos<PlacesProfile>>();
            _services.AddScoped<IGenericRepos<BuddyProfile>, GenericRepos<BuddyProfile>>();
            _services.AddScoped<IGenericRepos<Invitation>, GenericRepos<Invitation>>();
            _services.AddScoped<IGenericRepos<InvitationOption>, GenericRepos<InvitationOption>>();
            _services.AddScoped<IGenericRepos<OfferType>, GenericRepos<OfferType>>();
            _services.AddScoped<IGenericRepos<PlaceOffer>, GenericRepos<PlaceOffer>>();
            _services.AddScoped<IGenericRepos<ProductCategory>, GenericRepos<ProductCategory>>();
            _services.AddScoped<IGenericRepos<Product>, GenericRepos<Product>>();
            _services.AddScoped<IGenericRepos<PlaceOffer>, GenericRepos<PlaceOffer>>();
            _services.AddScoped<NotificationHelper>();
            _services.AddScoped<ValidationFilterAttribute>();


            var configurationMapper = new MapperConfiguration(option =>
            {
                option.AddProfile(new UserProfile());

            });
            var mapper = configurationMapper.CreateMapper();
            _services.AddSingleton(mapper);

        }
    }
}

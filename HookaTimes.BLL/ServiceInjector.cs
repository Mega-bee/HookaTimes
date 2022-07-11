using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HookaTimes.DAL;

using HookaTimes.DAL.Repos;
using HookaTimes.DAL.Services;
using HookaTimes.DAL.Models;
using HookaTimes.BLL.Utilities.Logging;
using HookaTimes.BLL.ViewModels;
using HookaTimes.BLL;
using HookaTimes.BLL.IServices;
using HookaTimes.BLL.Service;
using HookaTimes.BLL.Utilities;
using HookaTimes.BLL.Utilities.ActionFilters;

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
            _services.AddScoped<IUnitOfWork, UnitOfWork>();
            _services.AddScoped<IGenericRepos<AccProfile>, GenericRepos<AccProfile>>();
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

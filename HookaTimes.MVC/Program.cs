using HookaTimes.BLL;
using HookaTimes.BLL.Utilities.Extensions;
using HookaTimes.BLL.Utilities.Logging;
using HookaTimes.LoggerService;
using HookaTimes.MVC.Areas._keenthemes;
using HookaTimes.MVC.Areas._keenthemes.libs;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
    options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
});
builder.Services.AddScoped<IKTTheme, KTTheme>();
builder.Services.AddSingleton<IKTBootstrapBase, KTBootstrapBase>();

// Add services to the container.
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization(Microsoft.AspNetCore.Mvc.Razor.LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] {
            new CultureInfo("ja"),
            new CultureInfo("en"),
            new CultureInfo("ar"),
            new CultureInfo("de"),
            new CultureInfo("es"),
            new CultureInfo("fr"),
        };
    options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureAuthentication();
//new ServiceInjector(builder.Services).Render();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
var mvcBuilder = builder.Services.AddRazorPages();

if (builder.Environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();
}
var app = builder.Build();

app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/not-found";
        await next();
    }
});

KTThemeSettings.init(configuration);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();
app.UseThemeMiddleware();

//app.MapControllerRoute(name: "signin",
//                pattern: "signin",
//                defaults: new {area="Admin", controller = "Auth", action = "signIn" });
//app.MapControllerRoute(name: "signup",
//                pattern: "signup",
//                defaults: new { area = "Admin", controller = "Auth", action = "signUp" });
//app.MapControllerRoute(name: "reset-password",
//                pattern: "reset-password",
//                defaults: new { area = "Admin", controller = "Auth", action = "resetPassword" });
//app.MapControllerRoute(name: "new-password",
//                pattern: "new-password",
//                defaults: new { area = "Admin", controller = "Auth", action = "newPassword" });

//app.MapControllerRoute(name: "not-found",
//                pattern: "not-found",
//                defaults: new { area = "Admin", controller = "System", action = "notFound" });
//app.MapControllerRoute(name: "error",
//                pattern: "error",
//                defaults: new { area = "Admin", controller = "System", action = "error" });

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapControllerRoute(
//      name: "areas",
//      pattern: "{area:exists}/{controller=Dashboards}/{action=Index}/{id?}"
//    );
//    //endpoints.MapControllerRoute(
//    //name: "default",
//    //pattern: "{controller=Home}/{action=Index}/{id?}");
//});

app.MapControllerRoute(
      name: "default",
      pattern: "{area=Admin}/{controller=Dashboards}/{action=Index}/{id?}"
    );


//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");





app.Run();

using AspNetCore›dentity.Web.ClaimProvider;
using AspNetCore›dentity.Web.Extension;
using AspNetCore›dentity.Repository.Models;
using AspNetCore›dentity.Core.OptionsModel;
using AspNetCore›dentity.Web.Requirement;
using AspNetCore›dentity.Repository.SeedData;
using AspNetCore›dentity.Web.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using AspNetCore›dentity.Repository.Models;
using AspNetCore›dentity.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var cs = builder.Configuration.GetConnectionString("BaglantiCumlem");
builder.Services.AddDbContext<UygulamaDbContext>(o => o.UseSqlServer(cs));

builder.Services.AddIdentiyWithEx();
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IClaimsTransformation, UserClaimProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ExchangeExpireRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ViolenceRequirementHandler>();
builder.Services.AddScoped<IMemberService, MemberService>();

builder.Services.AddAuthorization(opt =>
{
    opt.AddPolicy("AnkaraPolicy", policy =>
    {
        policy.RequireClaim("city", "ankara");
    });

    opt.AddPolicy("ExchangePolicy", policy =>
    {
        policy.AddRequirements(new ExchangeExpireRequirement());
    });

    opt.AddPolicy("ViolencePolicy", policy =>
    {
        policy.AddRequirements(new ViolenceRequirement() { ThresholdAge = 18 });
    });
});

//cookiler icin ayarlamalar burada yapilir...
builder.Services.ConfigureApplicationCookie(option =>
{
    var cookieBuilder = new CookieBuilder();

    cookieBuilder.Name = "IdentityCookie";


    option.LoginPath = new PathString("/Home/Signin");
    option.Cookie = cookieBuilder;
    option.ExpireTimeSpan= TimeSpan.FromHours(555);
    option.SlidingExpiration = true; // cookie timespan gunceller,



});
//resim eklemek icin 
builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<UygulamaRole>>();

    PermissionSeed.Seed(roleManager);
}

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




app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

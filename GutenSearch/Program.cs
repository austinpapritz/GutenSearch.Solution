using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GutenSearch.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Read configuration for rate limiter.
var configuration = builder.Configuration;

// Configure AspNetCoreRateLimit.
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Context builder.
builder.Services.AddDbContext<GutenSearchContext>(
  dbContextOptions => dbContextOptions
    .UseMySql(
      builder.Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(builder.Configuration["ConnectionStrings:DefaultConnection"]
    )
  )
);

// Auth builder.
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
          .AddEntityFrameworkStores<GutenSearchContext>()
          .AddDefaultTokenProviders()
          .AddRoles<IdentityRole>();


// New code below to configure pw requirements during development (switch values to false/0)
builder.Services.Configure<IdentityOptions>(options =>
{
  // Default Password settings.
  options.Password.RequireDigit = false;
  options.Password.RequireLowercase = false;
  options.Password.RequireNonAlphanumeric = false;
  options.Password.RequireUppercase = false;
  options.Password.RequiredLength = 0;
  options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddAuthorization(options =>
{
  options.AddPolicy("RequireAdministratorRole",
       policy => policy.RequireRole("Librarian"));
});

//[Authorize(Policy = "RequireAdministratorRole")]

var app = builder.Build();

// Adding roles
using (var scope = app.Services.CreateScope())
{
  var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

  string[] roleNames = { "Librarian" };
  foreach (var roleName in roleNames)
  {
    var roleExist = roleManager.RoleExistsAsync(roleName).Result;
    if (!roleExist)
    {
      roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
    }
  }
}


// FOR DEVELOPMENT ONLY.
DataInitializer.InitializeData(app);

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

// Add Auth to app.
app.UseAuthentication();
app.UseAuthorization();

// AspNetCoreRateLimit.
app.UseIpRateLimiting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using MercuryShopping.Repository.AdminRepository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<IOrdersRepo, OrdersRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IReviewsRepo, ReviewsRepo>();
builder.Services.AddScoped<IAdminRepo, AdminRepo>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});



//*****************************************************************************
var conString = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<MercuryShopping.Models.MyDBContext>(opts => opts.UseSqlServer(conString));



builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

//*************************************************************************

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors("AllowAllOrigins");/////////////




app.UseSession();////





app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Customers}/{action=SearchProducts}");

app.Run();

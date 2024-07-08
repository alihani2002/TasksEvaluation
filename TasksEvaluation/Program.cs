using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TasksEvaluation.Core.DTOs;
using TasksEvaluation.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using TasksEvaluation.Areas.Identity.Data;
using TasksEvaluation.Infrastructure.Helpers;
using TasksEvaluation.Core.Entities.Business;
using TasksEvaluation.Core.Interfaces.IRepositories;
using TasksEvaluation.Core.Interfaces.IServices;
using TasksEvaluation.Infrastructure.Services;
using TasksEvaluation.Infrastructure.Repository;
using TasksEvaluation.Core.Mapper;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConfig"));

});

builder.Services.AddAutoMapper(typeof(MappingProfile));

// Register StudentService as a scoped service
builder.Services.AddScoped<IStudentService, StudentService>();

// Register repositories
builder.Services.AddScoped<IBaseRepository<Student>, BaseRepository<Student>>();  // Adjust based on actual repository implementation

// Register mappers
builder.Services.AddScoped<IBaseMapper<Student, StudentDTO>, BaseMapper<Student, StudentDTO>>();
builder.Services.AddScoped<IBaseMapper<StudentDTO, Student>, BaseMapper<StudentDTO, Student>>();


builder.Services.AddIdentity<ApplicationUser,IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultUI()
    .AddDefaultTokenProviders();

// Add Fluent Validation
builder.Services.AddValidatorsFromAssembly(typeof(CourseDTO).Assembly);


var app = builder.Build();

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

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

//Seed Data In DataBase 
DbInitializer.Seed(app);

app.Run();

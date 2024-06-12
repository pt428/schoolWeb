using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolWeb2.Models;
using SchoolWeb2.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddDbContext<AppDbContext>(
//	options => options.UseSqlServer(
//		builder.Configuration["ConnectionStrings:AzureDb"] ));

//builder.Services.AddDbContext<AppDbContext>(
//	options => options.UseSqlServer(
//		builder.Configuration["ConnectionStrings:SomeeDb"]));
builder.Services.AddDbContext<AppDbContext>(
	options => options.UseSqlServer(
		builder.Configuration["ConnectionStrings:MonsterDb"]));
builder.Services.AddScoped<StudentService>();
builder.Services.AddScoped<SubjectService>();
builder.Services.AddScoped<GradeService>();
builder.Services.AddIdentity<AppUser,IdentityRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(
	opt => {
		opt.Password.RequiredLength = 8;
		opt.Password.RequireNonAlphanumeric = false;
		opt.Password.RequireDigit = false;
		opt.Password.RequireLowercase = false;	
		opt.Password.RequireUppercase = false;
		 
		}
	);
builder.Services.ConfigureApplicationCookie(
	opt => {
		opt.LoginPath = "/Account/Login";
		opt.Cookie.Name = ".AspNetCore.Identity.Application";
		opt.ExpireTimeSpan = TimeSpan.FromMinutes( 5 );
		opt.SlidingExpiration=true;
	});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseDeveloperExceptionPage();
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

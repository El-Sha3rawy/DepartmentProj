using ApplicationLayer.DepartmentsCqrs.Behaviours;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConString"),
        sql => sql.MigrationsAssembly("Infrastructure")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(DepartmentMapping));

builder.Services.AddMediatR(m =>
{
    m.RegisterServicesFromAssembly(typeof(GetDepartmentByIdQuery).Assembly);
});

builder.Services.AddGrpc();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssembly(typeof(CreateDepartmentValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();

builder.Services.AddCors(o => o.AddPolicy("AllowAll", policy =>
{
    policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod();

}));
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = new[] { "Admin", "Manager", "Employee" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    if (await userManager.FindByEmailAsync("Admin@Dept.Com") == null)
    {
        var admin = new IdentityUser { UserName = "DeptAdmin", Email = "Admin@Dept.Com" };
        await userManager.CreateAsync(admin, "Admin@123");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    if (await userManager.FindByEmailAsync("Manager@Dept.Com") == null)
    {
        var manager = new IdentityUser { UserName = "DeptManager", Email = "Manager@Dept.Com" };
        await userManager.CreateAsync(manager, "Manager@123");
        await userManager.AddToRoleAsync(manager, "Manager");
    }

    if (await userManager.FindByEmailAsync("Employee@Dept.Com") == null)
    {
        var employee = new IdentityUser { UserName = "DeptEmployee", Email = "Employee@Dept.Com" };
        await userManager.CreateAsync(employee, "Employee@123");
        await userManager.AddToRoleAsync(employee, "Employee");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DepartmentGrpcServices>()
                .EnableGrpcWeb()
                .RequireCors("AllowAll");

    endpoints.MapGrpcService<AuthGrpcService>()
             .EnableGrpcWeb()
             .RequireCors("AllowAll");
});


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();

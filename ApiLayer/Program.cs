using ApiLayer.Services;
using ApplicationLayer.DepartmentsCqrs.Behaviours;
using ApplicationLayer.DepartmentsCqrs.Vailidators;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using InfrastructureLayer;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConString"),
        sql => sql.MigrationsAssembly("InfrastructureLayer")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMediatR(m =>
{
    m.RegisterServicesFromAssemblies(
        Assembly.GetExecutingAssembly(),
        typeof(ApplicationLayer.DepartmentsCqrs.Handlers.DeptCommandHandlers).Assembly
    );
});

builder.Services.AddGrpc();

builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssembly(typeof(CreateDeptValidator).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

builder.Services.AddScoped<IRepository, Repository>();


builder.Services.AddCors(o => o.AddPolicy("AllowAll", policy =>
{
    policy
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
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
        await userManager.CreateAsync(admin,"Admin@123");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    if (await userManager.FindByEmailAsync("Manager@Dept.Com") == null)
    {
        var manager = new IdentityUser { UserName = "DeptManager", Email = "Manager@Dept.Com" };
        await userManager.CreateAsync(manager,"Manager@123");
        await userManager.AddToRoleAsync(manager, "Manager");
    }

    if (await userManager.FindByEmailAsync("Employee@Dept.Com") == null)
    {
        var employee = new IdentityUser { UserName = "DeptEmployee", Email = "Employee@Dept.Com" };
        await userManager.CreateAsync(employee,"Employee@123");
        await userManager.AddToRoleAsync(employee, "Employee");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<DepartmentGrpcService>()
             .EnableGrpcWeb()
             .RequireCors("AllowAll");

    endpoints.MapGrpcService<AuthGrpcService>()
             .EnableGrpcWeb()
             .RequireCors("AllowAll");

    endpoints.MapControllers().RequireCors("AllowAll");

    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("gRPC API running. Use gRPC client or Blazor WASM (gRPC-Web).");
    });
});

app.Run();

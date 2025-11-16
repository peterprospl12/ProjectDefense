using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ProjectDefense.Application.Interfaces;
using ProjectDefense.Domain.Entities;
using ProjectDefense.Infrastructure.Configuration;
using ProjectDefense.Infrastructure.Identity;
using ProjectDefense.Infrastructure.Persistence;
using ProjectDefense.Infrastructure.Persistence.Repositories;
using ProjectDefense.Infrastructure.Services;
using ProjectDefense.Web.Api;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("ProjectDefense.Infrastructure")));

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
        options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
    });

builder.Services.AddHttpClient();

builder.Services.Configure<MailGunSettings>(builder.Configuration.GetSection("MailGun"));

builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IEmailSender, EmailService>(); // Also register as IEmailSender for Identity's default pages

builder.Services.AddValidatorsFromAssembly(typeof(ProjectDefense.Application.Behaviors.ValidationBehavior<,>).Assembly);

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ProjectDefense.Application.Behaviors.ValidationBehavior<,>));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ProjectDefense.Application.Behaviors.ValidationBehavior<,>).Assembly));

builder.Services.AddScoped<IAvailabilityRepository, AvailabilityRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IStudentBlockRepository, StudentBlockRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    await RoleConfiguration.SeedRolesAsync(scope.ServiceProvider);
    await RoleConfiguration.SeedLecturerAsync(scope.ServiceProvider);
    await RoleConfiguration.SeedStudentsAsync(scope.ServiceProvider);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapSlotsEndpoints();

app.Run();
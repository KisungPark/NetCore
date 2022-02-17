using Microsoft.EntityFrameworkCore;
using Net.Data;
using Net.Service.Data;
using Net.Service.Interfaces;
using Net.Service;
using Microsoft.AspNetCore.DataProtection;
using Net.Utilities.Utils;
using Microsoft.AspNetCore.Authentication.Cookies;
using Net.Service.Services;

#region Build option
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Data Protection
Common.SetDataProtection(builder.Services, @"C:\Users\allocation\source\repos\Net\Web\DataProtector\", "Net", Enums.CryptoType.CngCbc);

// IUser�� ������ ����
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// CodeFirstDB ���̱׷��̼� �۾�
// Microsoft.EntityFrameworkCore.Tools ��ġ
// [PS] Add-Migration AddingTestTable -Context CodeFirstDbContext -Project Net.Service
// [PS] update-database -project Net.Service
//builder.Services.AddDbContext<CodeFirstDbContext>(options => options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString(name: "CodeFirst"), sqlServerOptionsAction: mig => mig.MigrationsAssembly(assemblyName: "NetCore.Service")));

// DBFirstDB ����������
builder.Services.AddDbContext<DBFirstDbContext>(options => options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString(name: "DBFirst")));

// MVC ���
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

//�ſ������� ���α���
builder.Services.AddAuthentication(defaultScheme:CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Membership/Forbidden";
        options.LoginPath = "/Membership/Login";
    });
builder.Services.AddAuthorization();

//���� ����
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".NetCore.Session";
    //���� ���ѽð�
    options.IdleTimeout = TimeSpan.FromMinutes(30); //�⺻���� 20��
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

/**********
app.UseRouting(), app.UseAuthentication(), app.UseAuthorization(),
app.UseSession(), app.UseEndpoints()
�̷��� 5���� �޼���� �ݵ�� ������ ���Ѿ� �ùٷ� �۵���.
**********/

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
//�����
app.UseRouting();
//�ſ�����
app.UseAuthentication();
app.UseAuthorization();
//����
app.UseSession();
//���Ʈ ���
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using Microsoft.Extensions.Options;
using Stripe;
using Tarumt.WAM.Assignment.Extensions;
using Tarumt.WAM.Assignment.Infrastructure.Models;
using Tarumt.WAM.Assignment.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseConfig(builder.Configuration, builder.Environment);
builder.Services.AddAuthenticationConfig(builder.Configuration, builder.Environment);
builder.Services.AddMvcConfig(builder.Configuration, builder.Environment);
builder.Services.AddResponseCompressionConfig();
builder.Services.AddServiceConfig();
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseResponseCompression();
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseUserMiddleware();
app.UseStaticFiles();
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value);
app.MapControllerRoute(
    "areaRoute",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe")["SecretKey"];

await app.RunAsync();

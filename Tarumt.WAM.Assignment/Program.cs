using Microsoft.Extensions.Options;
using Tarumt.WAM.Assignment.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDatabaseConfig(builder.Configuration, builder.Environment);
builder.Services.AddAuthenticationConfig(builder.Configuration, builder.Environment);
builder.Services.AddMvcConfig(builder.Configuration, builder.Environment);
builder.Services.AddResponseCompressionConfig();
builder.Services.AddServiceConfig();

var app = builder.Build();
app.UseResponseCompression();
app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRequestLocalization(app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value);
app.MapControllerRoute(
    "areaRoute",
    "{area:exists}/{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();

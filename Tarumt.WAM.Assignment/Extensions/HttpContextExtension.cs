namespace Tarumt.WAM.Assignment.Extensions
{
    public static class HttpContextExtension
    {
        public static bool IsTurbolinksRequest(this HttpContext ctx) =>
               ctx.Request.Headers.ContainsKey("Turbolinks-Referrer");
        }
}

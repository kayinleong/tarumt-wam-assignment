namespace Tarumt.WAM.Assignment.Infrastructure.Models
{
    public class StripeSettings
    {
        public required string SecretKey { get; set; }
        public required string PublishableKey { get; set; }
    }
}

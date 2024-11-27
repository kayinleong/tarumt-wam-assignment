namespace Tarumt.WAM.Assignment.Extensions
{
    public static class DecimalExtensions
    {
        public static string ToRoundedString(this decimal d, int decimalPlaces)
        {
            return d.ToString("N" + decimalPlaces);
        }
    }
}

using System.Globalization;

namespace BusinessClicker.Data
{
    public static class FloatExtension
    {
        public static void ApplyPercent(this ref float target, float percent)
        {
            target *= percent / 100 + 1;
        }
        
        public static string ToInvariantString(this float target)
        {
            return target.ToString(CultureInfo.InvariantCulture);
        }
    }
}
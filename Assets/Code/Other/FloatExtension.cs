namespace BusinessClicker.Data
{
    public static class FloatExtension
    {
        public static void ApplyPercent(this ref float target, float percent)
        {
            target *= percent / 100 + 1;
        }
    }
}
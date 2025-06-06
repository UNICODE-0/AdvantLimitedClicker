using BusinessClicker.Components;

namespace BusinessClicker.Data
{
    public static class BusinessHelper
    {
        public static void CalculateLvlPrice(ref BusinessComponent business)
        {
            business.LvlUpPrice = (business.Lvl + 1) * business.Cfg.BasicPrice;
        }
        
        public static void CalculateIncome(ref BusinessComponent business)
        {
            business.Income = business.Lvl * business.Cfg.BasicIncome;

            if (business.Upgrade1Status)
                business.Income.ApplyPercent(business.Cfg.Upgrade1.IncomeMultiplier);
                
            if (business.Upgrade2Status)
                business.Income.ApplyPercent(business.Cfg.Upgrade2.IncomeMultiplier);
        }
    }
}
using System;
using Sirenix.OdinInspector;
using TMPro;
using Unity.IL2CPP.CompilerServices;

namespace BusinessClicker.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BusinessUpgradeViewComponent
    {
        [Required]
        public BusinessUpgradeView Upgrade1View;
        
        [Required]
        public BusinessUpgradeView Upgrade2View;
    }
    
    [Serializable]
    public class BusinessUpgradeView
    {
        [Required]
        public TMP_Text LabelField;
        
        [Required]
        public TMP_Text IncomeMultiplierField;
        
        [Required]
        public TMP_Text StatusField;
    }
}
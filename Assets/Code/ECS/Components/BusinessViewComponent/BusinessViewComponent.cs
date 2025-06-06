using Sirenix.OdinInspector;
using TMPro;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;

namespace BusinessClicker.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BusinessViewComponent
    {
        [Required]
        public Image ProgressBar;
        
        [Required]
        public TMP_Text LabelField;
        
        [Required]
        public TMP_Text LvlField;
        
        [Required]
        public TMP_Text IncomeField;
        
        [Required]
        public TMP_Text LvlUpPriceField;
    }
}
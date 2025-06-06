using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace BusinessClicker.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BusinessUpgradeComponent
    {
        [Required]
        public CustomButton LvlUpButton;
        
        [Required]
        public CustomButton Upgrade1Button;
        
        [Required]
        public CustomButton Upgrade2Button;
    }
}
using TMPro;
using Unity.IL2CPP.CompilerServices;

namespace BusinessClicker.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BalanceViewComponent
    {
        public TMP_Text BalanceField;
    }
}
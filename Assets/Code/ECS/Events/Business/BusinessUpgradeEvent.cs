using Unity.IL2CPP.CompilerServices;

namespace BusinessClicker.Events
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BusinessUpgradeEvent
    {
        public UpgradeVariant Variant;
    }

    public enum UpgradeVariant
    {
        First,
        Second
    }
}
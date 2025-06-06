using BusinessClicker.Components;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace BusinessClicker.Mono.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    
    [RequireComponent(typeof(BusinessButtonsProvider))]
    public sealed class BusinessUpgradeViewProvider : MonoProvider<BusinessUpgradeViewComponent>
    {
    }
}
using BusinessClicker.Components;
using Unity.IL2CPP.CompilerServices;
using UnityEngine;

namespace BusinessClicker.Mono.Providers
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    
    public sealed class BalanceViewProvider : MonoProvider<BalanceViewComponent>
    {
    }
}
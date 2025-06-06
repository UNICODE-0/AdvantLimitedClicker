using BusinessClicker.Data;
using BusinessClicker.SO;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;

namespace BusinessClicker.Components
{
    [System.Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct BusinessComponent
    {
        [Required]
        public BusinessCfg Cfg;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ProgressBar(0, 1)]
        [ReadOnly] public float IncomeProgress;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public int CurrentLVL;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public int CurrentIncome;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool Upgrade1Status;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public bool Upgrade2Status;
    }
}
using BusinessClicker.Data;
using BusinessClicker.SO;
using Sirenix.OdinInspector;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.Serialization;

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
        
        [MinValue(0)]
        public int Lvl;
        
        public bool Upgrade1Status;
        public bool Upgrade2Status;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ProgressBar(0, 1)]
        [ReadOnly] public float IncomeProgress;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public float Income;
        
        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public float ProgressTime;

        [FoldoutGroup(Consts.COMPONENT_RUNTIME_FOLDOUT_NAME)]
        [ReadOnly] public float LvlUpPrice;
    }
}
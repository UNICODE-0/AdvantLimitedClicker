using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace BusinessClicker.SO
{
    [CreateAssetMenu(fileName = "BusinessCfg", menuName = "ScriptableObjects/BusinessCfg", order = 1)]
    public class BusinessCfg : ScriptableObject
    {
        public string Name;
        
        [Unit(Units.Second)]
        [MinValue(1)]
        public int IncomeFrequency = 3;
        
        [MinValue(1)]
        public int BasicPrice = 3;
        
        [MinValue(1)]
        public int BasicIncome = 3;
        
        [BoxGroup("Upgrade 1")]
        [HideLabel]
        public BusinessUpgrade Upgrade1;
        
        [BoxGroup("Upgrade 2")]
        [HideLabel]
        public BusinessUpgrade Upgrade2;
    }

    [Serializable]
    public class BusinessUpgrade
    {
        public string Name;
        
        [MinValue(1)]
        public int Price = 50;
        
        [Unit(Units.Percent)]
        [MinValue(1)]
        public int IncomeMultiplier = 50;

    }
}
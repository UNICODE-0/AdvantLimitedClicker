using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BusinessClicker.SO
{
    // Возможность изменения количества улучшений у бизнеса (BusinessUpgrade) не реализовывал,
    // так как в требованиях указано: "расширяемость кода обеспечивать не нужно"
    [CreateAssetMenu(fileName = "BusinessCfg", menuName = "ScriptableObjects/BusinessCfg", order = 1)]
    public class BusinessCfg : ScriptableObject
    {
        public int Id;
        
        [Unit(Units.Second)]
        [MinValue(1)]
        public float IncomeFrequency = 3;
        
        [MinValue(1)]
        public float BasicPrice = 3;
        
        [MinValue(1)]
        public float BasicIncome = 3;
        
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
        [MinValue(1)]
        public float Price = 50;
        
        [Unit(Units.Percent)]
        [MinValue(1)]
        public float IncomeMultiplier = 50;

    }
}
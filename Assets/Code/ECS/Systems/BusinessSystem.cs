using BusinessClicker.Components;
using BusinessClicker.Data;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Systems
{
    public class BusinessSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<BusinessComponent> _businessPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessComponent>().End();
            _businessPool = world.GetPool<BusinessComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                business.ProgressTime += Time.deltaTime;
                business.IncomeProgress = business.ProgressTime / business.Cfg.IncomeFrequency;

                business.Income = business.Lvl * business.Cfg.BasicIncome;

                if (business.Upgrade1Status)
                    business.Income.ApplyPercent(business.Cfg.Upgrade1.IncomeMultiplier);
                
                if (business.Upgrade2Status)
                    business.Income.ApplyPercent(business.Cfg.Upgrade2.IncomeMultiplier);

                business.LvlUpPrice = (business.Lvl + 1) * business.Cfg.BasicPrice;

                if (business.IncomeProgress >= 1f)
                {
                    business.ProgressTime = 0;
                    business.IncomeProgress = 0;
                }
            }
        }
    }
}
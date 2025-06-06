using BusinessClicker.Components;
using BusinessClicker.Data;
using BusinessClicker.Events;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Systems
{
    public class BusinessSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessLvlUpEvent> _lvlUpEventPool;
        private EcsPool<BusinessUpgrade1Event> _upgrade1EventPool;
        private EcsPool<BusinessUpgrade2Event> _upgrade2EventPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessComponent>().End();
            
            _businessPool = world.GetPool<BusinessComponent>();
            _lvlUpEventPool = world.GetPool<BusinessLvlUpEvent>();
            _upgrade1EventPool = world.GetPool<BusinessUpgrade1Event>();
            _upgrade2EventPool = world.GetPool<BusinessUpgrade2Event>();

            InitializeBusinesses();
        }
        
        private void InitializeBusinesses()
        {
            foreach (int entity in _filter)
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                
                CalculateLvlPrice(ref business);
                CalculateIncome(ref business);
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);

                HandleButtonClicks(ref business, entity);
                
                business.ProgressTime += Time.deltaTime;
                business.IncomeProgress = Mathf.Clamp01(business.ProgressTime / business.Cfg.IncomeFrequency);

                if (business.IncomeProgress >= 1f)
                {
                    business.ProgressTime = 0;
                    business.IncomeProgress = 0;
                }
            }
        }
        
        private void HandleButtonClicks(ref BusinessComponent business, int entity)
        {
            if (business.LvlUpButton.ClickedThisFrame)
            {
                LvlUp(ref business);
                _lvlUpEventPool.Add(entity);
            }

            if (business.Upgrade1Button.ClickedThisFrame)
            {
                ApplyUpgrade1(ref business);
                _upgrade1EventPool.Add(entity);
            }

            if (business.Upgrade2Button.ClickedThisFrame)
            {
                ApplyUpgrade2(ref business);
                _upgrade2EventPool.Add(entity);
            }
        }
        
        private void ApplyUpgrade1(ref BusinessComponent business)
        {
            business.Upgrade1Status = true;
            CalculateIncome(ref business);
        }
        
        private void ApplyUpgrade2(ref BusinessComponent business)
        {
            business.Upgrade2Status = true;
            CalculateIncome(ref business);
        }
        
        private void LvlUp(ref BusinessComponent business)
        {
            business.Lvl += 1;
            CalculateLvlPrice(ref business);
            CalculateIncome(ref business);
        }

        private void CalculateLvlPrice(ref BusinessComponent business)
        {
            business.LvlUpPrice = (business.Lvl + 1) * business.Cfg.BasicPrice;
        }
        
        private void CalculateIncome(ref BusinessComponent business)
        {
            business.Income = business.Lvl * business.Cfg.BasicIncome;

            if (business.Upgrade1Status)
                business.Income.ApplyPercent(business.Cfg.Upgrade1.IncomeMultiplier);
                
            if (business.Upgrade2Status)
                business.Income.ApplyPercent(business.Cfg.Upgrade2.IncomeMultiplier);
        }
    }
}
using BusinessClicker.Components;
using BusinessClicker.Data;
using BusinessClicker.Events;
using Leopotam.EcsLite;
using UnityEngine;
using System.Collections.Generic;

namespace BusinessClicker.Systems
{
    public class BusinessSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private EcsFilter _filter;
        
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessLvlUpEvent> _lvlUpEventPool;
        private EcsPool<BusinessUpgrade1Event> _upgrade1EventPool;
        private EcsPool<BusinessUpgrade2Event> _upgrade2EventPool;
        private EcsPool<BalanceChangeEvent> _balanceChangeEventPool;

        private Profile _profile;
        private SaveManager _saveManager;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessComponent>().End();
            
            _businessPool = world.GetPool<BusinessComponent>();
            _lvlUpEventPool = world.GetPool<BusinessLvlUpEvent>();
            _upgrade1EventPool = world.GetPool<BusinessUpgrade1Event>();
            _upgrade2EventPool = world.GetPool<BusinessUpgrade2Event>();
            _balanceChangeEventPool = world.GetPool<BalanceChangeEvent>();

            var shared = systems.GetShared<SharedData>();
            _profile = shared.Profile;
            _saveManager = shared.SaveManager;

            InitializeBusinesses();
        }
        
        private void InitializeBusinesses()
        {
            var businesses = _saveManager.LoadBusinesses();
            
            foreach (int entity in _filter)
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                
                if(businesses != null)
                {
                    int id = business.Cfg.Id;
                    business.Lvl = businesses[id].Lvl;
                    business.ProgressTime = businesses[id].ProgressTime;
                    business.Upgrade1Status = businesses[id].Upgrade1Status;
                    business.Upgrade2Status = businesses[id].Upgrade2Status;
                }
                
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
                if (business.Lvl > 0)
                {
                    business.ProgressTime += Time.deltaTime;
                    business.IncomeProgress = Mathf.Clamp01(business.ProgressTime / business.Cfg.IncomeFrequency);

                    if (business.IncomeProgress >= 1f)
                    {
                        _profile.Balance += business.Income;
                        business.ProgressTime = 0;
                        business.IncomeProgress = 0;
                        _balanceChangeEventPool.Add(entity);
                    }
                }
            }
        }
        
        private void HandleButtonClicks(ref BusinessComponent business, int entity)
        {
            if (business.LvlUpButton.ClickedThisFrame && 
                _profile.Balance >= business.LvlUpPrice)
            {
                _profile.Balance -= business.LvlUpPrice;
                LvlUp(ref business);
                _lvlUpEventPool.Add(entity);
                _balanceChangeEventPool.Add(entity);
            }

            if (business.Upgrade1Button.ClickedThisFrame &&
                _profile.Balance >= business.Cfg.Upgrade1.Price)
            {
                _profile.Balance -= business.Cfg.Upgrade1.Price;
                ApplyUpgrade1(ref business);
                _upgrade1EventPool.Add(entity);
                _balanceChangeEventPool.Add(entity);
            }

            if (business.Upgrade2Button.ClickedThisFrame &&
                _profile.Balance >= business.Cfg.Upgrade2.Price)
            {
                _profile.Balance -= business.Cfg.Upgrade2.Price;
                ApplyUpgrade2(ref business);
                _upgrade2EventPool.Add(entity);
                _balanceChangeEventPool.Add(entity);
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

        public void Destroy(IEcsSystems systems)
        {
            Dictionary<int, BusinessSaveData> saveData = new Dictionary<int, BusinessSaveData>();
            foreach (int entity in _filter)
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                saveData.Add(business.Cfg.Id, new BusinessSaveData()
                {
                    Lvl = business.Lvl,
                    ProgressTime = business.ProgressTime,
                    Upgrade1Status = business.Upgrade1Status,
                    Upgrade2Status = business.Upgrade2Status
                });
            }

            _saveManager.SaveBusinesses(saveData);
        }
    }
}
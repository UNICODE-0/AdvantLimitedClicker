using BusinessClicker.Components;
using BusinessClicker.Data;
using BusinessClicker.Events;
using Leopotam.EcsLite;
using UnityEngine;
using System.Collections.Generic;
using Unity.IL2CPP.CompilerServices;

namespace BusinessClicker.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class BusinessSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem, IEcsApplicationPauseSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BalanceChangeEvent> _balanceChangeEventPool;

        private Profile _profile;
        private SaveManager _saveManager;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessComponent>().End();
            
            _businessPool = world.GetPool<BusinessComponent>();
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
                ref var business = ref _businessPool.Get(entity);
                
                if(businesses != null)
                {
                    int id = business.Cfg.Id;
                    business.Lvl = businesses[id].Lvl;
                    business.ProgressTime = businesses[id].ProgressTime;
                    business.Upgrade1Status = businesses[id].Upgrade1Status;
                    business.Upgrade2Status = businesses[id].Upgrade2Status;
                }
                
                BusinessHelper.CalculateLvlPrice(ref business);
                BusinessHelper.CalculateIncome(ref business);
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref var business = ref _businessPool.Get(entity);
                
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
        
        public void SaveData()
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
        
        public void Destroy(IEcsSystems systems)
        {
            SaveData();
        }
        public void Pause(IEcsSystems systems, bool status)
        {
            if(status) SaveData();
        }
    }
}
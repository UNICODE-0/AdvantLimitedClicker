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
    public class BusinessUpgradeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessUpgradeComponent> _businessUpgradePool;
        
        private EcsPool<BusinessLvlUpEvent> _lvlUpEventPool;
        private EcsPool<BusinessUpgradeEvent> _upgradeEventPool;
        private EcsPool<BalanceChangeEvent> _balanceChangeEventPool;

        private Profile _profile;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessComponent>()
                .Inc<BusinessUpgradeComponent>()
                .End();
            
            _businessPool = world.GetPool<BusinessComponent>();
            _businessUpgradePool = world.GetPool<BusinessUpgradeComponent>();
            
            _lvlUpEventPool = world.GetPool<BusinessLvlUpEvent>();
            _upgradeEventPool = world.GetPool<BusinessUpgradeEvent>();
            _balanceChangeEventPool = world.GetPool<BalanceChangeEvent>();

            _profile = systems.GetShared<SharedData>().Profile;
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref var business = ref _businessPool.Get(entity);
                ref var businessUpgrade = ref _businessUpgradePool.Get(entity);

                if (businessUpgrade.LvlUpButton.ClickedThisFrame && 
                    _profile.Balance >= business.LvlUpPrice)
                {
                    _profile.Balance -= business.LvlUpPrice;
                    LvlUp(ref business);
                    _lvlUpEventPool.Add(entity);
                    _balanceChangeEventPool.Add(entity);
                }

                if (businessUpgrade.Upgrade1Button.ClickedThisFrame &&
                    _profile.Balance >= business.Cfg.Upgrade1.Price)
                {
                    _profile.Balance -= business.Cfg.Upgrade1.Price;
                    ApplyUpgrade1(ref business);
                    _upgradeEventPool.Add(entity).Variant = UpgradeVariant.First;
                    _balanceChangeEventPool.Add(entity);
                }

                if (businessUpgrade.Upgrade2Button.ClickedThisFrame &&
                    _profile.Balance >= business.Cfg.Upgrade2.Price)
                {
                    _profile.Balance -= business.Cfg.Upgrade2.Price;
                    ApplyUpgrade2(ref business);
                    _upgradeEventPool.Add(entity).Variant = UpgradeVariant.Second;
                    _balanceChangeEventPool.Add(entity);
                }
            }
        }
        
        private void ApplyUpgrade1(ref BusinessComponent business)
        {
            business.Upgrade1Status = true;
            BusinessHelper.CalculateIncome(ref business);
        }
        
        private void ApplyUpgrade2(ref BusinessComponent business)
        {
            business.Upgrade2Status = true;
            BusinessHelper.CalculateIncome(ref business);
        }
        
        private void LvlUp(ref BusinessComponent business)
        {
            business.Lvl += 1;
            BusinessHelper.CalculateLvlPrice(ref business);
            BusinessHelper.CalculateIncome(ref business);
        }
    }
}
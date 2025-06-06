using BusinessClicker.Components;
using BusinessClicker.Data;
using BusinessClicker.Events;
using BusinessClicker.SO;
using Leopotam.EcsLite;
using Unity.IL2CPP.CompilerServices;

namespace BusinessClicker.Systems
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class BusinessViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessViewComponent> _businessViewPool;
        private EcsPool<BusinessLvlUpEvent> _lvlUpEventPool;
        private EcsPool<BusinessUpgradeEvent> _upgradeEventPool;

        private TermsListSO _terms;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessViewComponent>()
                .Inc<BusinessComponent>()
                .End();
            
            _businessPool = world.GetPool<BusinessComponent>();
            _businessViewPool = world.GetPool<BusinessViewComponent>();
            _lvlUpEventPool = world.GetPool<BusinessLvlUpEvent>();
            _upgradeEventPool = world.GetPool<BusinessUpgradeEvent>();
            
            _terms = systems.GetShared<SharedData>().TermsManager.TermsList; 
            
            InitializeViews();
        }
        
        private void InitializeViews()
        {
            foreach (int entity in _filter)
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                ref BusinessViewComponent businessView = ref _businessViewPool.Get(entity);
                
                UpdateAll(ref business, ref businessView);
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                ref BusinessViewComponent businessView = ref _businessViewPool.Get(entity);
                
                UpdateProgress(ref business, ref businessView);
                HandleEvents(entity, ref business, ref businessView);
            }
        }

        private void HandleEvents(int entity, ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            if (_lvlUpEventPool.Has(entity))
            {
                UpdateLvl(ref business, ref businessView);
                _lvlUpEventPool.Del(entity);
            }
                
            if (_upgradeEventPool.Has(entity))
            {
                ref var upgradeEvent = ref _upgradeEventPool.Get(entity);
                switch (upgradeEvent.Variant)
                {
                    case UpgradeVariant.First:
                        UpdateUpgradeStatus(business.Upgrade1Button, business.Upgrade1Status, ref business.Cfg.Upgrade1,
                            ref businessView.Upgrade1View);
                        UpdateIncome(ref business, ref businessView);
                        _upgradeEventPool.Del(entity);
                        break;
                    case UpgradeVariant.Second:
                        UpdateUpgradeStatus(business.Upgrade2Button, business.Upgrade2Status, ref business.Cfg.Upgrade2,
                            ref businessView.Upgrade2View);
                        UpdateIncome(ref business, ref businessView);
                        break;
                }
                
                _upgradeEventPool.Del(entity);
            }
        }

        private void UpdateAll(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LabelField.text = _terms.Businesses[business.Cfg.Id].Name;

            UpdateProgress(ref business, ref businessView);
            UpdateLvl(ref business, ref businessView);
            
            UpdateUpgrade(ref business.Cfg.Upgrade1, ref businessView.Upgrade1View,
                _terms.Businesses[business.Cfg.Id].Upgrade1);
            UpdateUpgradeStatus(business.Upgrade1Button, business.Upgrade1Status, ref business.Cfg.Upgrade1,
                ref businessView.Upgrade1View);
            
            UpdateUpgrade(ref business.Cfg.Upgrade2, ref businessView.Upgrade2View,
                _terms.Businesses[business.Cfg.Id].Upgrade2);
            UpdateUpgradeStatus(business.Upgrade2Button, business.Upgrade2Status, ref business.Cfg.Upgrade2,
                ref businessView.Upgrade2View);
        }

        public void UpdateProgress(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.ProgressBar.fillAmount = business.IncomeProgress;
        }
        
        public void UpdateIncome(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.IncomeField.text = $"{business.Income}{_terms.Currency}";
        }
        
        private void UpdateLvl(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LvlField.text = business.Lvl.ToString();
            businessView.LvlUpPriceField.text = $"{_terms.Price}: {business.LvlUpPrice}{_terms.Currency}";
            UpdateIncome(ref business, ref businessView);
        }
        
        private void UpdateUpgrade(ref BusinessUpgrade upgrade, ref BusinessUpgradeView view, string upgradeName)
        {
            view.LabelField.text = upgradeName;
            view.IncomeMultiplierField.text = $"{_terms.Income}: +{upgrade.IncomeMultiplier}%";
        }

        private void UpdateUpgradeStatus(CustomButton button, bool status, ref BusinessUpgrade upgrade, ref BusinessUpgradeView view)
        {
            if (status)
            {
                view.StatusField.text = _terms.Purchased;
                button.interactable = false;
            }
            else
            {
                view.StatusField.text = $"{_terms.Price}: {upgrade.Price}{_terms.Currency}";
                button.interactable = true;
            }
        }
    }
}
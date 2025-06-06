using BusinessClicker.Components;
using BusinessClicker.Data;
using BusinessClicker.Events;
using BusinessClicker.Mono;
using BusinessClicker.SO;
using Leopotam.EcsLite;

namespace BusinessClicker.Systems
{
    public class BusinessViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessViewComponent> _businessViewPool;
        private EcsPool<BusinessLvlUpEvent> _lvlUpEventPool;
        private EcsPool<BusinessUpgrade1Event> _upgrade1EventPool;
        private EcsPool<BusinessUpgrade2Event> _upgrade2EventPool;

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
            _upgrade1EventPool = world.GetPool<BusinessUpgrade1Event>();
            _upgrade2EventPool = world.GetPool<BusinessUpgrade2Event>();
            
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
                
            if (_upgrade1EventPool.Has(entity))
            {
                businessView.Upgrade1View.StatusField.text = _terms.Purchased;
                UpdateIncome(ref business, ref businessView);
                _upgrade1EventPool.Del(entity);
            }
                
            if (_upgrade2EventPool.Has(entity))
            {
                businessView.Upgrade2View.StatusField.text = _terms.Purchased;
                UpdateIncome(ref business, ref businessView);
                _upgrade2EventPool.Del(entity);
            }
        }

        private void UpdateAll(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LabelField.text = _terms.Businesses[business.Cfg.Id].Name;

            UpdateProgress(ref business, ref businessView);
            UpdateLvl(ref business, ref businessView);
            UpdateUpgradeView(business.Cfg.Upgrade1, ref businessView.Upgrade1View,
                _terms.Businesses[business.Cfg.Id].Upgrade1);
            UpdateUpgradeView(business.Cfg.Upgrade2, ref businessView.Upgrade2View,
                _terms.Businesses[business.Cfg.Id].Upgrade2);
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
        
        private void UpdateUpgradeView(in BusinessUpgrade upgrade, ref BusinessUpgradeView view, string upgradeName)
        {
            view.LabelField.text = upgradeName;
            view.IncomeMultiplierField.text = $"{_terms.Income}: +{upgrade.IncomeMultiplier}%";
            view.StatusField.text = $"{_terms.Price}: {upgrade.Price}{_terms.Currency}";
        }
    }
}
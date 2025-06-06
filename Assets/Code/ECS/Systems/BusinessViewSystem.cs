using BusinessClicker.Components;
using BusinessClicker.Data;
using BusinessClicker.Events;
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
                UpdateUpgrade(ref business.Cfg.Upgrade1, ref businessView.Upgrade1View);
                UpdateIncome(ref business, ref businessView);
                _upgrade1EventPool.Del(entity);
            }
                
            if (_upgrade2EventPool.Has(entity))
            {
                UpdateUpgrade(ref business.Cfg.Upgrade2, ref businessView.Upgrade2View);
                UpdateIncome(ref business, ref businessView);
                _upgrade2EventPool.Del(entity);
            }
        }

        private void UpdateAll(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LabelField.text = business.Cfg.Name;

            UpdateProgress(ref business, ref businessView);
            UpdateLvl(ref business, ref businessView);
            UpdateUpgrade(ref business.Cfg.Upgrade1, ref businessView.Upgrade1View);
            UpdateUpgrade(ref business.Cfg.Upgrade2, ref businessView.Upgrade2View);
        }

        public void UpdateProgress(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.ProgressBar.fillAmount = business.IncomeProgress;
        }
        
        public void UpdateIncome(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.IncomeField.text = business.Income.ToInvariantString();
        }
        
        private void UpdateLvl(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LvlField.text = business.Lvl.ToString();
            businessView.LvlUpPriceField.text = business.LvlUpPrice.ToInvariantString();
            UpdateIncome(ref business, ref businessView);
        }
        
        private void UpdateUpgrade(ref BusinessUpgrade upgrade, ref BusinessUpgradeView view)
        {
            view.LabelField.text = upgrade.Name;
            view.IncomeMultiplierField.text = upgrade.IncomeMultiplier.ToInvariantString();
            view.StatusField.text = upgrade.Price.ToInvariantString();
        }
    }
}
using BusinessClicker.Components;
using BusinessClicker.Data;
using BusinessClicker.SO;
using Leopotam.EcsLite;

namespace BusinessClicker.Systems
{
    public class BusinessViewSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        
        private EcsPool<BusinessComponent> _businessPool;
        private EcsPool<BusinessViewComponent> _businessViewPool;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessViewComponent>()
                .Inc<BusinessComponent>()
                .End();
            
            _businessPool = world.GetPool<BusinessComponent>();
            _businessViewPool = world.GetPool<BusinessViewComponent>();
            
            InitializeViews();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                ref BusinessViewComponent businessView = ref _businessViewPool.Get(entity);
                
                UpdateProgress(ref business, ref businessView);
            }
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

        private void UpdateAll(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            UpdateProgress(ref business, ref businessView);
            UpdateLabel(ref business, ref businessView);
            UpdateLvl(ref business, ref businessView);
            UpdateIncome(ref business, ref businessView);
            UpdateLvlUpPrice(ref business, ref businessView);
            UpdateUpgrade(ref business.Cfg.Upgrade1, ref businessView.Upgrade1View);
            UpdateUpgrade(ref business.Cfg.Upgrade2, ref businessView.Upgrade2View);
        }
        
        private void UpdateProgress(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.ProgressBar.fillAmount = business.IncomeProgress;
        }
        
        private void UpdateLabel(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LabelField.text = business.Cfg.Name;
        }
        
        private void UpdateLvl(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LvlField.text = business.Lvl.ToString();
        }
        
        private void UpdateIncome(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.IncomeField.text = business.Income.ToInvariantString();
        }
        
        private void UpdateLvlUpPrice(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.LvlUpPriceField.text = business.LvlUpPrice.ToInvariantString();
        }
        
        private void UpdateUpgrade(ref BusinessUpgrade upgrade, ref BusinessUpgradeView view)
        {
            view.LabelField.text = upgrade.Name;
            view.IncomeMultiplierField.text = upgrade.IncomeMultiplier.ToInvariantString();
            view.StatusField.text = upgrade.Price.ToInvariantString();
        }
    }
}
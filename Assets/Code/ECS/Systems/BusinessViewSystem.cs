using System.Globalization;
using BusinessClicker.Components;
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
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
                ref BusinessViewComponent businessView = ref _businessViewPool.Get(entity);

                businessView.ProgressBar.fillAmount = business.IncomeProgress;

                businessView.LabelField.text = business.Cfg.Name;
                businessView.LvlField.text = business.Lvl.ToString();
                businessView.IncomeField.text = business.Income.ToString(CultureInfo.InvariantCulture);
                businessView.LvlUpPriceField.text = business.LvlUpPrice.ToString(CultureInfo.InvariantCulture);
                
                businessView.Upgrade1View.LabelField.text = business.Cfg.Upgrade1.Name;
                businessView.Upgrade1View.IncomeMultiplierField.text =
                    business.Cfg.Upgrade1.IncomeMultiplier.ToString(CultureInfo.InvariantCulture);
                businessView.Upgrade1View.StatusField.text =
                    business.Cfg.Upgrade1.Price.ToString(CultureInfo.InvariantCulture);
                
                businessView.Upgrade2View.LabelField.text = business.Cfg.Upgrade2.Name;
                businessView.Upgrade2View.IncomeMultiplierField.text =
                    business.Cfg.Upgrade2.IncomeMultiplier.ToString(CultureInfo.InvariantCulture);
                businessView.Upgrade2View.StatusField.text =
                    business.Cfg.Upgrade2.Price.ToString(CultureInfo.InvariantCulture);
                
                
            }
        }
    }
}
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

        private TermsListSO _terms;

        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessViewComponent>()
                .Inc<BusinessUpgradeComponent>()
                .Inc<BusinessComponent>()
                .End();
            
            _businessPool = world.GetPool<BusinessComponent>();
            _businessViewPool = world.GetPool<BusinessViewComponent>();
            
            _terms = systems.GetShared<SharedData>().TermsManager.TermsList; 
            
            InitializeViews();
        }
        
        private void InitializeViews()
        {
            foreach (int entity in _filter)
            {
                ref var business = ref _businessPool.Get(entity);
                ref var businessView = ref _businessViewPool.Get(entity);

                businessView.LabelField.text = _terms.Businesses[business.Cfg.Id].Name;
                UpdateProgress(ref business, ref businessView);
            }
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref var business = ref _businessPool.Get(entity);
                ref var businessView = ref _businessViewPool.Get(entity);

                UpdateProgress(ref business, ref businessView);
            }
        }
        

        public void UpdateProgress(ref BusinessComponent business, ref BusinessViewComponent businessView)
        {
            businessView.ProgressBar.fillAmount = business.IncomeProgress;
        }
    }
}
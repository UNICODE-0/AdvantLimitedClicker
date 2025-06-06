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
    public class BalanceViewSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem, IEcsApplicationPauseSystem
    {
        private EcsFilter _balanceFilter;
        private EcsFilter _balanceChangeEventFilter;

        private EcsPool<BalanceViewComponent> _balanceViewPool;
        private EcsPool<BalanceChangeEvent> _balanceChangeEventPool;

        private SharedData _shared;
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _balanceFilter = world.Filter<BalanceViewComponent>()
                .End();
            
            _balanceChangeEventFilter = world.Filter<BalanceChangeEvent>()
                .End();
            
            _balanceViewPool = world.GetPool<BalanceViewComponent>();
            _balanceChangeEventPool = world.GetPool<BalanceChangeEvent>();

            _shared = systems.GetShared<SharedData>();

            InitializeBalance();
        }

        public void InitializeBalance()
        {
            float amount = _shared.SaveManager.LoadBalance();
            _shared.Profile.Balance = amount;
            
            foreach (int entity in _balanceFilter) 
            {
                ref var balanceView = ref _balanceViewPool.Get(entity);
                UpdateBalance(ref balanceView);
            }
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _balanceFilter) 
            {
                ref var balanceView = ref _balanceViewPool.Get(entity);
                HandleEvents(ref balanceView);
            }
        }

        public void HandleEvents(ref BalanceViewComponent view)
        {
            if (_balanceChangeEventFilter.GetEntitiesCount() > 0)
            {
                UpdateBalance(ref view);
                foreach (var entity in _balanceChangeEventFilter)
                {
                    _balanceChangeEventPool.Del(entity);
                }
            }
        }

        public void UpdateBalance(ref BalanceViewComponent view)
        {
            view.BalanceField.text =
                $"{_shared.TermsManager.TermsList.Balance}: {_shared.Profile.Balance}{_shared.TermsManager.TermsList.Currency}";
        }

        public void SaveData()
        {
            _shared.SaveManager.SaveBalance(_shared.Profile.Balance);
        }

        public void Destroy(IEcsSystems systems)
        {
            SaveData();
        }

        public void Pause(IEcsSystems systems, bool status)
        {
            if (status) SaveData();
        }
    }
}
using BusinessClicker.Components;
using Leopotam.EcsLite;

namespace BusinessClicker.Systems
{
    public class BusinessSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<BusinessComponent> _businessPool;
        
        public void Init(IEcsSystems systems)
        {
            EcsWorld world = systems.GetWorld();
            _filter = world.Filter<BusinessComponent>().End();
            _businessPool = world.GetPool<BusinessComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (int entity in _filter) 
            {
                ref BusinessComponent business = ref _businessPool.Get(entity);
            }
        }
    }
}
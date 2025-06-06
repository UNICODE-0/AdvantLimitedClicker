using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BusinessClicker.Mono.Providers
{
    public class MonoProvider<T> : EntityProvider where T: struct 
    {
        [HideLabel]
        [SerializeField]
        private T _serializedData;
        
        private EcsPool<T> _pool;
        public EcsPool<T> Pool => _pool ??= EcsWorld.Default.GetPool<T>();


        protected override void Initialize()
        {
            Pool.Add(_cachedEntity);
        }
    }
}
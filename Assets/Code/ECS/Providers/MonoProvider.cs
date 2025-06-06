using System;
using Leopotam.EcsLite;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BusinessClicker.Mono.Providers
{
    public class MonoProvider<T> : EntityProvider where T: struct
    {
        [HideLabel]
        [SerializeField]
        [DisableInPlayMode]
        private T _serializedData;
        
        private EcsPool<T> _pool;
        public EcsPool<T> Pool => _pool ??= EcsWorld.Default.GetPool<T>();

        protected override void Initialize()
        {
            Pool.Add(_cachedEntity) = _serializedData;
        }

#if UNITY_EDITOR
        [OnInspectorGUI]
        private void OnInspectorGUI()
        {
            if(_cachedEntity == -1) return;
            _serializedData = Pool.Get(_cachedEntity);
        }
#endif
    }
}
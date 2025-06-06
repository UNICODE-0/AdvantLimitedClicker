using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

namespace BusinessClicker.Mono.Providers
{
    public class EntityProvider : MonoBehaviour
    {
        public static readonly Dictionary<int, int> Entities = new();

        protected int _cachedEntity = -1;

        private void OnEnable()
        {
            SetOrCreateEntity();
        }

        private void SetOrCreateEntity()
        {
            if (_cachedEntity == -1) {
                var instanceId = gameObject.GetInstanceID();
                if (Entities.TryGetValue(instanceId, out var entity))
                {
                    _cachedEntity = entity;
                }
                else {
                    _cachedEntity = entity = EcsWorld.Default.NewEntity();
                    Entities.Add(instanceId, entity);
                }
            }
        }

        protected virtual void Initialize(){}
    }
}
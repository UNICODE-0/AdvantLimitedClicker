using BusinessClicker.Data;
using BusinessClicker.Systems;
using UnityEngine;
using Leopotam.EcsLite;

namespace BusinessClicker.Mono
{
    public class EcsStartup : MonoBehaviour
    {
        private EcsWorld _world;
        private IEcsSystems _systems;

        private void Start()
        {
            SharedData sharedData = new SharedData()
            {
                TermsManager = new TermsManager()
            };
            
            _world = EcsWorld.Default;
            _systems = new EcsSystems(_world, sharedData);
            _systems
                .Add (new BusinessSystem())
                .Add (new BusinessViewSystem())
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_world != null)
            {
                _world.Destroy();
                _world = null;
            }
        }
    }
}
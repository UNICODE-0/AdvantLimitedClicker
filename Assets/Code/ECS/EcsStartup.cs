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
            Application.targetFrameRate = 120;
            
            SharedData sharedData = new SharedData()
            {
                TermsManager = new TermsManager(),
                Profile = new Profile(),
                SaveManager = new SaveManager()
            };
            
            _world = EcsWorld.Default;
            _systems = new EcsSystems(_world, sharedData);
            _systems
                .Add(new BusinessUpgradeSystem())
                .Add (new BusinessSystem())
                .Add (new BusinessViewSystem())
                .Add (new BalanceViewSystem())
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

#if !UNITY_EDITOR
        private void OnApplicationPause(bool pauseStatus) 
        {
            _systems.OnApplicationPause(pauseStatus);
        }
#endif
    }
}
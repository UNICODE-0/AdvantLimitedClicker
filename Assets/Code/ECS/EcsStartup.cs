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
            _world = EcsWorld.Default;
            _systems = new EcsSystems(_world);
            _systems
                // .Add (new TestSystem1 ())
                // .Add (new TestSystem2 ())
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
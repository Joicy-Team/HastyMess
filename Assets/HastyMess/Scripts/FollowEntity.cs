using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace HastyMess.Scripts
{
    public class FollowEntity : MonoBehaviour
    {
        private EntityManager _manager;
        public Entity EntityToFollow;

        private void Awake()
        {
            _manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        }

        private void LateUpdate()
        {
            if (EntityToFollow == Entity.Null) return;

            var entPos = _manager.GetComponentData<Translation>(EntityToFollow);
            transform.position = entPos.Value;
        }
    }
}
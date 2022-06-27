using Unity.Entities;
using UnityEngine;

namespace HastyMess.Scripts.Data
{
    [AddComponentMenu("DOTS/Components/Leader Authoring")]
    public class LeaderAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public GameObject followerObject;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (!followerObject.TryGetComponent<FollowEntity>(out var followEntity))
                followEntity = followerObject.AddComponent<FollowEntity>();

            followEntity.EntityToFollow = entity;
        }
    }
}
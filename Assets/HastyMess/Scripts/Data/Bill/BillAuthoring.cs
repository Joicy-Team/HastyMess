using Unity.Entities;
using UnityEngine;

namespace HastyMess.Scripts.Data
{
    [AddComponentMenu("DOTS/Components/Bill Authoring")]
    public class BillAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float speed;
        public float dashForce;
        public float dashCooldown;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var data = new BillComponent
            {
                Speed = speed * 10,
                DashForce = dashForce * 100,
                DashCooldown = dashCooldown,
                CooldownPassed = dashCooldown
            };
            dstManager.AddComponentData(entity, data);
        }
    }
}
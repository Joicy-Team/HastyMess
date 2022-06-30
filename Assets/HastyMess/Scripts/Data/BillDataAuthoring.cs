using System;
using System.Diagnostics.CodeAnalysis;
using Unity.Entities;
using UnityEngine;

namespace HastyMess.Scripts.Data
{
    [Serializable]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public struct BillData : IComponentData
    {
        public float Speed;
        public float DashForce;
        public float DashCooldown;
        public float CooldownPassed;
        public Vector2 MoveDirection;
        public Vector2 LastMoveDirection;
        public bool DashAction;
    }

    [AddComponentMenu("DOTS/Components/Bill Data Authoring")]
    public class BillDataAuthoring : MonoBehaviour, IConvertGameObjectToEntity
    {
        public float speed;
        public float dashForce;
        public float dashCooldown;

        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var data = new BillData
            {
                Speed = speed * 100,
                DashForce = dashForce * 100,
                DashCooldown = dashCooldown,
                CooldownPassed = dashCooldown
            };
            dstManager.AddComponentData(entity, data);
        }
    }
}
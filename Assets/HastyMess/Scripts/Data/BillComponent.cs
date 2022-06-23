using Unity.Entities;
using Unity.Mathematics;

namespace HastyMess.Scripts.Data
{
    [GenerateAuthoringComponent]
    public struct BillComponent : IComponentData
    {
        public float Speed;
        public float DashForce;
        public float DashCooldown;
        public float CooldownPassed;
        public float2 MoveDirection;
        public float2 LastMoveDirection;
    }
}
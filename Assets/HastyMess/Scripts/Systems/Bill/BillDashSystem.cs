using HastyMess.Scripts.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;

namespace HastyMess.Scripts.Systems
{
    // ReSharper disable once PartialTypeWithSinglePart
    public partial class BillDashSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            Entities.ForEach(
                (
                    ref PhysicsVelocity velocity, ref BillComponent bill, in PhysicsMass mass
                ) =>
                {
                    var forceVector = bill.LastMoveDirection * bill.DashForce * deltaTime;

                    // Applies dash if dash key was performed this frame and dash cooldown has passed
                    if (!bill.DashAction || bill.CooldownPassed < bill.DashCooldown) return;
                    velocity.ApplyLinearImpulse(mass, new float3(forceVector, 0));
                    bill.CooldownPassed = 0f;
                }).Run();
        }
    }
}
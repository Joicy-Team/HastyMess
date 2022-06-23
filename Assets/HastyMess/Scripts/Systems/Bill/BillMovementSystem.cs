using HastyMess.Scripts.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;

namespace HastyMess.Scripts.Systems
{
    [UpdateAfter(typeof(BillControllerSystem))]
    public partial class BillMovementSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            Entities.ForEach((ref PhysicsVelocity velocity, in BillComponent bill) =>
            {
                var newVelocity = velocity.Linear.xy;

                // Don't update velocity if dashing
                if (!math.any(newVelocity >= new float2(bill.Speed, bill.Speed)))
                    newVelocity += bill.MoveDirection * bill.Speed * deltaTime;

                velocity.Linear.xy = newVelocity;
            }).Run();
        }
    }
}
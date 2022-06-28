using HastyMess.Scripts.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace HastyMess.Scripts.Systems
{
    [UpdateAfter(typeof(BillControllerSystem))]
    // ReSharper disable once RedundantExtendsListEntry
    public partial class BillMovementSystem : SystemBase
    {
        private static readonly int Horizontal = Animator.StringToHash("Horizontal");
        private static readonly int Vertical = Animator.StringToHash("Vertical");
        private static readonly int Speed = Animator.StringToHash("Speed");

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;

            // Movement
            Entities.ForEach((ref PhysicsVelocity velocity, in BillComponent bill) =>
            {
                var newVelocity = velocity.Linear.xy;

                // Don't update velocity if dashing
                if (!math.any(newVelocity >= new float2(bill.Speed, bill.Speed)))
                    newVelocity += bill.MoveDirection * bill.Speed * deltaTime;

                velocity.Linear.xy = newVelocity;
            }).Run();

            // Movement animations
            Entities.ForEach((in BillComponent bill, in ChildEntityComponent child) =>
            {
                if (!child.Parent.TryGetComponent(out Animator animator)) return;
                
                animator.SetFloat(Horizontal, bill.MoveDirection.x);
                animator.SetFloat(Vertical, bill.MoveDirection.y);
                animator.SetFloat(Speed, new Vector2(bill.MoveDirection.x, bill.MoveDirection.y).sqrMagnitude);
            }).WithoutBurst().Run();
        }
    }
}
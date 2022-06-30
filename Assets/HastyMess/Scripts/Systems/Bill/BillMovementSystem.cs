using HastyMess.Scripts.Data;
using Unity.Entities;
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

            Entities.ForEach((in BillData bill, in ChildEntity child) =>
            {
                // Movement
                if (child.Parent.TryGetComponent(out Rigidbody2D rb))
                {
                    var vel = rb.velocity;

                    // Don't update velocity if dashing
                    if (vel.x < bill.Speed && vel.y < bill.Speed)
                        vel += bill.MoveDirection * bill.Speed * deltaTime;

                    rb.velocity = vel;
                }

                // Animations
                if (!child.Parent.TryGetComponent(out Animator animator)) return;

                animator.SetFloat(Horizontal, bill.MoveDirection.x);
                animator.SetFloat(Vertical, bill.MoveDirection.y);
                animator.SetFloat(Speed, bill.MoveDirection.sqrMagnitude);
            }).WithoutBurst().Run();
        }
    }
}
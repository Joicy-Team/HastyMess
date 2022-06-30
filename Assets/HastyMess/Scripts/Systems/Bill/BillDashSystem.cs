using HastyMess.Scripts.Data;
using Unity.Entities;
using UnityEngine;

namespace HastyMess.Scripts.Systems
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once RedundantExtendsListEntry
    public partial class BillDashSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var deltaTime = Time.fixedDeltaTime;
            Entities.ForEach((ref BillData bill, in ChildEntity child) =>
            {
                // Applies dash if dash key was performed this frame and dash cooldown has passed
                if (!child.Parent.TryGetComponent(out Rigidbody2D rb)) return;
                if (!bill.DashAction || bill.CooldownPassed < bill.DashCooldown) return;

                var forceVector = bill.LastMoveDirection * bill.DashForce * deltaTime;
                rb.AddForce(forceVector, ForceMode2D.Impulse);
                bill.CooldownPassed = 0f;
            }).WithoutBurst().Run();
        }
    }
}
using HastyMess.Scripts.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HastyMess.Scripts.Systems
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once RedundantExtendsListEntry
    public partial class BillControllerSystem : SystemBase
    {
        private InputAction _dash;
        private GameInput _gameInput;
        private InputAction _move;

        protected override void OnCreate()
        {
            // Initializes input system
            _gameInput = new GameInput();
            _gameInput.Player.Enable();
            _move = _gameInput.Player.Move;
            _dash = _gameInput.Player.Dash;
        }

        protected override void OnUpdate()
        {
            var moveDirection = math.normalizesafe
            (new float2(
                _move.ReadValue<Vector2>().x,
                _move.ReadValue<Vector2>().y
            ));
            var dashAction = _dash.WasPerformedThisFrame();
            var deltaTime = Time.DeltaTime;
            var fixedDeltaTime = Time.fixedDeltaTime;

            Entities.ForEach(
                (
                    ref PhysicsVelocity velocity, ref Rotation rotation, ref BillComponent bill,
                    in PhysicsMass mass
                ) =>
                {
                    if (bill.CooldownPassed < bill.DashCooldown)
                        bill.CooldownPassed += deltaTime;

                    rotation.Value = Quaternion.Euler(0, 0, 0);
                    HandleMovement(ref bill, moveDirection);
                    HandleDash(ref velocity, ref bill, mass, dashAction, fixedDeltaTime);
                }).Run();
        }

        private static void HandleMovement(ref BillComponent bill, float2 moveDirection)
        {
            bill.MoveDirection = moveDirection;
            if (!moveDirection.Equals(new float2(0, 0)))
                bill.LastMoveDirection = moveDirection;
        }

        private static void HandleDash
        (
            ref PhysicsVelocity physicsVelocity, ref BillComponent bill,
            PhysicsMass physicsMass, bool dashAction, float deltaTime
        )
        {
            var forceVector = bill.LastMoveDirection * bill.DashForce * deltaTime;

            // Applies dash if dash key was performed this frame and dash cooldown has passed
            if (!dashAction || bill.CooldownPassed < bill.DashCooldown) return;
            physicsVelocity.ApplyLinearImpulse(physicsMass, new float3(forceVector, 0));
            bill.CooldownPassed = 0f;
        }
    }
}
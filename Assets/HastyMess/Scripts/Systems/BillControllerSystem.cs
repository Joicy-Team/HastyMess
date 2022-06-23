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
    public partial class BillControllerSystem : SystemBase
    {
        private GameInput _gameInput;
        private InputAction _move;

        protected override void OnCreate()
        {
            // Initializes input system
            _gameInput = new GameInput();
            _gameInput.Player.Enable();
            _move = _gameInput.Player.Move;
        }

        protected override void OnUpdate()
        {
            var moveDirection = new float3(_move.ReadValue<Vector2>().x, _move.ReadValue<Vector2>().y, 0);
            // Alternative for FixedUpdate()
            var deltaTime = Time.fixedDeltaTime;

            Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref PhysicsMass physicsMass, ref Rotation rotation,
                in BillComponent bill) =>
            {
                // Removes any rotation
                rotation.Value = quaternion.Euler(0, 0, 0);

                // If there's no input, stop the player
                if (moveDirection.Equals(new float3(0, 0, 0)))
                {
                    physicsVelocity.Linear = new float3(0, 0, 0);
                    return;
                }

                // TODO: Cap the maximum speed somehow
                var forceVector = moveDirection * bill.MoveSpeed * deltaTime;
                physicsVelocity.ApplyLinearImpulse(physicsMass, forceVector);
            }).Run();
        }
    }
}
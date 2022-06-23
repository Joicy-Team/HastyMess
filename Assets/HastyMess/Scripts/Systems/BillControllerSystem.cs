using HastyMess.Scripts.Data;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HastyMess.Scripts.Systems
{
    // ReSharper disable once PartialTypeWithSinglePart
    // ReSharper disable once RedundantExtendsListEntry
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
            var moveDirection =
                math.normalizesafe(new float3(_move.ReadValue<Vector2>().x, _move.ReadValue<Vector2>().y, 0));

            Entities.ForEach((ref PhysicsVelocity physicsVelocity, ref Rotation rotation, in BillComponent bill) =>
            {
                // Removes any rotation
                rotation.Value = quaternion.Euler(0, 0, 0);
                // Applies movement
                physicsVelocity.Linear = moveDirection * bill.MoveSpeed;
            }).Run();
        }
    }
}
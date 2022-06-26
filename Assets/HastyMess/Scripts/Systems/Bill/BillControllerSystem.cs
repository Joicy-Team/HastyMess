using HastyMess.Scripts.Data;
using Unity.Entities;
using Unity.Mathematics;
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
            var moveDirection = ReadMovement(_move);
            var dashAction = _dash.WasPerformedThisFrame();
            var timeDeltaTime = Time.DeltaTime;

            float2 ReadMovement(InputAction inputAction)
            {
                return math.normalizesafe
                (new float2(
                    inputAction.ReadValue<Vector2>().x,
                    inputAction.ReadValue<Vector2>().y
                ));
            }

            BillComponent CheckDashCooldown(BillComponent bill, float deltaTime)
            {
                if (bill.CooldownPassed < bill.DashCooldown)
                    bill.CooldownPassed += deltaTime;
                return bill;
            }

            Rotation FixRotation(Rotation rotation)
            {
                rotation.Value = Quaternion.Euler(0, 0, 0);
                return rotation;
            }

            Entities.ForEach((ref Rotation rotation, ref BillComponent bill) =>
            {
                bill = CheckDashCooldown(bill, timeDeltaTime);

                rotation = FixRotation(rotation);
                bill = HandleMovement(bill, moveDirection);
                bill = HandleDash(bill, dashAction);
            }).Run();
        }

        private static BillComponent HandleMovement(BillComponent bill, float2 moveDirection)
        {
            bill.MoveDirection = moveDirection;
            if (!moveDirection.Equals(new float2(0, 0)))
                bill.LastMoveDirection = moveDirection;
            return bill;
        }

        private static BillComponent HandleDash(BillComponent bill, bool dashAction)
        {
            bill.DashAction = dashAction;
            return bill;
        }
    }
}
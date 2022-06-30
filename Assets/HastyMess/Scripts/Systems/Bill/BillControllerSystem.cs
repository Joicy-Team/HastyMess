using HastyMess.Scripts.Data;
using Unity.Entities;
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
            var moveDirection = _move.ReadValue<Vector2>().normalized;
            var dashAction = _dash.WasPerformedThisFrame();
            var timeDeltaTime = Time.DeltaTime;

            Entities.ForEach((ref BillData bill) =>
            {
                bill = CheckDashCooldown(bill, timeDeltaTime);
                bill = HandleMovement(bill, moveDirection);
                bill = HandleDash(bill, dashAction);
            }).Run();
        }

        private static BillData CheckDashCooldown(BillData bill, float deltaTime)
        {
            if (bill.CooldownPassed < bill.DashCooldown)
                bill.CooldownPassed += deltaTime;
            return bill;
        }

        private static BillData HandleMovement(BillData bill, Vector2 moveDirection)
        {
            bill.MoveDirection = moveDirection;
            if (moveDirection != Vector2.zero)
                bill.LastMoveDirection = moveDirection;
            return bill;
        }

        private static BillData HandleDash(BillData bill, bool dashAction)
        {
            bill.DashAction = dashAction;
            return bill;
        }
    }
}
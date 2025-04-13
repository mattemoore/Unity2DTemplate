using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerStateIdle : PlayerState
    {
        public PlayerStateIdle(PlayerStateController playerStateController) : base(playerStateController)
        {
        }

        public override void OnCollisionEnter(Collision collision)
        {

        }

        public override void OnEnter()
        {
            // Debug.Log($"Entering {this} state.");
            PlayerInputController.MoveButtonPressed += MoveButtonPressedEventHandler;
            PlayerInputController.AttackButtonPressed += AttackButtonPressedEventHandler;
            PlayerAnimator.speed = 1f;
            PlayerAnimator.CrossFade("Idle", 0.0f, -1, 0.0f);
        }

        public override void OnExit()
        {
            PlayerInputController.MoveButtonPressed -= MoveButtonPressedEventHandler;
            PlayerInputController.AttackButtonPressed -= AttackButtonPressedEventHandler;
        }

        public override void UpdateState()
        {
            // Debug.Log($"Updating {this} state.");
        }

        public override string ToString()
        {
            return "Idle";
        }

        private void AttackButtonPressedEventHandler()
        {
            throw new NotImplementedException();
        }

        private void MoveButtonPressedEventHandler(InputMoveDirection moveDirection)
        {
            if (moveDirection != InputMoveDirection.None)
            {
                PlayerStateMoving moveState = new PlayerStateMoving(StateController, moveDirection);
                StateController.ChangeState(moveState);
            }
        }
    }
}
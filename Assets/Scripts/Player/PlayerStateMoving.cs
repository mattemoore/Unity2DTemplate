using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerStateMoving : PlayerState
    {
        private readonly InputMoveDirection _currentDirection;

        public PlayerStateMoving(PlayerStateController playerStateController, InputMoveDirection moveDirection) : base(playerStateController)
        {
            _currentDirection = moveDirection;
        }

        public override void OnCollisionEnter(Collision collision)
        {

        }

        public override void OnEnter()
        {
            PlayerInputController.MoveButtonPressed += MoveButtonPressedEventHandler;

            if (_currentDirection == InputMoveDirection.Left || _currentDirection == InputMoveDirection.Right)
            {
                float animatorSpeed = _currentDirection == InputMoveDirection.Right ? 1.0f : -1.0f;
                float normalizedTimeOffset = _currentDirection == InputMoveDirection.Right ? 0.0f : 1.0f;
                PlayerAnimator.SetFloat(Animator.StringToHash("AnimationSpeed"), animatorSpeed);
                PlayerAnimator.CrossFade("Walk", 0.0f, -1, normalizedTimeOffset);
            }
        }

        public override void OnExit()
        {
            PlayerInputController.MoveButtonPressed -= MoveButtonPressedEventHandler;
        }

        public override void UpdateState()
        {
            // Debug.Log($"Updating {this} state with direction {_currentDirection}.");
            if (_currentDirection == InputMoveDirection.Right)
            {
                PlayerController.MoveRight();
            }
            else if (_currentDirection == InputMoveDirection.Left)
            {
                PlayerController.MoveLeft();
            }
        }

        public override string ToString()
        {
            return "Moving";
        }

        private void MoveButtonPressedEventHandler(InputMoveDirection newDirection)
        {
            if (newDirection != _currentDirection)
            {
                StateController.ChangeState(newDirection != InputMoveDirection.None
                    ? new PlayerStateMoving(StateController, newDirection)
                    : new PlayerStateIdle(StateController));
            }
        }
    }
}
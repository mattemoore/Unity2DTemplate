using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterStateMoving : CharacterState
    {
        private readonly CharacterMovementDirection _currentMovementDirection;

        public CharacterStateMoving(CharacterStateController playerStateController, CharacterMovementDirection moveDirection) : base(playerStateController)
        {
            _currentMovementDirection = moveDirection;
        }

        public override void OnCollisionEnter(Collision collision)
        {

        }

        public override void OnEnter()
        {
            HumanBrain.MoveEvent += MoveEventHandler;

            if (_currentMovementDirection == CharacterMovementDirection.Forward || _currentMovementDirection == CharacterMovementDirection.Backward)
            {
                float animatorSpeed = _currentMovementDirection == CharacterMovementDirection.Forward ? 1.0f : -1.0f;
                float normalizedTimeOffset = _currentMovementDirection == CharacterMovementDirection.Backward ? 0.0f : 1.0f;
                CharacterAnimator.SetFloat(Animator.StringToHash("AnimationSpeed"), animatorSpeed);
                CharacterAnimator.CrossFade("Walk", 0.0f, -1, normalizedTimeOffset);
            }
        }

        public override void OnExit()
        {
            HumanBrain.MoveEvent -= MoveEventHandler;
        }

        public override void UpdateState()
        {
            // Debug.Log($"Updating {this} state with direction {_currentDirection}.");
            CharacterController.Move(_currentMovementDirection, 1.0f);
        }

        public override string ToString()
        {
            return "Moving";
        }

        private void MoveEventHandler(CharacterMovementDirection characterMovementDirection)
        {
            if (characterMovementDirection != _currentMovementDirection)
            {
                StateController.ChangeState(characterMovementDirection != CharacterMovementDirection.None
                    ? new CharacterStateMoving(StateController, characterMovementDirection)
                    : new CharacterStateIdle(StateController));
            }
        }
    }
}
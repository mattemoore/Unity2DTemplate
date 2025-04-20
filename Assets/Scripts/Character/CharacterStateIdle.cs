using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterStateIdle : CharacterState
    {
        public CharacterStateIdle(CharacterStateController playerStateController) : base(playerStateController)
        {
        }

        public override void OnCollisionEnter(Collision collision)
        {

        }

        public override void OnEnter()
        {
            // Debug.Log($"Entering {this} state.");
            HumanBrain.MoveEvent += MoveButtonPressedEventHandler;
            HumanBrain.Attack1Event += AttackButton1PressedEventHandler;
            HumanBrain.Attack2Event += AttackButton2PressedEventHandler;
            HumanBrain.Attack3Event += AttackButton3PressedEventHandler;
            HumanBrain.Attack4Event += AttackButton4PressedEventHandler;
            CharacterAnimator.speed = 1f;
            CharacterAnimator.CrossFade("Idle", 0.0f, -1, 0.0f);
        }

        public override void OnExit()
        {
            HumanBrain.MoveEvent -= MoveButtonPressedEventHandler;
            HumanBrain.Attack1Event -= AttackButton1PressedEventHandler;
            HumanBrain.Attack2Event -= AttackButton2PressedEventHandler;
            HumanBrain.Attack3Event -= AttackButton3PressedEventHandler;
            HumanBrain.Attack4Event -= AttackButton4PressedEventHandler;
        }

        public override void UpdateState()
        {
            // Debug.Log($"Updating {this} state.");
        }

        public override string ToString()
        {
            return "Idle";
        }

        private void AttackButton1PressedEventHandler()
        {
            CharacterStateHighKicking highKickState = new CharacterStateHighKicking(StateController);
            StateController.ChangeState(highKickState);
        }

        private void AttackButton2PressedEventHandler()
        {
            CharacterStateFrontKicking frontKickState = new CharacterStateFrontKicking(StateController);
            StateController.ChangeState(frontKickState);
        }

        private void AttackButton3PressedEventHandler()
        {
            // PlayerStateHighKicking attackState = new PlayerStateHighKicking(StateController);
            // StateController.ChangeState(attackState);
        }

        private void AttackButton4PressedEventHandler()
        {
            // PlayerStateHighKicking attackState = new PlayerStateHighKicking(StateController);
            // StateController.ChangeState(attackState);
        }

        private void MoveButtonPressedEventHandler(CharacterMovementDirection moveDirection)
        {
            if (moveDirection != CharacterMovementDirection.None)
            {
                CharacterStateMoving moveState = new CharacterStateMoving(StateController, moveDirection);
                StateController.ChangeState(moveState);
            }
        }
    }
}
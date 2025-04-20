using System;
using UnityEngine;

namespace Assets.Scripts
{
    // TODO: Merge all attacking states into one, pass in Attack Move Properties object that contains animation name, max cancel frame etc., movement left or right etc. move until x frame etc.)
    public class CharacterStateFrontKicking : CharacterState
    {
        public CharacterStateFrontKicking(CharacterStateController playerStateController) : base(playerStateController)
        {
        }

        public override void OnCollisionEnter(Collision collision)
        {

        }

        public override void OnEnter()
        {
            HumanBrain.Attack2Event += Attack2EventHandler;
            // Debug.Log($"Entering {this} state.");
            CharacterAnimator.speed = 1f;
            CharacterAnimator.CrossFade("FrontKick", 0.0f, -1, 0.0f);
        }

        public override void OnExit()
        {
            HumanBrain.Attack2Event -= Attack2EventHandler;
            // Debug.Log($"Exiting {this} state.");
        }

        public override void UpdateState()
        {
            // Debug.Log($"Updating {this} state.");
            int currentFrame = GetCurrentAnimationFrame("FrontKick");
            if (currentFrame <= 4)
                CharacterController.Move(CharacterMovementDirection.Forward, 2.0f);

            if (IsAnimationFinished("FrontKick"))
            {
                CharacterStateIdle idleState = new CharacterStateIdle(StateController);
                StateController.ChangeState(idleState);
            }
        }

        public override string ToString()
        {
            return "Front Kick";
        }

        private void Attack2EventHandler()
        {
            int currentFrame = GetCurrentAnimationFrame("FrontKick");
            Debug.Log($"Current frame: {currentFrame}");
            if (currentFrame >=1 && currentFrame <= 3)
            {
                CharacterStateIdle idleState = new CharacterStateIdle(StateController);
                StateController.ChangeState(idleState);
            }
        }
    }
}
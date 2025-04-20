using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterStateHighKicking : CharacterState
    {
        public CharacterStateHighKicking(CharacterStateController playerStateController) : base(playerStateController)
        {
        }

        public override void OnCollisionEnter(Collision collision)
        {

        }

        public override void OnEnter()
        {
            HumanBrain.Attack1Event += Attack1EventHandler;
            // Debug.Log($"Entering {this} state.");
            CharacterAnimator.speed = 1f;
            CharacterAnimator.CrossFade("HighKick", 0.0f, -1, 0.0f);
        }

        public override void OnExit()
        {
            HumanBrain.Attack2Event -= Attack1EventHandler;
            // Debug.Log($"Exiting {this} state.");
        }

        public override void UpdateState()
        {
            // Debug.Log($"Updating {this} state.");
            if (IsAnimationFinished("HighKick"))
            {
                CharacterStateIdle idleState = new CharacterStateIdle(StateController);
                StateController.ChangeState(idleState);
            }
        }

        public override string ToString()
        {
            return "High Kick";
        }

        private void Attack1EventHandler()
        {
            int currentFrame = GetCurrentAnimationFrame("HighKick");
            Debug.Log($"Current frame: {currentFrame}");
            if (currentFrame >=1 && currentFrame <= 3)
            {
                CharacterStateIdle idleState = new CharacterStateIdle(StateController);
                StateController.ChangeState(idleState);
            }
        }
    }
}
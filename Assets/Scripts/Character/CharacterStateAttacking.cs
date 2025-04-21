using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterStateAttacking : CharacterState
    {
        public CharacterStateAttacking(CharacterStateMachine playerStateController, CharacterMove characterMove) : base(playerStateController, characterMove, false)
        {
            
        }

        public override void OnCollisionEnter(Collision collision)
        {

        }

        public override void OnEnter()
        {
            PlayAnimation();
        }

        public override void OnExit()
        {
            
        }

        public override void UpdateState()
        {
            int currentFrame = GetCurrentAnimationFrame();
            Debug.Log($"Current frame:{currentFrame}");

            if (CharacterMove.TriggerDirection != CharacterMovementDirection.None)
            {
            if (currentFrame >= CharacterMove.MovementStartFrame && currentFrame <= CharacterMove.MovementEndFrame  )
                Character.Move(CharacterMove.TriggerDirection, CharacterMove.MovementSpeedMultiplier);
            }

            if (IsAnimationFinished())
            {
                CharacterStateIdle idleState = new CharacterStateIdle(StateController, Character.Attributes.Moves.Find(m => m.TriggerDirection == CharacterMovementDirection.None));
                StateController.ForceChangeState(idleState);
            }
        }

        public override string ToString()
        {
            return "Attacking";
        }
    }
}
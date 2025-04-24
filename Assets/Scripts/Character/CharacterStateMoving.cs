using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterStateMoving : CharacterState
    {
        public CharacterStateMoving(CharacterStateMachine playerStateController, CharacterMove characterMove) : base(playerStateController, characterMove, true)
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
            Character.Move(CharacterMove.TriggerDirection, CharacterMove.MovementSpeedMultiplier);
        }

        public override string ToString()
        {
            return "Moving";
        }
    }
}
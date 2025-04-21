using System;
using NUnit.Framework;
using UnityEngine;

namespace Assets.Scripts
{
    public class CharacterStateIdle : CharacterState
    {
        public CharacterStateIdle(CharacterStateMachine playerStateController, CharacterMove characterMove) : base(playerStateController, characterMove, true)
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
            // Debug.Log($"Updating {this} state.");
        }

        public override string ToString()
        {
            return "Idle";
        }
    }
}
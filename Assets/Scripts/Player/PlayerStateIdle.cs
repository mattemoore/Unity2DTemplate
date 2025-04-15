using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace Assets.Scripts
{
    public class PlayerStateIdle : PlayerState
    {
        [SerializeField]
        private AnimationClip _Animation;

        protected virtual void OnEnable()
        {
            AnimancerState state = PlayerController.Animancer.Play(_Animation);
            state.Events(this).OnEnd ??= PlayerController.StateMachine.ForceSetDefaultState;
        }
    }
}
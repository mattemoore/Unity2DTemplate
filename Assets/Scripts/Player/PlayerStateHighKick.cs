using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace Assets.Scripts
{
    public class PlayerStateHighKick : PlayerState
    {
        [SerializeField]
        private AnimationClip _Animation;

        protected virtual void OnEnable()
        {
            PlayerController.Animancer.Play(_Animation);
        }
    }
}
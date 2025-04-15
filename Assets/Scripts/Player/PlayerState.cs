using UnityEngine;
using Animancer;
using Animancer.FSM;

namespace Assets.Scripts
{
    public abstract class PlayerState : StateBehaviour
    {
        [SerializeField]
        private PlayerController _playerController;
        public PlayerController PlayerController => _playerController;

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            gameObject.GetComponentInParentOrChildren(ref _playerController);
        }
#endif
    }
}
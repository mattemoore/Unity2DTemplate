using Animancer;
using Animancer.FSM;
using EditorAttributes;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(NamedAnimancerComponent))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private AnimancerComponent _Animancer;
        public AnimancerComponent Animancer => _Animancer;

        [SerializeField]
        private StateMachine<PlayerState>.WithDefault _StateMachine;
        public StateMachine<PlayerState>.WithDefault StateMachine => _StateMachine;

        [SerializeField, Clamp(0.1f, 10.0f), Tooltip("The speed of the player.")]
        private float _playerMoveSpeed = 2.0f;
        public float PlayerMoveSpeed => _playerMoveSpeed;

        public void MoveRight()
        {
            // Debug.Log("MoveRight called on " + gameObject.name);
            transform.position += _playerMoveSpeed * Time.deltaTime * Vector3.right;
        }

        public void MoveLeft()
        {
            // Debug.Log("MoveLeft called on " + gameObject.name);
            transform.position += _playerMoveSpeed * Time.deltaTime * Vector3.left;
        }

        protected virtual void Awake()
        {
            _StateMachine.InitializeAfterDeserialize();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {

        }
    }
}

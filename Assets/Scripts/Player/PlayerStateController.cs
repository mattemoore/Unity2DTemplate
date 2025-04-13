using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(PlayerController))]
    public class PlayerStateController : MonoBehaviour
    {
        public PlayerState CurrentState { get; internal set; }
        internal PlayerController PlayerController { get; private set; }
        internal float DeltaTimeSinceLastUpdate { get; private set; }
        
        public static event Action<PlayerController, PlayerState> OnChangeState;
        
        public void ChangeState(PlayerState newState)
        {
            CurrentState.OnExit();
            newState.OnEnter();
            CurrentState = newState;
            OnChangeState?.Invoke(PlayerController, newState);
        }

        private void OnEnable()
        {
            PlayerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            CurrentState = new PlayerStateIdle(this);
            ChangeState(CurrentState);
        }

        // NOTE: This is placed in LateUpdate to ensure that all AI updates are processed first,
        //       allowing commands to be queued before this point. This prevents issues such as
        //       actions like holding down a shot from ending prematurely because the command
        //       hasn't been queued in time.
        private void LateUpdate()
        {
            DeltaTimeSinceLastUpdate = Time.deltaTime;
            CurrentState.UpdateState();
        }

        private void OnCollisionEnter(Collision collision)
        {
            CurrentState.OnCollisionEnter(collision);
        }
    }
}
using System;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(CharacterController))]
    public class CharacterStateController : MonoBehaviour
    {
        // I think that after domain reloading after a script is updated references to non-Unity objects are null,
        // Start is not called after domain reload in this case.  But playing and stoping and playing domain reloads are fine.
        private CharacterState CurrentState;

        internal CharacterController PlayerController { get; private set; }
        internal float DeltaTimeSinceLastUpdate { get; private set; }

        public static event Action<CharacterController, CharacterState> OnChangeState;

        public void ChangeState(CharacterState newState)
        {
            CurrentState.OnExit();
            newState.OnEnter();
            CurrentState = newState;
            OnChangeState?.Invoke(PlayerController, newState);
        }

        private void Awake()
        {
           
        }

        private void Start()
        {
            PlayerController = GetComponent<CharacterController>();
            CurrentState = new CharacterStateIdle(this);
            this.ChangeState(CurrentState);
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
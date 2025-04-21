using System;
using EditorAttributes;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Character))]
    public class CharacterStateMachine : MonoBehaviour
    {
        // I think that after domain reloading after a script is updated references to non-Unity objects are null,
        // Start is not called after domain reload in this case.  But playing and stoping and playing domain reloads are fine.
        private CharacterState CurrentState;
        internal Character Character { get; private set; }
        internal float DeltaTimeSinceLastUpdate { get; private set; }

        public static event Action<Character, CharacterState> OnChangeState;

        public void ChangeState(CharacterState newState)
        {
            if (CanChangeState(newState))
                SetState(newState);
        }

        public void ForceChangeState(CharacterState newState)
        {
            SetState(newState);
        }

        public void ChangeToDefaultState()
        {
            CharacterState defaultState = GetDefaultState();
            if (CanChangeState(defaultState))
            {
                SetState(defaultState);
            }
        }

        public void ForceDefaultState()
        {
            SetState(GetDefaultState());
        }

        private bool CanChangeState(CharacterState newState)
        {
            if (!CurrentState.IsInterruptible) return false;

            // Do not change state if the current move is same as incoming move
            // ASSUMPTION: A move only applies to one state
            if (newState.CharacterMove.Name == CurrentState.CharacterMove.Name) return false;
            
            return true;
        }

        private void SetState(CharacterState newState)
        {
            CurrentState.OnExit();
            newState.OnEnter();
            CurrentState = newState;
            OnChangeState?.Invoke(Character, newState);
        }

        private CharacterState GetDefaultState()
        {
            Debug.Log("Getting default state");
            return new CharacterStateIdle(this, Character.Attributes.Moves.Find(m => m.TriggerDirection == CharacterMovementDirection.None));
        }

        private void Awake()
        {

        }

        private void Start()
        {
            Character = GetComponent<Character>();
            CurrentState = GetDefaultState();
            ChangeState(CurrentState);
        }

        // NOTE: This is placed in LateUpdate to ensure that all AI updates are processed first.
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
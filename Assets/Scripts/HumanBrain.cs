/// <summary>
/// PlayerInputController.cs
/// This script processes player input every frame and emits events as needed.
/// A move direction event is emitted every frame.  Attack and pause events are emitted when the corresponding buttons are pressed.
/// </summary>

using System;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public enum InputMovementDirection
    {
        Left,
        Right,
        Up,
        Down,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight,
        None
    }

    [RequireComponent(typeof(Character))]
    public class HumanBrain : MonoBehaviour
    {
        [SerializeField, Tooltip("Attack 1 input action.")]
        private InputActionReference _action1;
        [SerializeField, Tooltip("Attack 2 input action.")]
        private InputActionReference _action2;
        [SerializeField, Tooltip("Attack 3 input action.")]
        private InputActionReference _action3;
        [SerializeField, Tooltip("Attack 4 input action.")]
        private InputActionReference _action4;
        [SerializeField, Tooltip("Move input action.")]
        private InputActionReference _moveAction;
        [SerializeField, Tooltip("Pause input action.")]
        private InputActionReference _pauseAction;

        public static event Action PauseButtonPressed;

        private bool _pollContinuousInputs = false;
        private Character _character;
        private CharacterStateMachine _characterStateMachine;
        private bool _action1WasActive;

        private void Start()
        {
            _character = GetComponent<Character>();
            _characterStateMachine = GetComponent<CharacterStateMachine>();
        }

        private void OnEnable()
        {
            GameController.PlayStopped += PlayStoppedEventHandler;
            GameController.PlayStarted += PlayStartedEventHandler;
            _pauseAction.action.performed += PauseButtonPressedEventHandler;
        }

        private void OnDisable()
        {
            GameController.PlayStopped -= PlayStoppedEventHandler;
            GameController.PlayStarted -= PlayStartedEventHandler;
            _pauseAction.action.performed -= PauseButtonPressedEventHandler;
        }

        private void Update()
        {
            if (_pollContinuousInputs)
            {
                // Get movement direction
                Vector2 moveVector = _moveAction.action.ReadValue<Vector2>();
                Vector2 quantizedMovedVector = QuantizeVector(moveVector);
                InputMovementDirection inputDirection = quantizedMovedVector != Vector2.zero
                    ? GetInputMovementDirection(quantizedMovedVector)
                    : InputMovementDirection.None;
                CharacterMovementDirection characterMoveDirection = GetCharacterMovementDirection(inputDirection);

                // Get buttons pressed
                bool action1IsActive = _action1.action.ReadValue<float>() > 0;
                CharacterAction characterAction = CharacterAction.None;
                if (action1IsActive && !_action1WasActive)
                {
                    characterAction = CharacterAction.Action1;
                    Debug.Log("Action1 invoked...");
                }
                _action1WasActive = action1IsActive;

                // Find a move that matches the combination of input direction and button pressed
                CharacterMove move = _character.Attributes.Moves.Find(move => move.TriggerDirection == characterMoveDirection && move.TriggerAction == characterAction);
                if (!move.Equals(default(CharacterMove)))
                {
                    // Convert move found to state and update state if appropriate
                    if (move.State == CharacterMoveState.Attack)
                    {
                        _characterStateMachine.ChangeState(new CharacterStateAttacking(_characterStateMachine, move));
                    }
                    else if (move.State == CharacterMoveState.Movement)
                    {
                        _characterStateMachine.ChangeState(new CharacterStateMoving(_characterStateMachine, move));
                    }
                    else // idle
                    {
                        _characterStateMachine.ChangeToDefaultState();
                    }
                }
            }
        }

        private void PauseButtonPressedEventHandler(InputAction.CallbackContext context)
        {
            PauseButtonPressed?.Invoke();
        }

        // GameController events
        private void PlayStartedEventHandler()
        {
            _pollContinuousInputs = true;
        }

        private void PlayStoppedEventHandler()
        {
            _pollContinuousInputs = false;
        }

        private static Vector2 QuantizeVector(Vector2 vectorToQuantize)
        {
            return new Vector2((int)Mathf.Round(vectorToQuantize.x), (int)Mathf.Round(vectorToQuantize.y));
        }

        private static InputMovementDirection GetInputMovementDirection(Vector2 quantizedVector)
        {
            float quantized_move_x = quantizedVector.x;
            float quantized_move_y = quantizedVector.y;
            if ((quantized_move_x != 0 && quantized_move_x != 1 && quantized_move_x != -1) || (quantized_move_y != 0 && quantized_move_y != 1 && quantized_move_y != -1))
            {
                throw new ArgumentOutOfRangeException("Quantized move vector values must be 0, 1 or -1.");
            }

            if (quantized_move_x == 1 && quantized_move_y == 1) return InputMovementDirection.UpRight;
            if (quantized_move_x == 1 && quantized_move_y == -1) return InputMovementDirection.DownRight;
            if (quantized_move_x == -1 && quantized_move_y == 1) return InputMovementDirection.UpLeft;
            if (quantized_move_x == -1 && quantized_move_y == -1) return InputMovementDirection.DownLeft;
            if (quantized_move_x == 1) return InputMovementDirection.Right;
            if (quantized_move_x == -1) return InputMovementDirection.Left;
            if (quantized_move_y == 1) return InputMovementDirection.Up;
            return InputMovementDirection.Down;
        }

        private CharacterMovementDirection GetCharacterMovementDirection(InputMovementDirection inputDirection)
        {
            CharacterMovementDirection characterMoveDirection;
            if (inputDirection == InputMovementDirection.Up)
            {
                characterMoveDirection = CharacterMovementDirection.Jump;
            }
            else if (inputDirection == InputMovementDirection.Down)
            {
                characterMoveDirection = CharacterMovementDirection.Duck;
            }
            else if (inputDirection == InputMovementDirection.Left || inputDirection == InputMovementDirection.Right)
            {
                CharacterFacingDirection facingDirection = _character.FacingDirection;
                if (facingDirection == CharacterFacingDirection.Right)
                {
                    characterMoveDirection = inputDirection == InputMovementDirection.Left ? CharacterMovementDirection.Backward : CharacterMovementDirection.Forward;
                }
                else
                {
                    characterMoveDirection = inputDirection == InputMovementDirection.Right ? CharacterMovementDirection.Backward : CharacterMovementDirection.Forward;
                }
            }
            else
            {
                characterMoveDirection = CharacterMovementDirection.None;
            }

            return characterMoveDirection;
        }
    }
}

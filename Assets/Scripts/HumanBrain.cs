/// <summary>
/// PlayerInputController.cs
/// This script processes player input every frame and emits events as needed.
/// A move direction event is emitted every frame.  Attack and pause events are emitted when the corresponding buttons are pressed.
/// </summary>

using System;
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

    [RequireComponent(typeof(CharacterController))]
    public class HumanBrain : MonoBehaviour
    {
        [SerializeField, Tooltip("Attack 1 input action.")]
        private InputActionReference _attack1Action;
        [SerializeField, Tooltip("Attack 2 input action.")]
        private InputActionReference _attack2Action;
        [SerializeField, Tooltip("Attack 3 input action.")]
        private InputActionReference _attack3Action;
        [SerializeField, Tooltip("Attack 4 input action.")]
        private InputActionReference _attack4Action;
        [SerializeField, Tooltip("Move input action.")]
        private InputActionReference _moveAction;
        [SerializeField, Tooltip("Pause input action.")]
        private InputActionReference _pauseAction;

        public static event Action PauseButtonPressed;
        public static event Action<CharacterMovementDirection> MoveEvent;
        // TODO: Make this one event with a parameter for the attack type, create an attack type enum in CharacterController like CharacterMovementDirection
        public static event Action Attack1Event;
        public static event Action Attack2Event;
        public static event Action Attack3Event;
        public static event Action Attack4Event;

        private bool _pollContinuousInputs = false;
        private CharacterController _characterContoller;

        private void Start()
        {
            _characterContoller = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            GameController.PlayStopped += PlayStoppedEventHandler;
            GameController.PlayStarted += PlayStartedEventHandler;
            _attack1Action.action.performed += AttackButton1PressedEventHandler;
            _attack2Action.action.performed += AttackButton2PressedEventHandler;
            _attack3Action.action.performed += AttackButton3PressedEventHandler;
            _attack4Action.action.performed += AttackButton4PressedEventHandler;
            _pauseAction.action.performed += PauseButtonPressedEventHandler;
        }

        private void OnDisable()
        {
            GameController.PlayStopped -= PlayStoppedEventHandler;
            GameController.PlayStarted -= PlayStartedEventHandler;
            _attack1Action.action.performed -= AttackButton1PressedEventHandler;
            _attack2Action.action.performed -= AttackButton2PressedEventHandler;
            _attack3Action.action.performed -= AttackButton3PressedEventHandler;
            _attack4Action.action.performed -= AttackButton4PressedEventHandler;
            _pauseAction.action.performed -= PauseButtonPressedEventHandler;
        }

        private void Update()
        {
            if (_pollContinuousInputs)
            {
                Vector2 moveVector = _moveAction.action.ReadValue<Vector2>();
                Vector2 quantizedMovedVector = QuantizeVector(moveVector);
                InputMovementDirection inputDirection = quantizedMovedVector != Vector2.zero
                    ? GetInputMovemnetDirection(quantizedMovedVector)
                    : InputMovementDirection.None;
                CharacterMovementDirection characterMoveDirection = GetCharacterMovementDirection(inputDirection);
                MoveEvent?.Invoke(characterMoveDirection);
            }
        }

        // InputAction events emit PlayerInputController events
        private void AttackButton1PressedEventHandler(InputAction.CallbackContext context)
        {
            Attack1Event?.Invoke();
        }

        private void AttackButton2PressedEventHandler(InputAction.CallbackContext context)
        {
            Attack2Event?.Invoke();
        }

        private void AttackButton3PressedEventHandler(InputAction.CallbackContext context)
        {
            Attack3Event?.Invoke();
        }

        private void AttackButton4PressedEventHandler(InputAction.CallbackContext context)
        {
            Attack4Event?.Invoke();
        }

        private void PauseButtonPressedEventHandler(InputAction.CallbackContext context)
        {
            PauseButtonPressed?.Invoke();
        }

        // GameController events
        private void PlayStartedEventHandler()
        {
            _pauseAction.action.Enable();
            _moveAction.action.Enable();
            _attack1Action.action.Enable();
            _attack2Action.action.Enable();
            _attack3Action.action.Enable();
            _attack4Action.action.Enable();
            _pollContinuousInputs = true;
        }

        private void PlayStoppedEventHandler()
        {
            _pauseAction.action.Disable();
            _moveAction.action.Disable();
            _attack1Action.action.Disable();
            _attack2Action.action.Disable();
            _attack3Action.action.Disable();
            _attack4Action.action.Disable();
            _pollContinuousInputs = false;
        }

        private static Vector2 QuantizeVector(Vector2 vectorToQuantize)
        {
            return new Vector2((int)Mathf.Round(vectorToQuantize.x), (int)Mathf.Round(vectorToQuantize.y));
        }

        private static InputMovementDirection GetInputMovemnetDirection(Vector2 quantizedVector)
        {
            float quantized_move_x = quantizedVector.x;
            float quantized_move_y = quantizedVector.y;
            if ((quantized_move_x != 0 && quantized_move_x != 1 && quantized_move_x != -1) || (quantized_move_y != 0 && quantized_move_y != 1 && quantized_move_y != -1))
            {
                throw new System.ArgumentOutOfRangeException("Quantized move vector values must be 0, 1 or -1.");
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
                CharacterFacingDirection facingDirection = _characterContoller.FacingDirection;
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

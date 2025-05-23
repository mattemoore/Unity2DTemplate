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
        private CharacterMove _idleMove;

        private void Start()
        {
            _character = GetComponent<Character>();
            _characterStateMachine = GetComponent<CharacterStateMachine>();
            _idleMove = _character.Attributes.Moves.Find(m => m.TriggerDirection == CharacterMovementDirection.None);
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
                CharacterMovementDirection characterMoveDirection = ConvertInputToMovement(moveVector);

                // Get buttons pressed
                // TODO: Refactor this into a separate method to reduce complexity, build up array of CharacterActions
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
                if (move != null)
                {
                    CharacterStateMachine.SendMoveToStateMachine(move, _characterStateMachine);
                }
                // TODO: Store the movement direction and button combo into a history for future replay feature
            }
            else
            {
                CharacterStateMachine.SendMoveToStateMachine(_idleMove, _characterStateMachine);
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

        private CharacterMovementDirection ConvertInputToMovement(Vector3 moveVector)
        {
            Vector2 quantizedMovedVector = QuantizeVector(moveVector);
            InputMovementDirection inputDirection = quantizedMovedVector != Vector2.zero
                ? GetInputMovementDirection(quantizedMovedVector)
                : InputMovementDirection.None;
            CharacterMovementDirection characterMoveDirection = GetCharacterMovementDirection(inputDirection);
            return characterMoveDirection;
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
                bool movingInFacingDirection = inputDirection == InputMovementDirection.Right == _character.IsFacingRight;
                characterMoveDirection = movingInFacingDirection ? CharacterMovementDirection.Forward : CharacterMovementDirection.Backward;
            }
            else
            {
                characterMoveDirection = CharacterMovementDirection.None;
            }
            return characterMoveDirection;
        }
    }
}

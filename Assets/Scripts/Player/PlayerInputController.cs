/// <summary>
/// PlayerInputController.cs
/// This script processes player input every frame and emits events as needed.
/// A move direction event is emitted every frame.  If the player is pressing an input button, a move direction of None is emitted.
/// </summary>

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts
{
    public enum InputMoveDirection
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

    public readonly struct PlayerInput
    {
        public readonly InputMoveDirection MoveDirection;
        public readonly bool Attack;
        public readonly float FrameNumber;

        public PlayerInput(InputMoveDirection moveDirection, bool attack, float frameNumber)
        {
            MoveDirection = moveDirection;
            Attack = attack;
            FrameNumber = frameNumber;
        }
    }

    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField, Tooltip("Attack input action.")]
        private InputActionReference _attackAction;
        [SerializeField, Tooltip("Move input action.")]
        private InputActionReference _moveAction;
        [SerializeField, Tooltip("Pause input action.")]
        private InputActionReference _pauseAction;

        public static event Action PauseButtonPressed;
        public static event Action<InputMoveDirection> MoveButtonPressed;
        public static event Action AttackButtonPressed;

        public List<PlayerInput> InputHistory { get; private set; } = new List<PlayerInput>();

        private bool _playIsStopped;

        private void Start()
        {
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
            if (_playIsStopped) return;

            Vector2 moveVector = _moveAction.action.ReadValue<Vector2>();
            Vector2 quantizedMovedVector = QuantizeVector(moveVector);
            // Debug.Log($"Move vector: {moveVector}. Quantized move vector: {quantizedMovedVector}.");
            var moveDirection = quantizedMovedVector != Vector2.zero 
                ? GetMoveDirection(quantizedMovedVector) 
                : InputMoveDirection.None;
            MoveButtonPressed?.Invoke(moveDirection);

            bool attackButtonPressed = _attackAction.action.ReadValue<float>() > 0;
            if (attackButtonPressed)
                AttackButtonPressed?.Invoke();

            InputHistory.Add(new PlayerInput(
                moveDirection,
                attackButtonPressed,
                Time.frameCount
            ));
        }

        private static Vector2 QuantizeVector(Vector2 vectorToQuantize)
        {
            return new Vector2((int)Mathf.Round(vectorToQuantize.x), (int)Mathf.Round(vectorToQuantize.y));
        }

        private static InputMoveDirection GetMoveDirection(Vector2 quantizedVector)
        {
            float quantized_move_x = quantizedVector.x;
            float quantized_move_y = quantizedVector.y;
            if ((quantized_move_x != 0 && quantized_move_x != 1 && quantized_move_x != -1) || (quantized_move_y != 0 && quantized_move_y != 1 && quantized_move_y != -1))
            {
                throw new System.ArgumentOutOfRangeException("Quantized move vector values must be 0, 1 or -1.");
            }

            if (quantized_move_x == 1 && quantized_move_y == 1) return InputMoveDirection.UpRight;
            if (quantized_move_x == 1 && quantized_move_y == -1) return InputMoveDirection.DownRight;
            if (quantized_move_x == -1 && quantized_move_y == 1) return InputMoveDirection.UpLeft;
            if (quantized_move_x == -1 && quantized_move_y == -1) return InputMoveDirection.DownLeft;
            if (quantized_move_x == 1) return InputMoveDirection.Right;
            if (quantized_move_x == -1) return InputMoveDirection.Left;
            if (quantized_move_y == 1) return InputMoveDirection.Up;
            return InputMoveDirection.Down;
        }

        private void PlayStartedEventHandler()
        {
            _pauseAction.action.Enable();
            _moveAction.action.Enable();
            _attackAction.action.Enable();
            _playIsStopped = true;
        }

        private void PlayStoppedEventHandler()
        {
            _pauseAction.action.Disable();
            _moveAction.action.Disable();
            _attackAction.action.Disable();
            _playIsStopped = false;
        }

        private void PauseButtonPressedEventHandler(InputAction.CallbackContext context)
        {
            PauseButtonPressed?.Invoke();
            Debug.Log("Pause button pressed.");
        }
    }
}

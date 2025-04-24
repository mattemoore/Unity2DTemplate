/// <summary>
/// PlayerInputController.cs
/// This script processes player input every frame and emits events as needed.
/// A move direction event is emitted every frame.  Attack and pause events are emitted when the corresponding buttons are pressed.
/// </summary>

using System;
using TMPro.EditorUtilities;
using UnityEngine;

namespace Assets.Scripts
{

    [RequireComponent(typeof(Character))]
    public class AIBrain : Brain
    {
        private Character _character;
        private CharacterStateMachine _characterStateMachine;
        private bool _isProcessing;
        private GameController _gameController;

        private void Start()
        {
            _character = GetComponent<Character>();
            _characterStateMachine = GetComponent<CharacterStateMachine>();
            _gameController = GameObject.Find("Root").GetComponent<GameController>();
        }

        private void OnEnable()
        {
            GameController.PlayStopped += PlayStoppedEventHandler;
            GameController.PlayStarted += PlayStartedEventHandler;
        }

        private void OnDisable()
        {
            GameController.PlayStopped -= PlayStoppedEventHandler;
            GameController.PlayStarted -= PlayStartedEventHandler;
        }

        private void LateUpdate()
        {
            if (_isProcessing)
            {
                // Update sensor data
                float distanceBetweenCharacters = _gameController.DistanceBetweenCharacters;
                Debug.Log($"Distance between characters: {distanceBetweenCharacters}");
                
                // Front kick if player is within 3 units of distance
                CharacterMove move = default;
                if (distanceBetweenCharacters < 3.0f)
                {
                    move = _character.Attributes.Moves.Find(m => m.Name == "Front Kick");
                }

                if (!move.Equals(default(CharacterMove)))
                {
                    SendMoveToStateMachine(move);
                }

                // Pick a move based on sensors
                // TODO NEXT: Refactor the code below that converts move to state into a method
                //            then take commom stuff and put it into a Brain Base class

                // Find the move
                // CharacterMove move = _character.Attributes.Moves.Find(move => move.TriggerDirection == characterMoveDirection && move.TriggerAction == characterAction);
                // if (!move.Equals(default(CharacterMove)))
                // {
                //     // Convert move found to state and update state if appropriate
                //     if (move.State == CharacterMoveState.Attack)
                //     {
                //         _characterStateMachine.ChangeState(new CharacterStateAttacking(_characterStateMachine, move));
                //     }
                //     else if (move.State == CharacterMoveState.Movement)
                //     {
                //         _characterStateMachine.ChangeState(new CharacterStateMoving(_characterStateMachine, move));
                //     }
                //     else // idle
                //     {
                //         _characterStateMachine.ChangeToDefaultState();
                //     }
                // }

                // TODO: Store the movement direction and button combo into a history for future replay feature
            }
        }

        // GameController events
        private void PlayStartedEventHandler()
        {
            _isProcessing = true;
        }

        private void PlayStoppedEventHandler()
        {
            _isProcessing = false;
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

        private void SendMoveToStateMachine(CharacterMove move)
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

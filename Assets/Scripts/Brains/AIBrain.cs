/// <summary>
/// PlayerInputController.cs
/// This script processes player input every frame and emits events as needed.
/// A move direction event is emitted every frame.  Attack and pause events are emitted when the corresponding buttons are pressed.
/// </summary>

using System;
using UnityEngine;

namespace Assets.Scripts
{

    [RequireComponent(typeof(Character))]
    public class AIBrain : MonoBehaviour
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
                    CharacterStateMachine.SendMoveToStateMachine(move, _characterStateMachine);
                }

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
    }
}

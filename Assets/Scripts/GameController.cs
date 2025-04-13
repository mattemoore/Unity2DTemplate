using UnityEngine;
using Assets.Scripts;
using System;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;
    [SerializeField, Tooltip("Attack input action.")]

    public static event Action PlayStarted;
    public static event Action PlayStopped;

    private bool _isPaused = false;

    private void OnEnable()
    {
        PlayerInputController.PauseButtonPressed += PauseButtonPressedEventHandler;
    }

    private void OnDisable()
    {
        PlayerInputController.PauseButtonPressed -= PauseButtonPressedEventHandler;
    }

    private void PauseButtonPressedEventHandler()
    {
       _isPaused = !_isPaused;
       if (_isPaused)
       {
           PlayStarted?.Invoke();
           Time.timeScale = 0f; // Pause the game
       }
       else
       {
           PlayStopped?.Invoke();
           Time.timeScale = 1f; // Unpause the game
       }
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
}

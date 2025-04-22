using UnityEngine;
using Assets.Scripts;
using System;

public class GameController : MonoBehaviour
{
    public Character Player1;
    public Character Player2;
   
    public static event Action PlayStarted;
    public static event Action PlayStopped;

    private bool _isPaused = false;

    private void OnEnable()
    {
        HumanBrain.PauseButtonPressed += PauseButtonPressedEventHandler;
    }

    private void OnDisable()
    {
        HumanBrain.PauseButtonPressed -= PauseButtonPressedEventHandler;
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
        PlayStarted?.Invoke();
    }

    private void Update()
    {
        
    }
}

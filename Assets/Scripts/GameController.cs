using UnityEngine;
using Assets.Scripts;
using System;

public class GameController : MonoBehaviour
{
    public Character Player1;
    public Character Player2;
    
    public float MinDistanceBetweenCharacters = 0.5f;

    public float DistanceBetweenCharacters { get; private set; }

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
        // TODO: Add netcode for gameobjects, use rigibodynetworking one for collision detection
        // to make it such that players can't walk through each other
        UpdateSensors();
    }

    private void LateUpdate()
    {

    }

    private void UpdateSensors()
    {
        DistanceBetweenCharacters = Vector3.Distance(Player1.transform.position, Player2.transform.position);
    }
}

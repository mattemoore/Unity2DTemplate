using UnityEngine;
using Assets.Scripts;
using System;
using System.Collections;

public class GameController : MonoBehaviour
{
    public Character Player1;
    public Character Player2;
    public Vector3 Player1StartPosition = new Vector3(-2f, -1.5f, 0);
    public Vector3 Player2StartPosition = new Vector3(2f, -1.5f, 0);
    public float MinDistanceBetweenCharacters = 0.5f;
    public float RoundLengthInSeconds = 10f;
    public int CurrentRound = 1;
    public int NumberOfRounds = 3;
    public float TimeBetweenRounds = 3;

    public float DistanceBetweenCharacters { get; private set; }

    public static event Action PlayStarted;
    public static event Action PlayStopped;

    private bool _isPlayStopped, _isPaused = false;

    public void StartMatch()
    {
        Debug.Log("Match starting!");
        StartRound(1);
    }

    private void EndMatch()
    {
        Debug.Log("Match ended!");
        StartMatch();
    }

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
            ResumePlay();
        }
        else
        {
            PausePlay();
        }
    }

    private void Update()
    {
        if (!_isPlayStopped)
        {
            UpdateSensors();
        }
    }

    private void StartPlay()
    {
        PlayStarted?.Invoke();
        _isPlayStopped = false;
    }

    private void StopPlay()
    {
        PlayStopped?.Invoke();
        _isPlayStopped = true;
    }

    private void PausePlay()
    {
        StopPlay();
        Time.timeScale = 0f;
    }

    private void ResumePlay()
    {
        StartPlay();
        Time.timeScale = 1.0f;
    }

    private void Start()
    {
        StartMatch();
    }

    private void StartRound(int roundNumber)
    {
        CurrentRound = roundNumber;

        // Reset player positions and states
        Player1.transform.position = Player1StartPosition;
        Player2.transform.position = Player2StartPosition;
        UpdateSensors();
        Debug.Log($"Round {roundNumber} started!");

        // Start the round timer
        StartPlay();
        StartCoroutine(RoundCountdownCoroutine(RoundLengthInSeconds, roundNumber));
    }

    private IEnumerator RoundCountdownCoroutine(float roundLength, int roundNumber)
    {
        float timer = roundLength;
        while (timer > 0f)
        {
            // Optionally, update UI with timer here
            yield return null;
            timer -= Time.deltaTime;
            Debug.Log($"Time left:{timer} in round number: {roundNumber}");
        }
        EndRound(roundNumber);
    }

    private void EndRound(int roundNumber)
    {
        Debug.Log($"{roundNumber} round ended!");
        StopPlay();
        StartCoroutine(PostRoundWait(TimeBetweenRounds, roundNumber));
    }

    private IEnumerator PostRoundWait(float waitSeconds, int roundNumber)
    {
        float timer = waitSeconds;
        while (timer > 0f)
        {
            yield return null;
            timer -= Time.unscaledDeltaTime;
            Debug.Log($"Time left between rounds for round {roundNumber}: {timer}");
        }

        if (roundNumber <= NumberOfRounds - 1)
        {
            roundNumber += 1;
            StartRound(roundNumber);
        }
        else
        {
            EndMatch();
        }
    }

    private void UpdateSensors()
    {
        DistanceBetweenCharacters = Vector3.Distance(Player1.transform.position, Player2.transform.position);
    }
}

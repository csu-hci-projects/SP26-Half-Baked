using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;

public class ExperimentController : MonoBehaviour
{
    public BallSpawner spawner;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI pointText;
    public UserTimer timer;
    public float roundLength;

    private int currentRound;
    private int roundPoints;
    private float roundTimeLeft;

    private bool roundInProgress;

    public void Start()
    {
        
    }

    public void Update()
    {
        roundText.text = "Round: " + currentRound;
        pointText.text = "Points: " + roundPoints;

        if (roundInProgress)
        {
            roundTimeLeft -= Time.deltaTime;
            timer.Sync(roundTimeLeft, roundLength);
            if (roundTimeLeft <= 0)
            {
                EndRound();
            }
        }
    }

    public void StartRound()
    {
        spawner.SpawnRound();
        currentRound++;
        roundTimeLeft = roundLength;
        roundInProgress = true;
    }

    public void EndRound()
    {
        roundTimeLeft = 0;
        roundInProgress = false;
        spawner.ClearBalls();
    }

    public void AddPoint()
    {
        roundPoints++;
    }
}

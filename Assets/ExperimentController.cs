using System.Runtime.Serialization.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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

    public Volume volume;
    public float focusDistanceNearsighted = 1.0f;
    public float focusDistanceFarsighted = 3.5f;

    private DepthOfField dof;
    private int condition = 0;
    public int enabledFocalLength = 32;
    public int disabledFocalLength = 1;

    public void Start()
    {
        volume.profile.TryGet<DepthOfField>(out dof);
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
        // clean up
        spawner.ClearBalls();
        roundPoints = 0;

        spawner.SpawnRound();
        currentRound++;
        roundTimeLeft = roundLength;
        roundInProgress = true;
    }

    private void SetCondition(int c)
    {
        condition = c;
        if (condition == 0)
        {
            dof.focalLength.value = disabledFocalLength;
        }
        else if (condition == 1)
        {
            dof.focalLength.value = enabledFocalLength;
            dof.focusDistance.value = focusDistanceNearsighted;
        }
        else if (condition == 2)
        {
            dof.focalLength.value = enabledFocalLength;
            dof.focusDistance.value = focusDistanceFarsighted;
        }
    }

    public void NextCondition()
    {
        SetCondition((condition + 1) % 3);
    }

    public void EndRound()
    {
        roundTimeLeft = 0;
        roundInProgress = false;
        spawner.ClearBalls();
    }

    public void AddPoint()
    {
        if (roundTimeLeft > 0)
        {
            roundPoints++;
        }
    }
}

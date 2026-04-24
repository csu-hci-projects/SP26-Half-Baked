using System;
using System.Collections.Generic;
using System.IO;
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

    private float lastCollectionTime = 0;
    private List<string> lines;

    public void Start()
    {
        volume.profile.TryGet<DepthOfField>(out dof);
        lines = new List<string>();
        lines.Add("Round,Condition,Interim,Bucket");
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

    public void ResetExperiment()
    {
        // clean up
        spawner.ClearBalls();
        roundPoints = 0;

        currentRound = 0;
        SetCondition(0);

        lines = new List<string>();
        lines.Add("Round,Condition,Interim,Bucket");
    }

    public void NextRound()
    {
        // clean up
        spawner.ClearBalls();
        roundPoints = 0;

        spawner.SpawnRound();
        currentRound++;
        roundTimeLeft = roundLength;
        roundInProgress = true;
        lastCollectionTime = Time.time;
    }

    public void SetCondition(int c)
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

        if(currentRound == 3)
        {
            ExportData();
        }
    }

    public void AddPoint()
    {
        if (roundTimeLeft > 0)
        {
            roundPoints++;
            if(spawner.GetBallsLeft() <= 5)
            {
                spawner.SpawnRound();
            }
        }
    }

    public void DropEnded()
    {
        int bucket = spawner.GetBallsInThisCollection();
        float interim = Time.time - lastCollectionTime;
        lastCollectionTime = Time.time;
        lines.Add(currentRound + "," + condition + "," + interim + "," + bucket);
    }

    public void ExportData()
    {
        DateTime dt = DateTime.Now;
        using (StreamWriter outputFile = new StreamWriter(Application.persistentDataPath + "/" + dt.ToString("yyyy-MM-dd\\THH:mm:ss\\Z") + ".csv"))
        {
            foreach (string line in lines)
            {
                outputFile.WriteLine(line);
            }
        }
    }
}

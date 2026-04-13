using System.Collections;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public ExperimentController controller;

    public int roundSize = 15;
    public GameObject ballPrefab;

    private int currentBallCount = 0;

    private void SpawnNewBall()
    {
        GameObject newBall = Instantiate(ballPrefab);
        newBall.transform.parent = transform;
        newBall.transform.localPosition = Vector3.zero + new Vector3(Random.Range(-0.1f, 0.1f), 0, Random.Range(-0.1f, 0.1f));
        newBall.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        newBall.GetComponent<PrizeBall>().spawner = this;
        currentBallCount++;
    }

    public void SpawnRound()
    {
        if (ballPrefab == null) return;
        StartCoroutine(SpawnQuantity(roundSize));
    }

    private IEnumerator SpawnQuantity(int quantity)
    {
        for (int i = 0; i < roundSize; i++)
        {
            SpawnNewBall();
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public void ClearBalls()
    {
        StartCoroutine(RemoveBalls());
    }

    private IEnumerator RemoveBalls()
    {
        while (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
            currentBallCount--;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public void OnBallCollected()
    {
        currentBallCount--;
        controller.AddPoint();
    }
}

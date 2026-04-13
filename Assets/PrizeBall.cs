using UnityEngine;

public class PrizeBall : MonoBehaviour
{
    public BallSpawner spawner;

    public void Collect()
    {
        spawner.OnBallCollected();
        Destroy(gameObject);
    }
}

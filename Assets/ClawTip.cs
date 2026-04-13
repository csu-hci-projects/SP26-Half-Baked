using UnityEngine;

public class ClawTip : MonoBehaviour
{

    void OnTriggerEnter(UnityEngine.Collider other)
    {
        if (other.gameObject.tag == "Collectable")
        {
            GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            other.gameObject.GetComponent<PrizeBall>().Collect();
        }
    }

}

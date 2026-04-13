using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserTimer : MonoBehaviour
{
    public Image fillImage;
    public TextMeshProUGUI secondsText;

    public void Sync(float timeLeft, float totalTime)
    {
        fillImage.fillAmount = (timeLeft / totalTime);
        secondsText.text = $"{(int)(timeLeft / 60.0f):N0}:{Mathf.Round(timeLeft % 60),02:N0}";
    }
}

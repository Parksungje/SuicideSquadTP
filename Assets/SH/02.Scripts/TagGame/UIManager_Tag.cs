using TMPro;
using UnityEngine;

public class UIManager_Tag : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resultText;

    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes}:{seconds:00}";
    }

    public void ResultText(int playerNumber)
    {
        resultText.text = $"{playerNumber}P ½Â¸®!";
        resultText.gameObject.SetActive(true);
    }
}
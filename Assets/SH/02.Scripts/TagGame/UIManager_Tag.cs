using TMPro;
using UnityEngine;

public class UIManager_Tag : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI alertText;

    public void UpdateTimer(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        timerText.text = $"{minutes}:{seconds:00}";
    }

    public void ResultText(int playerNumber)
    {
        resultText.text = $"{playerNumber}P 승리!";
        resultText.gameObject.SetActive(true);
    }
    
    public void AlertText(int playerNumber)
    {
        alertText.text = $"{playerNumber}P님이 폭탄을 소지 중입니다!";
    }
}
using DG.Tweening;
using Tild.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultApply : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winner;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private CanvasGroup nextButton;

    private void Start()
    {
        winner.text = MinigameManager.instance.GetWinner() ? "Player1" : "Player2";

        if (MinigameManager.instance.GetWinner())
        {
            RenderSettings.fogColor = Color.red;
            score.text = $"With {MinigameManager.instance._1PScore}:{MinigameManager.instance._2PScore}";
        }else
        {
            RenderSettings.fogColor = Color.blue;
            score.text = $"With {MinigameManager.instance._2PScore}:{MinigameManager.instance._1PScore}";
        }

        nextButton.DOFade(1, .5f).OnComplete(() => nextButton.blocksRaycasts = true).SetDelay(3);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Start_Scene");
    }
}
using DG.Tweening;
using TMPro;
using UnityEngine;

public class UIHandler_Russian : MonoBehaviour
{
    [SerializeField] private GameManager_Russian gameManager;
    [SerializeField] private TextMeshProUGUI chance;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Guids")]
    [SerializeField] private CanvasGroup guidP1;
    [SerializeField] private CanvasGroup guidP2;

    private void OnEnable()
    {
        if (gameManager == null) return;
        gameManager.TurnChanged += OnTurnChanged;
        gameManager.ScoreChanged += OnScoreChanged;
        gameManager.RoundStarted += OnRoundStarted;
        gameManager.RoundEnded += OnRoundEnded;
        gameManager.ShowChance += ShowChance;
        gameManager.GuidUI += ShowGuid;
        gameManager.HideGuid += HideGuid;
        ApplyInitial();
    }

    private void OnDisable()
    {
        if (gameManager == null) return;
        gameManager.TurnChanged -= OnTurnChanged;
        gameManager.ScoreChanged -= OnScoreChanged;
        gameManager.RoundStarted -= OnRoundStarted;
        gameManager.RoundEnded -= OnRoundEnded;
        gameManager.ShowChance -= ShowChance;
        gameManager.GuidUI -= ShowGuid;
        gameManager.HideGuid -= HideGuid;
    }

    private void ApplyInitial()
    {
        OnTurnChanged(gameManager.IsP1Turn);
        OnScoreChanged(gameManager.ScoreP1, gameManager.ScoreP2);
    }

    private void OnTurnChanged(bool isP1Turn)
    {
        if (turnText != null) turnText.text = isP1Turn ? "P1의 차례입니다." : "P2의 차례입니다.";
    }

    private void OnScoreChanged(int p1, int p2)
    {
        if (scoreText != null) scoreText.text = $"P1 | {p1} : {p2} | P2";
    }

    private void OnRoundStarted(int round)
    {
        OnTurnChanged(gameManager.IsP1Turn);
    }

    private void OnRoundEnded()
    {
        turnText.text = "-";
    }

    private void ShowChance(int bulletCount)
    {
        chance.text = $"<{bulletCount} / 6>";
        chance.DOFade(1, 0);
        chance.DOFade(0, 2);
    }

    private void ShowGuid(int num)
    {
        if (num == 1)
        {
            guidP1.DOFade(1, 0.5f);
        }
        else
        {
            guidP2.DOFade(1, 0.5f);
        }
    }

    private void HideGuid()
    {
        guidP1.DOFade(0, .1f);
        guidP2.DOFade(0, .1f);
    }
}
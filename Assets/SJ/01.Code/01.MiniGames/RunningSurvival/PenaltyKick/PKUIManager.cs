using UnityEngine;
using TMPro;
using System;
using DG.Tweening;

public class PKUIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup _goalUI;
    [SerializeField] private Transform _goalText;
    [SerializeField] private CanvasGroup _saveUI;
    [SerializeField] private Transform _saveText;
    [SerializeField] private GameObject _p1WinUI;
    [SerializeField] private GameObject _p2WinUI;

    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _p1Score;
    private int _p2Score;
    private const int MaxScore = 5;

    public event Action<bool> OnGameEnd;
    public bool IsGameEnded { get; private set; } = false;

    public void ShowGoalUI()
    {
        if (IsGameEnded) return;
        _goalText.localScale = Vector3.zero;

        _goalUI.DOFade(1, .5f).OnComplete(() =>
        {
            _goalText.localScale = Vector3.one * 70f;

            Sequence seq = DOTween.Sequence();
            seq.Join(_goalText.DOScale(10f, .25f).SetEase(Ease.OutExpo));
            seq.Join(_goalText.DORotate(new Vector3(0f, 0f, 1080f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic));
        });
        _saveUI.alpha = 0;
        HideConfirmationUI();
    }

    public void ShowSaveUI()
    {
        if (IsGameEnded) return;
        _saveText.localScale = Vector3.zero;

        _saveUI.DOFade(1, .5f).OnComplete(() =>
        {
            _saveText.localScale = Vector3.one * 70f;

            Sequence seq = DOTween.Sequence();
            seq.Join(_saveText.DOScale(10f, .25f).SetEase(Ease.OutExpo));
            seq.Join(_saveText.DORotate(new Vector3(0f, 0f, 1080f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic));
        });
        _goalUI.alpha = 0;
        HideConfirmationUI();
    }

    public void HideResultUI()
    {
        _saveUI.DOFade(0, 1f);
        _goalUI.DOFade(0, 1f);
    }

    public void ShowConfirmationUI()
    {
        if (IsGameEnded) return;
        HideResultUI();
        _p1WinUI.SetActive(false);
        _p2WinUI.SetActive(false);
    }

    public void HideConfirmationUI() { }

    public void AddScore(bool shooterWin)
    {
        if (IsGameEnded) return;

        if (shooterWin)
            _p1Score++;
        else
            _p2Score++;

        UpdateScoreUI();
        CheckWinner();
    }

    private void UpdateScoreUI()
    {
        _scoreText.text = $"{_p1Score}:{_p2Score}";
    }

    private void CheckWinner()
    {
        if (_p1Score >= MaxScore)
            EndGame(true);
        else if (_p2Score >= MaxScore)
            EndGame(false);
    }

    private void EndGame(bool p1Win)
    {
        IsGameEnded = true;
        ShowWinUI(p1Win);
        OnGameEnd?.Invoke(p1Win);
    }

    public void ShowWinUI(bool p1Win)
    {
        HideResultUI();
        HideConfirmationUI();
        _p1WinUI.SetActive(p1Win);
        _p2WinUI.SetActive(!p1Win);
    }
}

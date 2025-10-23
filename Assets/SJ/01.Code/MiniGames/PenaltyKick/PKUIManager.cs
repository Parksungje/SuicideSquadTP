C#

using UnityEngine;
using TMPro;
using Tild.Menu;

public class PKUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _goalUI;
    [SerializeField] private GameObject _saveUI;
    [SerializeField] private GameObject _p1WinUI;
    [SerializeField] private GameObject _p2WinUI;

    [SerializeField] private TextMeshProUGUI _p1ScoreText;
    [SerializeField] private TextMeshProUGUI _p2ScoreText;

    private int _p1Score;
    private int _p2Score;
    private const int MaxScore = 5;

    public void ShowGoalUI()
    {
        _goalUI.SetActive(true);
        _saveUI.SetActive(false);
    }

    public void ShowSaveUI()
    {
        _saveUI.SetActive(true);
        _goalUI.SetActive(false);
    }

    public void HideResultUI()
    {
        _goalUI.SetActive(false);
        _saveUI.SetActive(false);
    }

    public void AddScore(bool shooterWin)
    {
        if (shooterWin)
            _p1Score++;
        else
            _p2Score++;

        UpdateScoreUI();
        CheckWinner();
    }

    private void UpdateScoreUI()
    {
        _p1ScoreText.text = _p1Score.ToString();
        _p2ScoreText.text = _p2Score.ToString();
    }

    private void CheckWinner()
    {
        if (_p1Score >= MaxScore)
        {
            ShowWinUI(true);
            MinigameManager.instance.Finish(true);
        }
        else if (_p2Score >= MaxScore)
        {
            ShowWinUI(false);
            MinigameManager.instance.Finish(false);
        }
    }

    private void ShowWinUI(bool p1Win)
    {
        HideResultUI();
        _p1WinUI.SetActive(p1Win);
        _p2WinUI.SetActive(!p1Win);
    }
}
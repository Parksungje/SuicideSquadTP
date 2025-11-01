using UnityEngine;
using TMPro;

public enum ScoreType
{
    Plus, Minus, Times, Divide
}

public class ScoreComponent : MonoBehaviour
{
    private RunningGameManager _gameManager;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScoreType _scoreType;
    [SerializeField] private bool _randomizeType = false;

    private int _scorePM;
    private int _scoreTD;

    private bool _p1Used = false;
    private bool _p2Used = false;

    private void Start()
    {
        _gameManager = FindAnyObjectByType<RunningGameManager>();

        if (_randomizeType)
            _scoreType = GetBalancedScoreType();

        _scorePM = Random.Range(5, 50);
        _scoreTD = Random.Range(2, 5);

        if (_scoreText != null)
        {
            switch (_scoreType)
            {
                case ScoreType.Plus:
                    _scoreText.text = $"+{_scorePM}";
                    break;
                case ScoreType.Minus:
                    _scoreText.text = $"-{_scorePM}";
                    break;
                case ScoreType.Times:
                    _scoreText.text = $"¡¿{_scoreTD}";
                    break;
                case ScoreType.Divide:
                    _scoreText.text = $"¡À{_scoreTD}";
                    break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("P1") && !_p1Used)
        {
            _p1Used = true;
            ApplyScore(ref _gameManager.p1Score);
        }
        else if (other.CompareTag("P2") && !_p2Used)
        {
            _p2Used = true;
            ApplyScore(ref _gameManager.p2Score);
        }

        _gameManager.UpdateScoreUI();
    }

    private void ApplyScore(ref int score)
    {
        switch (_scoreType)
        {
            case ScoreType.Plus:
                score += _scorePM;
                break;
            case ScoreType.Minus:
                score -= _scorePM;
                break;
            case ScoreType.Times:
                score *= _scoreTD;
                break;
            case ScoreType.Divide:
                if (_scoreTD != 0)
                    score /= _scoreTD;
                break;
        }

        if (score > 1000) score = 1000;
    }


    private ScoreType GetBalancedScoreType()
    {
        int r = Random.Range(0, 100);
        if (r < 40) return ScoreType.Plus; 
        if (r < 65) return ScoreType.Minus;
        if (r < 85) return ScoreType.Times;
        return ScoreType.Divide;           
    }
}

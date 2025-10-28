using UnityEngine;
using TMPro;

public enum ScoreType
{
    Plus, Minus, Times, Divide
}

public class ScoreComponent : MonoBehaviour
{
    [SerializeField] private RunningGameManager _gameManager;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private ScoreType _scoreType;
    [SerializeField] private bool _randomizeType = false;

    private int _scorePM;
    private int _scoreTD;
    private bool _used = false;

    private void Start()
    {
        if (_randomizeType)
            _scoreType = (ScoreType)Random.Range(0, 4);

        _scorePM = Random.Range(10, 100);
        _scoreTD = Random.Range(2, 10);

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
        if (_used) return;
        _used = true;

        switch (_scoreType)
        {
            case ScoreType.Plus:
                _gameManager.currentScore += _scorePM;
                break;
            case ScoreType.Minus:
                _gameManager.currentScore -= _scorePM;
                break;
            case ScoreType.Times:
                _gameManager.currentScore *= _scoreTD;
                break;
            case ScoreType.Divide:
                _gameManager.currentScore /= _scoreTD;
                break;
        }

        _gameManager.UpdateScoreUI();
        Destroy(gameObject);
    }
}

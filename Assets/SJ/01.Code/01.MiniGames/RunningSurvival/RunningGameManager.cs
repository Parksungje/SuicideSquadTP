using Code.Player;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class RunningGameManager : MonoBehaviour
{
    [field: SerializeField] private RunningGameSO _runningInput;
    [SerializeField] private GameObject _p1Obj;
    [SerializeField] private GameObject _p2Obj;
    [SerializeField] private GameObject _scoreBoard;
    [SerializeField] private TMP_Text _scoreText;

    private bool _aPressed, _dPressed, _leftArrowPressed, _rightArrowPressed;
    private Coroutine _spawnCoroutine;

    public int currentScore;

    private void OnEnable()
    {
        if (_runningInput == null) return;

        _runningInput.OnAKeyDown += SetP1A;
        _runningInput.OnDKeyDown += SetP1D;
        _runningInput.OnLeftArrowDown += SetP2L;
        _runningInput.OnRightArrowDown += SetP2R;

        _spawnCoroutine = StartCoroutine(ScoreBoardSpawnLoop());
    }

    private void OnDestroy()
    {
        if (_runningInput == null) return;

        _runningInput.OnAKeyDown -= SetP1A;
        _runningInput.OnDKeyDown -= SetP1D;
        _runningInput.OnLeftArrowDown -= SetP2L;
        _runningInput.OnRightArrowDown -= SetP2R;

        if (_spawnCoroutine != null)
            StopCoroutine(_spawnCoroutine);
    }

    private void SetP1A(bool isPressed) => _aPressed = isPressed;
    private void SetP1D(bool isPressed) => _dPressed = isPressed;
    private void SetP2L(bool isPressed) => _leftArrowPressed = isPressed;
    private void SetP2R(bool isPressed) => _rightArrowPressed = isPressed;

    private void FixedUpdate()
    {
        if (_aPressed) _p1Obj.transform.DOMove(new Vector3(-20, 0, 15), 0.4f);
        if (_dPressed) _p1Obj.transform.DOMove(new Vector3(-6.5f, 0, 15), 0.4f);
        if (_leftArrowPressed) _p2Obj.transform.DOMove(new Vector3(6.5f, 0, 15), 0.4f);
        if (_rightArrowPressed) _p2Obj.transform.DOMove(new Vector3(20, 0, 15), 0.4f);
    }

    public void UpdateScoreUI()
    {
        if (_scoreText == null) return;
        _scoreText.text = currentScore.ToString();
        _scoreText.transform.DOKill();
        _scoreText.transform.localScale = Vector3.one;
        _scoreText.transform.DOScale(1.3f, 0.15f).SetLoops(2, LoopType.Yoyo);
    }

    private IEnumerator ScoreBoardSpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            Vector3 startPos = new Vector3(5, 7, 200);
            Vector3 endPos = new Vector3(5, 7, -20);
            float moveDuration = 10f;

            GameObject newScoreBoard = Instantiate(_scoreBoard, startPos, Quaternion.identity);
            newScoreBoard.transform.DOMove(endPos, moveDuration).SetEase(Ease.Linear).OnComplete(() => Destroy(newScoreBoard));
        }
    }
}

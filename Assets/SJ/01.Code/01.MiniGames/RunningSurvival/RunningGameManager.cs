using Code.Player;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Tild.Menu;

public class RunningGameManager : MonoBehaviour
{
    [field: SerializeField] private RunningGameSO _runningInput;
    [SerializeField] private GameObject _p1Obj;
    [SerializeField] private GameObject _p2Obj;
    [SerializeField] private GameObject _scoreBoard;
    [SerializeField] private TMP_Text _p1Score;
    [SerializeField] private TMP_Text _p2Score;
    [SerializeField] private float directionCooldown = 0.5f;
    [SerializeField] private int totalBoards = 15;
    [SerializeField] private float boardSpawnDelay = 3f;
    [SerializeField] private CanvasGroup resultPanel;
    [SerializeField] private TMP_Text resultText;

    private bool _aPressed, _dPressed, _leftArrowPressed, _rightArrowPressed;
    private Coroutine _spawnCoroutine;
    private bool _p1CanChange = true;
    private bool _p2CanChange = true;

    public int p1Score;
    public int p2Score;
    private int _spawnedCount = 0;
    private bool _gameEnded = false;

    private void OnEnable()
    {
        SoundManager.Instance.Play("Run_BGM");
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
        if (_spawnCoroutine != null) StopCoroutine(_spawnCoroutine);
    }

    private void SetP1A(bool isPressed)
    {
        if (!_p1CanChange || !isPressed || _gameEnded) return;
        _aPressed = true;
        _dPressed = false;
        StartCoroutine(P1DirectionCooldown());
    }

    private void SetP1D(bool isPressed)
    {
        if (!_p1CanChange || !isPressed || _gameEnded) return;
        _dPressed = true;
        _aPressed = false;
        StartCoroutine(P1DirectionCooldown());
    }

    private void SetP2L(bool isPressed)
    {
        if (!_p2CanChange || !isPressed || _gameEnded) return;
        _leftArrowPressed = true;
        _rightArrowPressed = false;
        StartCoroutine(P2DirectionCooldown());
    }

    private void SetP2R(bool isPressed)
    {
        if (!_p2CanChange || !isPressed || _gameEnded) return;
        _rightArrowPressed = true;
        _leftArrowPressed = false;
        StartCoroutine(P2DirectionCooldown());
    }

    private IEnumerator P1DirectionCooldown()
    {
        _p1CanChange = false;
        yield return new WaitForSeconds(directionCooldown);
        _p1CanChange = true;
    }

    private IEnumerator P2DirectionCooldown()
    {
        _p2CanChange = false;
        yield return new WaitForSeconds(directionCooldown);
        _p2CanChange = true;
    }

    private void FixedUpdate()
    {
        if (_gameEnded) return;
        if (_aPressed) _p1Obj.transform.DOMove(new Vector3(-13, 0, 15), 0.4f);
        if (_dPressed) _p1Obj.transform.DOMove(new Vector3(13f, 0, 15), 0.4f);
        if (_leftArrowPressed) _p2Obj.transform.DOMove(new Vector3(-13f, 0, 15), 0.4f);
        if (_rightArrowPressed) _p2Obj.transform.DOMove(new Vector3(13, 0, 15), 0.4f);
    }

    public void UpdateScoreUI()
    {
        _p1Score.text = $"{p1Score}";
        _p2Score.text = $"{p2Score}";
    }

    private IEnumerator ScoreBoardSpawnLoop()
    {
        while (_spawnedCount < totalBoards)
        {
            yield return new WaitForSeconds(boardSpawnDelay);
            Vector3 startPos = new Vector3(0, 7, 200);
            Vector3 endPos = new Vector3(0, 7, -20);
            float moveDuration = 5f;
            GameObject newScoreBoard = Instantiate(_scoreBoard, startPos, Quaternion.identity);
            newScoreBoard.transform.DOMove(endPos, moveDuration)
                .SetEase(Ease.Linear)
                .OnComplete(() => Destroy(newScoreBoard));
            _spawnedCount++;
        }
        yield return new WaitForSeconds(5f);
        ShowResult();
    }

    private void ShowResult()
    {
        _gameEnded = true;
        string resultMsg;
        //if (p1Score > p2Score) resultMsg = "P1 ½Â¸®!";
        //else if (p1Score < p2Score) resultMsg = "P2 ½Â¸®!";
        //else resultMsg = "¹«½ÂºÎ!";
        resultMsg = "°ÔÀÓ Á¾·á!";
        resultText.text = resultMsg;
        resultPanel.gameObject.SetActive(true);
        resultPanel.alpha = 0;
        resultPanel.DOFade(1f, 0.8f).SetEase(Ease.OutQuad);
        if (p1Score != p2Score) MinigameManager.instance?.Finish(p1Score > p2Score);
    }
}

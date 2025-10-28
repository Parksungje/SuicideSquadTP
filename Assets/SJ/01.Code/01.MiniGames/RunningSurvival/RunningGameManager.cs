using Code.Player;
using System;
using UnityEngine;
using DG.Tweening; // DOTween »ç¿ë
using System.Security.Cryptography;
using System.Collections;

public class RunningGameManager : MonoBehaviour
{
    [field: SerializeField] private RunningGameSO _runningInput;
    [SerializeField] private GameObject _p1Obj;
    [SerializeField] private GameObject _p2Obj;

    [SerializeField] private GameObject _scoreBoard;

    private bool _aPressed, _dPressed, _leftArrowPressed, _rightArrowPressed;

    private Coroutine _spawnCoroutine;

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
        {
            StopCoroutine(_spawnCoroutine);
        }
    }

    private void SetP1A(bool isPressed)
    {
        _aPressed = isPressed;
    }

    private void SetP1D(bool isPressed)
    {
        _dPressed = isPressed;
    }

    private void SetP2L(bool isPressed)
    {
        _leftArrowPressed = isPressed;
    }

    private void SetP2R(bool isPressed)
    {
        _rightArrowPressed = isPressed;
    }

    private void FixedUpdate()
    {
        if (_aPressed) _p1Obj.transform.DOMove(new Vector3(-18, 0, 15), 0.3f);
        if (_dPressed) _p1Obj.transform.DOMove(new Vector3(-5, 0, 15), 0.3f);

        if (_leftArrowPressed) _p2Obj.transform.DOMove(new Vector3(5, 0, 15), 0.3f);
        if (_rightArrowPressed) _p2Obj.transform.DOMove(new Vector3(18, 0, 15), 0.3f);
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
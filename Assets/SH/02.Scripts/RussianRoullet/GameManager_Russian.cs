using System;
using System.Collections;
using System.Collections.Generic;
using Tild.Menu;
using UnityEditor;
using UnityEngine;

[DisallowMultipleComponent]
public class GameManager_Russian : MonoBehaviour
{
    [SerializeField] private RussianRoulletSO inputSO;
    [SerializeField] private int targetScore = 3;
    [SerializeField, Range(0f, 1f)] private float twoBulletProbability = 2f / 3f;

    public event Action<bool> TurnChanged;
    public event Action<int, int> ScoreChanged;
    public event Action<int> RoundStarted;
    public event Action RoundEnded;
    public event Action<int> ShowChance;
    public event Action<int> GuidUI;
    public event Action HideGuid;

    private Polishing_RussianRoulette polishing;

    public int ScoreP1 => scoreP1;
    public int ScoreP2 => scoreP2;
    public bool IsP1Turn => isP1Turn;

    private int scoreP1;
    private int scoreP2;
    private int roundIndex;
    private readonly bool[] cylinder = new bool[6];
    private int chamberIndex;
    private bool isP1Turn;
    private bool roundActive = false;
    private bool inputBusy;

    private void Start()
    {
        polishing = GetComponent<Polishing_RussianRoulette>();
    }

    public void GameStart()
    {
        StartNewMatch();
    }

    private void OnEnable()
    {
        if (inputSO != null)
        {
            inputSO.OnL_Click += OnP1Click;
            inputSO.OnR_Click += OnP2Click;
        }
    }

    private void OnDisable()
    {
        if (inputSO != null)
        {
            inputSO.OnL_Click -= OnP1Click;
            inputSO.OnR_Click -= OnP2Click;
        }
    }

    private void StartNewMatch()
    {
        scoreP1 = 0;
        scoreP2 = 0;
        roundIndex = 0;
        Debug.Log($"[Russian] New Match started. Target Score = {targetScore}");
        ScoreChanged?.Invoke(scoreP1, scoreP2);
        StartRound();
    }

    private void StartRound()
    {
        polishing.SetAnimationToIdle();

        roundIndex++;
        Array.Clear(cylinder, 0, cylinder.Length);

        int bulletCount = UnityEngine.Random.value < twoBulletProbability ? 2 : 1;
        int placed = 0;
        while (placed < bulletCount)
        {
            int idx = UnityEngine.Random.Range(0, 6);
            if (!cylinder[idx])
            {
                cylinder[idx] = true;
                placed++;
            }
        }

        chamberIndex = UnityEngine.Random.Range(0, 6);
        isP1Turn = UnityEngine.Random.value < 0.5f;
        roundActive = true;
        inputBusy = false;

        polishing.SetCamera(isP1Turn ? 1 : 2);
        polishing.SetAnimatorHolding(isP1Turn ? 1 : 2);
        polishing.SetAnimatorScaring(isP1Turn ? 2 : 1);

        GuidUI?.Invoke(isP1Turn ? 1 : 2);
        ShowChance?.Invoke(bulletCount);

        RoundStarted?.Invoke(roundIndex);
        TurnChanged?.Invoke(isP1Turn);
    }

    private void OnP1Click()
    {
        if (!roundActive || inputBusy) return;
        if (!isP1Turn) { return; }
        StartCoroutine(ResolveTrigger("P1"));
    }

    private void OnP2Click()
    {
        if (!roundActive || inputBusy) return;
        if (isP1Turn) { return; }
        StartCoroutine(ResolveTrigger("P2"));
    }

    private IEnumerator ResolveTrigger(string who)
    {
        inputBusy = true;
        HideGuid?.Invoke();

        bool fire = cylinder[chamberIndex];
        RoundEnded?.Invoke();

        if (fire)
        {
            //발사
            polishing.SetAnimatorFire(isP1Turn ? 1 : 2);
            polishing.SetAnimatorDeath(isP1Turn ? 2 : 1);
            polishing.GunLight();

            if (who == "P1") scoreP1++; else scoreP2++;
            ScoreChanged?.Invoke(scoreP1, scoreP2);

            if (scoreP1 >= targetScore || scoreP2 >= targetScore)
            {
                string winner = scoreP1 > scoreP2 ? "P1" : "P2";
                roundActive = false;

                yield return new WaitForSeconds(3f);
                MinigameManager.instance.Finish(scoreP1 > scoreP2);
                yield break;
            }

            roundActive = false;
            yield return new WaitForSeconds(3f);
            StartRound();
        }
        else
        {
            chamberIndex = (chamberIndex + 1) % 6;
            isP1Turn = !isP1Turn;

            //안발사
            yield return new WaitForSeconds(3f);

            TurnChanged?.Invoke(isP1Turn);

            polishing.SetCamera(isP1Turn ? 1 : 2);
            polishing.SetAnimatorHolding(isP1Turn ? 1 : 2);
            polishing.SetAnimatorScaring(isP1Turn ? 2 : 1);

            GuidUI?.Invoke(isP1Turn ? 1 : 2);

            inputBusy = false;
        }
    }
}
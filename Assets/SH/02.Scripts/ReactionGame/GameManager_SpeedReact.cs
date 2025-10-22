using UnityEngine;
using TMPro;
using System.Collections;
using System.Diagnostics;
using Tild._1Script.Menu;

public class GameManager_SpeedReact : MonoBehaviour
{
    [SerializeField] private Renderer screenColor;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Light environmentLight;
    public int winScore = 3;

    private Stopwatch stopWatch;
    private Coroutine roundCoroutine;

    private bool canReact = false;
    private bool roundEnded = false;
    private bool isGreen = false;
    private int scoreL = 0;
    private int scoreR = 0;

    private void Start()
    {
        stopWatch = new Stopwatch();
        roundCoroutine = StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        roundEnded = false;
        canReact = false;
        isGreen = false;

        resultText.text = "";
        SetColor(Color.red);

        yield return new WaitForSeconds(Random.Range(2f, 4f));

        SetColor(Color.green);
        isGreen = true;
        canReact = true;
        stopWatch.Restart();
    }

    public void RegisterReaction(bool isLeft)
    {
        if (roundEnded) return;
        roundEnded = true;

        if (roundCoroutine != null)
        {
            StopCoroutine(roundCoroutine);
            roundCoroutine = null;
        }

        if (!isGreen)
        {
            string offender = isLeft ? "P1" : "P2";
            string winner = isLeft ? "P2" : "P1";
            if (isLeft) scoreR++; else scoreL++;

            SetColor(Color.red);
            StartCoroutine(ShowResult($"{offender} ¹ÝÄ¢! {winner} +1Á¡", true));
            return;
        }

        if (!canReact) return;

        canReact = false;
        stopWatch.Stop();
        float reactionTime = stopWatch.ElapsedMilliseconds;

        string winnerName = isLeft ? "P1" : "P2";
        if (isLeft) scoreL++; else scoreR++;

        StartCoroutine(ShowResult($"{winnerName} ½Â¸®! ({reactionTime} ms)", false));
    }

    private IEnumerator ShowResult(string message, bool foul)
    {
        resultText.text = message;
        scoreText.text = $"P1: {scoreL} | P2: {scoreR}";

        yield return new WaitForSeconds(3f);

        if (scoreL >= winScore || scoreR >= winScore)
        {
            string finalWinner = scoreL > scoreR ? "P1" : "P2";
            resultText.text = $"{finalWinner} ¿ì½Â!!";
            yield break;
        }

        roundCoroutine = StartCoroutine(RoundRoutine());
    }

    private void SetColor(Color c)
    {
        screenColor.material.color = c;
        RenderSettings.fogColor = c;
        environmentLight.color = c;
        screenColor.material.SetColor("_EmissionColor", c * 4);
    }
}
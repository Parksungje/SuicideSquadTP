using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class GameManager_SpeedReact : MonoBehaviour
{
    [SerializeField] private MeshRenderer screenColor;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int winScore = 3;

    private bool canReact = false;
    private bool roundEnded = false;
    private float greenTime;
    private int scoreL = 0;
    private int scoreR = 0;

    private void Start()
    {
        StartCoroutine(RoundRoutine());
    }

    private IEnumerator RoundRoutine()
    {
        roundEnded = false;
        canReact = false;
        resultText.text = "";
        screenColor.material.color = Color.red;

        yield return new WaitForSeconds(Random.Range(2f, 4f));

        screenColor.material.color = Color.green;
        greenTime = Time.time;
        canReact = true;
    }

    public void RegisterReaction(bool isLeft)
    {
        if (!canReact || roundEnded) return;

        roundEnded = true;
        canReact = false;

        float reactionTime = (Time.time - greenTime) * 1000f;
        string winner = isLeft ? "Left" : "Right";

        if (isLeft) scoreL++;
        else scoreR++;

        StartCoroutine(ShowResult(winner, reactionTime));
    }

    private IEnumerator ShowResult(string winner, float reactionTime)
    {
        resultText.text = $"{winner} Wins! ({reactionTime:F0} ms)";
        scoreText.text = $"L: {scoreL} | R: {scoreR}";

        yield return new WaitForSeconds(3f);

        if (scoreL >= winScore || scoreR >= winScore)
        {
            resultText.text = $"{winner} Player Wins the Game!";
            yield break;
        }

        StartCoroutine(RoundRoutine());
    }
}
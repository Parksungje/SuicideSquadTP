using UnityEngine;

public class GameManager_Tag : MonoBehaviour
{
    public int scoreA { private set; get; } = 0;
    public int scoreB { private set; get; } = 0;
    public int time { private set; get; } = 120;

    [Header("Game")]
    public TagPlayer hunter;

    public void InitializeGame()
    {
        scoreA = 0;
        scoreB = 0;
    }
}
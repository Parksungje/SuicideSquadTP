using System.Collections;
using UnityEngine;

public class GameManager_Tag : MonoBehaviour
{
    public float timeLimit = 90f;
    private float remainingTime;

    [Header("Game")]
    [SerializeField] private UIManager_Tag uiManager;
    public TagPlayer hunter;

    [Header("Players")]
    [SerializeField] private Collider[] players;
    private TagPlayer player1Tag;
    private TagPlayer player2Tag;

    private bool gameActive = false;

    private void Start()
    {
        player1Tag = players[0].GetComponent<TagPlayer>();
        player2Tag = players[1].GetComponent<TagPlayer>();

        //Physics.IgnoreCollision(players[0], players[1]);
        InitializeGame();
    }

    public void InitializeGame()
    {
        remainingTime = timeLimit;
        gameActive = true;

        SetHunter(player1Tag);
        StartCoroutine(GameTimer());
    }

    private IEnumerator GameTimer()
    {
        while (remainingTime > 0f)
        {
            remainingTime -= Time.deltaTime;
            uiManager.UpdateTimer(remainingTime);
            yield return null;
        }

        remainingTime = 0f;
        uiManager.UpdateTimer(remainingTime);
        EndGame();
    }

    private void EndGame()
    {
        gameActive = false;
        uiManager.ResultText(hunter == player1Tag ? 2 : 1);
        Time.timeScale = 0f;
    }

    public void SetHunter(TagPlayer newHunter)
    {
        hunter = newHunter;
        player1Tag.SetIsHunter(hunter == player1Tag);
        player2Tag.SetIsHunter(hunter == player2Tag);
    }

    public void OnPlayerTagged(TagPlayer tagged)
    {
        if (hunter == tagged) return;
        SetHunter(tagged);
        print("¼ú·¡ ¹Ù²ñ");
    }
}
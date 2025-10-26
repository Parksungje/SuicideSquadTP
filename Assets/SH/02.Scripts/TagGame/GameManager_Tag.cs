using System.Collections;
using UnityEngine;

public class GameManager_Tag : MonoBehaviour
{
    public float timeLimit = 90f;
    private float remainingTime;

    public bool collisionDebounce = true;

    [Header("Game")]
    [SerializeField] private UIManager_Tag uiManager;
    public bool _is1PHunter;

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

        SetHunter(true);
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
        uiManager.ResultText(_is1PHunter ? 2 : 1);
        Time.timeScale = 0f;
    }

    public void SetHunter(bool is1P)
    {
        _is1PHunter = is1P;
        player1Tag.SetIsHunter(is1P == true);
        player2Tag.SetIsHunter(is1P == false);
    }

    public void OnPlayerTagged(bool is1P)
    {
        print("¼ú·¡ ¹Ù²ñ");
        SetHunter(!is1P);
    
    }
}
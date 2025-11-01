using System.Collections;
using Tild.Menu;
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

    [SerializeField] private GameObject bombFuseEffect1P, bombFuseEffect2P, explodeFuseEffect1P,explodeFuseEffect2P;
    [SerializeField] private GameObject celebEffect1P, celebEffect2P;
    [SerializeField] private ParticleSystem madEmoji1P, madEmoji2P;
    [SerializeField] private GameObject celebCamera1P, celebCamera2P;
    
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
        explodeFuseEffect1P.SetActive(!_is1PHunter);
        explodeFuseEffect2P.SetActive(_is1PHunter);
        
        yield return new WaitForSeconds(1.5f);
        Destroy(!_is1PHunter ? players[0].transform.parent.gameObject : players[1].transform.parent.gameObject);
        yield return new WaitForSeconds(2);
        uiManager.ResultText(_is1PHunter ? 2 : 1);
        
        celebCamera1P.SetActive(!_is1PHunter);
        celebCamera2P.SetActive(_is1PHunter);
        
        celebEffect1P.SetActive(!_is1PHunter);
        celebEffect2P.SetActive(_is1PHunter);
        
        yield return new WaitForSeconds(3);
        MinigameManager.instance.Finish(!_is1PHunter);
        gameActive = false;
        
        Time.timeScale = 0f;
        
    }

   
    public void SetHunter(bool is1P)
    {
        _is1PHunter = is1P;
        player1Tag.SetIsHunter(_is1PHunter);
        player2Tag.SetIsHunter(!_is1PHunter);
        
        bombFuseEffect1P.SetActive(is1P);
        bombFuseEffect2P.SetActive(!is1P);

        if (is1P)
        {
            madEmoji1P.Play();
        }
        else
        {
            madEmoji2P.Play();
        }
        
        print("1P" +_is1PHunter);
        uiManager.AlertText(_is1PHunter ? 1 : 2);
   
        collisionDebounce = false;
        Invoke(nameof(Debounce),0.5f);
    }

    public void Debounce()
    {
        collisionDebounce = true;
    }

    public void OnPlayerTagged(bool is1P) 
    {
        print("���� �ٲ�");
        SetHunter(!is1P);
    
    }
}
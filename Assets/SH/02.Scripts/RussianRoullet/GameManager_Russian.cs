//using Code.Players;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.Rendering.VirtualTexturing;

//public class GameManager_Russian : MonoBehaviour
//{
//    public static GameManager_Russian Instance;

//    [SerializeField] private List<Player> players;
//    [SerializeField] private Revolver revolver;

//    private int currentPlayerIndex;
//    private int currentRound = 1;
//    private bool roundActive = false;

//    void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);
//    }

//    void Start()
//    {
//        StartRound();
//    }

//    void StartRound()
//    {
//        Debug.Log($"===== ROUND {currentRound} 시작 =====");
//        revolver.ReloadRandom();
//        foreach (var p in players) p.Revive();

//        currentPlayerIndex = Random.Range(0, players.Count);
//        roundActive = true;
//        Debug.Log($"{players[currentPlayerIndex].playerName}이(가) 먼저 시작합니다.");
//    }

//    public void OnShootButton()
//    {
//        if (!roundActive) return;

//        Player current = players[currentPlayerIndex];
//        bool fired = revolver.Fire();

//        if (fired)
//        {
//            Debug.Log($"{current.playerName}이(가) 사망했습니다!");
//            current.Die();

//            roundActive = false;
//            EndRound();
//        }
//        else
//        {
//            Debug.Log($"{current.playerName} 생존. 다음 턴으로 넘어갑니다.");
//            NextTurn();
//        }
//    }

//    void NextTurn()
//    {
//        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
//        if (!players[currentPlayerIndex].isAlive)
//            NextTurn();
//    }

//    void EndRound()
//    {
//        Debug.Log($"===== ROUND {currentRound} 종료 =====");

//        currentRound++;

//        if (currentRound <= 3)
//            Invoke(nameof(StartRound), 2f); // 2초 후 다음 라운드
//        else
//            EndGame();
//    }

//    void EndGame()
//    {
//        Debug.Log("===== 게임 종료 =====");

//        int minDeath = int.MaxValue;
//        Player winner = null;

//        foreach (var p in players)
//        {
//            Debug.Log($"{p.playerName}: 사망 {p.deathCount}회");
//            if (p.deathCount < minDeath)
//            {
//                minDeath = p.deathCount;
//                winner = p;
//            }
//        }

//        Debug.Log($"🏆 승자: {winner.playerName}!");
//    }
//}

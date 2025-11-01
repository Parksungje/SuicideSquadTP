using System.Collections.Generic;
using Febucci.UI;
using Tild.MiscUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tild.Menu
{
    public class MinigameManager : MonoBehaviour
    {
        public static MinigameManager instance = null;
        
        public GameType gameType { get; set; }
      
        public bool isRandomMode { get; set; }
        
        private int amount;
        private int round;
        public int _1PScore;
        public int _2PScore;
        
        private ResultUI resultUI;
        
        public List<MinigameSO> minigamePlayed;
        void Awake()
        {
          
            if (instance == null) 
                instance = this; 
         
      
            else if (instance != this) 
                Destroy(gameObject);
            
           
            DontDestroyOnLoad(gameObject); 
            
            resultUI = GetComponent<ResultUI>();
        }

        public void Initialize(GameType gameType, bool isRandom, int amount)
        {
            this.gameType = gameType;
            this.isRandomMode = isRandom;
            this.amount = amount;
            
        }

        public void NextMinigame(string scene)
        {
            TransitionManager.Go(scene);
        }

        public void Finish(bool is1Pwin)
        {
            Time.timeScale = 0;
            resultUI.ViewResult(_1PScore, _2PScore, is1Pwin, (() =>
            {
                if (is1Pwin) _1PScore++;
                else _2PScore++;

                Time.timeScale = 1;

                if (gameType == GameType.Round)
                {
                    round++;
                    if (round == amount)
                    {
                        TransitionManager.Go("ResultScene");
                        return;
                    }
                }
                else if (gameType == GameType.Score)
                {
                    if (_1PScore == amount || _2PScore == amount)
                    {
                        TransitionManager.Go("ResultScene");
                        return;
                    }
                }

                TransitionManager.Go("Choice_Scene");
            }));
        }


        public bool GetWinner()
        {
            return _1PScore > _2PScore;
        }
    }

    public enum GameType
    {
        Round, Score, Team
    }
    
}
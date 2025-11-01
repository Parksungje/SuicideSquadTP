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
        private int _1PScore;
        private int _2PScore;
        
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
            SceneManager.LoadScene(scene);
        }

        public void Finish(bool is1Pwin)
        {
            Time.timeScale = 0;
            if (is1Pwin) _1PScore++;
            else _2PScore++;

            resultUI.ViewResult(_1PScore, _2PScore, is1Pwin, (() =>
            {
                Time.timeScale = 1;
                SceneManager.LoadScene("Choice_Scene");
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
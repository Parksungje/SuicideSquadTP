using System.Collections.Generic;
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
        
        
        
        public List<MinigameSO> minigamePlayed;
        void Awake()
        {
          
            if (instance == null) 
                instance = this; 
         
      
            else if (instance != this) 
                Destroy(gameObject);
            
           
            DontDestroyOnLoad(gameObject); 
            
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
            if (is1Pwin) _1PScore++;
            else _2PScore++; 
            
            SceneManager.LoadScene("Choice_Scene");
        }
    }

    public enum GameType
    {
        Round, Score, Team
    }
    
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tild._1Script.Menu
{
    public class MinigameManager : MonoBehaviour
    {
        public static MinigameManager instance = null;
        
        private GameType gameType;
        private int amount;
        private bool isRandomMode = false;
        
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
            SceneManager.LoadScene("GameChoice");
        }

        public void NextMinigame(string scene)
        {
            SceneManager.LoadScene(scene);
        }
    }

    public enum GameType
    {
        Round, Score, Team
    }
    
}
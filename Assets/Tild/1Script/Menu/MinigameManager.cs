using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tild._1Script.Menu
{
    public class MinigameManager : MonoBehaviour
    {
        public static MinigameManager instance = null;
        
        private GameType gameType;
        private bool isRandomMode = false;
        void Awake()
        {
          
            if (instance == null) 
                instance = this; 
         
      
            else if (instance != this) 
                Destroy(gameObject);
            
           
            DontDestroyOnLoad(gameObject); 
            
        }

        public void Initialize(GameType gameType, bool isRandom)
        {
            this.gameType = gameType;
            this.isRandomMode = isRandom;
            SceneManager.LoadScene("GameChoice");
        }
    }

    public enum GameType
    {
        Round, Score, Team
    }
    
}
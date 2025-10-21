using UnityEngine;

namespace Tild._1Script.Menu
{
    public class MinigameInfoManager : MonoBehaviour
    {
        public static MinigameInfoManager instance;
        void Awake()
        {
          
            if (instance == null) 
                instance = this; 
         
      
            else if (instance != this) 
                Destroy(gameObject);
            
           
            DontDestroyOnLoad(gameObject); 
            
        }

    }
}
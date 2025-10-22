using DG.Tweening;
using UnityEngine;

namespace Tild.Menu
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager instance;
        
        [SerializeField] private RectTransform settingPanel;
        [SerializeField] private CanvasGroup fadeImage;
        void Awake()
        {
          
            if (instance == null) 
                instance = this; 
         
      
            else if (instance != this) 
                Destroy(gameObject);
            
           
            DontDestroyOnLoad(gameObject); 
            
        }

        public void FadeIn(float duration = 0.3f, System.Action onComplete = null)
        {
            fadeImage.DOFade(1f, duration)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }

        public void FadeOut(float duration = 0.3f,float delay = 0f, System.Action onComplete = null)
        {
            fadeImage.DOFade(0f, duration).SetDelay(delay)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
        }
    }
}
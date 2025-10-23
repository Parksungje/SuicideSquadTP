using Tild.Core;
using Tild.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Tild._1Script.Menu
{
    public class MinigameSelectButton : MonoBehaviour
    {
        [SerializeField] private RectTransform buttonRect;
        [SerializeField] private TMP_Text buttonText;
        [SerializeField] private Image minigameImage;
        private readonly OnMinigameBtnClicked OnMinigameBtnClicked = MenuEventChannels.OnMinigameBtnClicked;
        private string sceneName;
        private MinigameSO currentMinigame;

        public void Initialize(MinigameSO minigame)
        {
            currentMinigame = minigame;
            minigameImage.sprite = minigame.playScreen;
            buttonText.text = minigame.gameName;
            sceneName = minigame.scene;
        }

        public void OnPressed()
        {
            GameEventBus.RaiseEvent(OnMinigameBtnClicked.Initializer(currentMinigame));
        }
    }
}
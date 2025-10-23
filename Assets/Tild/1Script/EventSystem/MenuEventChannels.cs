using Tild.Menu;

namespace Tild.Core
{
    public class MenuEventChannels
    {
        public static OnMinigameBtnClicked OnMinigameBtnClicked = new OnMinigameBtnClicked();
    }

    public class OnMinigameBtnClicked : GameEvent
    {
        public MinigameSO minigame;

        public OnMinigameBtnClicked Initializer(MinigameSO minigame)
        {
            this.minigame = minigame;
            return this;
        }
    }
}
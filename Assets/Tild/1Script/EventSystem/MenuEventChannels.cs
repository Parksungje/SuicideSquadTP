using Tild.Menu;

namespace Tild.Core
{
    public class MenuEventChannels
    {
        public static OnMinigameBtnClicked OnMinigameBtnClicked = new OnMinigameBtnClicked();
    }

    public class OnMinigameBtnClicked : GameEvent
    {
        public MinigameSO Minigame;

        public OnMinigameBtnClicked Initializer(MinigameSO Minigame)
        {
            this.Minigame = Minigame;
            return this;
        }
    }
}
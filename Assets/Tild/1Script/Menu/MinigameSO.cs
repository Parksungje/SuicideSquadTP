using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tild.Menu
{
    [CreateAssetMenu(fileName = "Minigame", menuName = "SO/Tild/Minigame", order = 0)]
    public class MinigameSO : ScriptableObject
    {
        public string gameName;
        public string description;
        public Sprite playScreen;
        public string scene;
        public Color backgroundColor;
    }
}
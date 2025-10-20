using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tild._1Script.Menu
{
    [CreateAssetMenu(fileName = "Minigame", menuName = "SO/Tild/Minigame", order = 0)]
    public class MinigameSO : ScriptableObject
    {
        public string Name;
        public string Description;
        public Sprite PlayScreen;
        public Scene scene;
    }
}
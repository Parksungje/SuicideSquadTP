using UnityEngine;

public class Revolver : MonoBehaviour
{
    private int chamberCount = 6;
    private bool[] chambers;
    private int currentIndex = 0;

    public void ReloadRandom()
    {
        chambers = new bool[chamberCount];
        currentIndex = 0;

        float rand = Random.value;
        int bulletCount = rand < 0.5f ? 1 : 4;
        for (int i = 0; i < bulletCount; i++)
        {
            int idx;
            do { idx = Random.Range(0, chamberCount); }
            while (chambers[idx]);
            chambers[idx] = true;
        }

        Debug.Log($"ÅºÃ¢ ÀåÀü ¿Ï·á: {bulletCount}¹ß (È®·ü: {(float)bulletCount / chamberCount * 100:F1}%)");
    }

    public bool Fire()
    {
        bool fired = chambers[currentIndex];
        Debug.Log($"¹ß»ç! (Ä­: {currentIndex + 1}/{chamberCount}) => {(fired ? "BANG!" : "CLICK")}");
        currentIndex = (currentIndex + 1) % chamberCount;
        return fired;
    }
}

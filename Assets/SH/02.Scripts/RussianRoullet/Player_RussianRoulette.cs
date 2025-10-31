using UnityEngine;

public class Player_RussianRoulette : MonoBehaviour
{
    public string playerName;
    public bool isAlive = true;
    public int deathCount = 0;

    public void Die()
    {
        isAlive = false;
        deathCount++;
    }

    public void Revive() => isAlive = true;
}

using UnityEngine;

public class TagPlayer : MonoBehaviour
{
    private bool isHunter = false;
    private GameManager_Tag manager;

    private void Start()
    {
        manager = FindFirstObjectByType<GameManager_Tag>();
    }

    public void SetIsHunter(bool value)
    {
        isHunter = value;
    }

    private void OnCollisionEnter(Collision other)
    {
        TagPlayer otherPlayer = other.gameObject.GetComponent<TagPlayer>();
        if (otherPlayer != null)
        {
            if (isHunter)
            {
                manager.OnPlayerTagged(otherPlayer);
            }
        }
    }
}

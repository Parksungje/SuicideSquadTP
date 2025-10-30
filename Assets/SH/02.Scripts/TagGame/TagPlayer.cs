using UnityEngine;

public class TagPlayer : MonoBehaviour
{
    private bool isHunter = false;
    

    [SerializeField] private bool _is1P;
    private GameManager_Tag manager;

    private void Start()
    {
        manager = FindFirstObjectByType<GameManager_Tag>();
    }

    public void SetIsHunter(bool value)
    {
        isHunter = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!manager.collisionDebounce) return;

        print("�浹");
        if (isHunter)
        {
            manager.collisionDebounce = false;
            print(_is1P);

            manager.OnPlayerTagged(_is1P);
            Invoke("DebounceDelay", 0.5f);
        }
        
    }
    private void DebounceDelay()
    {
        manager.collisionDebounce = true;
    }
}

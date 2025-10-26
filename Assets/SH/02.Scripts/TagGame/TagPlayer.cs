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

        manager.collisionDebounce = true;
        print("충돌");
        if (isHunter)
        {
            print("헌터 입니다.");

            manager.OnPlayerTagged(_is1P);
            Invoke("DebounceDelay", 2);
        }
        
    }
    private void DebounceDelay()
    {
        manager.collisionDebounce = false;
    }
}

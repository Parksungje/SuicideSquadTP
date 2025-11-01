using UnityEngine;

public class TagPlayer : MonoBehaviour
{
    private bool isHunter = false;
    

    [SerializeField] private bool _is1P;
    [SerializeField] private GameManager_Tag manager;

   
    public void SetIsHunter(bool value)
    {
        isHunter = value;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!manager.collisionDebounce) return;


        if (isHunter && other.CompareTag("Player"))
        {
            manager.collisionDebounce = false;
            print(_is1P);
            SoundManager.Instance.Play("Tag_Switch");

            manager.OnPlayerTagged(_is1P);
            Invoke("DebounceDelay", 0.5f);
        }
        
    }
    private void DebounceDelay()
    {
        manager.collisionDebounce = true;
    }
}

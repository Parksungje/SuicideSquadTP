using UnityEngine;

public class FinishLineTrigger : MonoBehaviour
{
    public GreenLightRedLight manager;
    private void OnTriggerEnter(Collider other)
    {
        if (!manager) return;
        manager.OnFinishTriggerEnter(other.transform);
    }
}
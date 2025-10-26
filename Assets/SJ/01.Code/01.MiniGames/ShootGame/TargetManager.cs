using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetManager : MonoBehaviour
{
    [SerializeField] private List<TargetComponent> targets;
    [SerializeField] private float spawnInterval = 2f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            var inactiveTargets = targets.FindAll(t => !t.gameObject.activeSelf);
            if (inactiveTargets.Count > 0)
            {
                var target = inactiveTargets[Random.Range(0, inactiveTargets.Count)];
                target.gameObject.SetActive(true);
                target.Activate();
            }

        }
    }
}

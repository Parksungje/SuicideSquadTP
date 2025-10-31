using UnityEngine;
using TMPro;

public class ScoreComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;



    private RunningGameManager _runningGameManager;

    private void Awake()
    {
        _runningGameManager = FindAnyObjectByType<RunningGameManager>();
    }

    private void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        
    }

}

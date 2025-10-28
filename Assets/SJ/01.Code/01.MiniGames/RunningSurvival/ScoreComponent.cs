using UnityEngine;
using TMPro;

public class ScoreComponent : MonoBehaviour
{
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private bool _randomizeType = false;



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

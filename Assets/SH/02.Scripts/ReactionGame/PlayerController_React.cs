using UnityEngine;

public class PlayerController_React : MonoBehaviour
{
    [SerializeField] private SpeedReactGameSO reactInput;
    [SerializeField] private GameManager_SpeedReact gameManager;

    private void OnEnable()
    {
        reactInput.OnL_Click += HandleLClick;
        reactInput.OnR_Click += HandleRClick;
    }

    private void OnDisable()
    {
        reactInput.OnL_Click -= HandleLClick;
        reactInput.OnR_Click -= HandleRClick;
    }

    private void HandleLClick()
    {
        gameManager?.RegisterReaction(true);
    }

    private void HandleRClick()
    {
        gameManager?.RegisterReaction(false);
    }
}

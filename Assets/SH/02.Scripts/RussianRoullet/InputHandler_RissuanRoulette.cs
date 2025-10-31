using System;
using UnityEngine;

public class InputHandler_RissuanRoulette : MonoBehaviour
{
    [SerializeField] private RussianRoulletSO russianRouletteInput;
    private GameManager_Russian gameManager;

    private void Start()
    {
        gameManager = GetComponent<GameManager_Russian>();
    }

    private void OnEnable()
    {
        russianRouletteInput.OnL_Click += HandleL_Click;
        russianRouletteInput.OnR_Click += HandleR_Click;
    }

    private void OnDisable()
    {
        russianRouletteInput.OnL_Click -= HandleL_Click;
        russianRouletteInput.OnR_Click -= HandleR_Click;
    }

    private void HandleR_Click()
    {
        if (gameManager.currentPlayerIndex == 0)
        {
            gameManager.OnShootButton();
        }
    }

    private void HandleL_Click()
    {
        if (gameManager.currentPlayerIndex == 1)
        {
            gameManager.OnShootButton();
        }
    }
}
using System;
using UnityEngine;

public class InputHandler_RissuanRoulette : MonoBehaviour
{
    [SerializeField] private RussianRoulletSO russianRouletteInput;

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

    private void HandleL_Click()
    {
    }

    private void HandleR_Click()
    {
    }
}
using Code.Player;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpeedReactGame Input", menuName = "8S/GameSO/SpeedReact")]
public class SpeedReactGameSO : BaseInputSO
{
    public event Action OnL_Click;
    public event Action OnR_Click;

    public void LeftClick() => OnL_Click?.Invoke();
    public void RightClick() => OnR_Click?.Invoke();
}

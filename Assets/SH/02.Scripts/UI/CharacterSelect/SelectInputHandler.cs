using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SelectInputHandler : MonoBehaviour
{
    [SerializeField] private CharacterSelectSO selectInput;

    public bool bothReady = false;

    [Header("AppearanceComponentes")]
    [SerializeField] private Appearance P1_Appearance;
    [SerializeField] private Appearance P2_Appearance;

    [Header("UI Elements")]
    [SerializeField] private Image flash;
    [SerializeField] private Image fade;
    [SerializeField] private GameObject gameSetting;

    [SerializeField] private GameObject characterSelect;

    private void OnEnable()
    {
        selectInput.OnL_Previous += HandleL_Previous;
        selectInput.OnL_Next += HandleL_Next;
        selectInput.OnL_Confirm += HandleL_Confirm;
        selectInput.OnL_Cancel += HandleL_Cancel;

        selectInput.OnR_Previous += HandleR_Previous;
        selectInput.OnR_Next += HandleR_Next;
        selectInput.OnR_Confirm += HandleR_Confirm;
        selectInput.OnR_Cancel += Cancel;

        flash.color = Color.white;
    }

    private void OnDisable()
    {
        selectInput.OnL_Previous -= HandleL_Previous;
        selectInput.OnL_Next -= HandleL_Next;
        selectInput.OnL_Confirm -= HandleL_Confirm;
        selectInput.OnL_Cancel -= HandleL_Cancel;

        selectInput.OnR_Previous -= HandleR_Previous;
        selectInput.OnR_Next -= HandleR_Next;
        selectInput.OnR_Confirm -= HandleR_Confirm;
        selectInput.OnR_Cancel -= Cancel;
    }

    private void HandleL_Previous(bool isholding)
    {
        if (isholding) return;
        P1_Appearance.Previous();
    }

    private void HandleL_Next(bool isholding)
    {
        if (isholding) return;
        P1_Appearance.Next();
    }

    private void HandleL_Confirm(bool isholding)
    {
        if (isholding) return;
        P1_Appearance.Confirm();
    }

    private void HandleL_Cancel(bool isholding)
    {
        if (isholding) return;
        P1_Appearance.Cancel();
    }

    private void HandleR_Previous(bool isholding)
    {
        if (isholding) return;
        P2_Appearance.Previous();
    }

    private void HandleR_Next(bool isholding)
    {
        if (isholding) return;
        P2_Appearance.Next();
    }

    private void HandleR_Confirm(bool isholding)
    {
        if (isholding) return;
        P2_Appearance.Confirm();
    }

    private void Cancel(bool isholding)
    {
        if (isholding) return;
        P2_Appearance.Cancel();
    }

    public void CheckBothReady()
    {
        if (P1_Appearance.isConfirmed && P2_Appearance.isConfirmed)
        {
            bothReady = true;
            flash.gameObject.SetActive(true);
            flash.DOFade(0, 2);

            fade.raycastTarget = true;
            fade.DOFade(1, 1).OnComplete(() =>
            {
                SoundManager.Instance.Stop("CharacterSelect"); 
                SoundManager.Instance.Play("GameSetting");

                gameSetting.SetActive(true);
                characterSelect.SetActive(false);
                fade.DOFade(0, 1).SetDelay(3).OnComplete(() =>
                {
                    fade.raycastTarget = false;
                });
            });
        }
    }
}
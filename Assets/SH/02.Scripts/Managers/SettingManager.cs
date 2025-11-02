using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    private CanvasGroup canvasGroup;
    private bool isOpen = false;

    public bool activePSX = true;
    [SerializeField] private ScriptableRendererFeature PSX;

    [Header("UI")]
    [SerializeField] private GameObject psxCheckbox;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (PSX != null)
        {
            activePSX = PSX.isActive;
            if (psxCheckbox != null) psxCheckbox.SetActive(activePSX);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (isOpen) HideSetting();
            else ShowSetting();
        }
    }

    public void TogglePSX()
    {
        if (PSX == null) return;
        activePSX = !activePSX;
        PSX.SetActive(activePSX);
        if (psxCheckbox != null) psxCheckbox.SetActive(activePSX);
    }

    public void ShowSetting()
    {
        isOpen = true;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1f, .25f);
    }

    public void HideSetting()
    {
        isOpen = false;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0f, .25f);
    }
}
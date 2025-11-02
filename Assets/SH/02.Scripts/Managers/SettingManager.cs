using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using DG.Tweening;

public class SettingManager : MonoBehaviour
{
    public static SettingManager instance;
    private CanvasGroup canvasGroup;
    private bool isOpen = false;

    public bool activePSX = true;
    private ScriptableRendererFeature PSX;

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
        }

        var urp = (UniversalRenderPipelineAsset)GraphicsSettings.currentRenderPipeline;
        var so = new SerializedObject(urp);
        var prop = so.FindProperty("m_RendererDataList");

        if (prop != null && prop.arraySize > 0)
        {
            var rendererData = prop.GetArrayElementAtIndex(0).objectReferenceValue as UniversalRendererData;
            if (rendererData == null) return;

            foreach (var feature in rendererData.rendererFeatures)
            {
                if (feature != null && feature.name == "FullScreenPassRendererFeature")
                {
                    PSX = feature;
                }
            }
        }
        activePSX = PSX.isActive;
        psxCheckbox.SetActive(activePSX);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            if (isOpen)
            {
                HideSetting();
            }else
            {
                ShowSetting();
            }
        }
    }

    public void TogglePSX()
    {
        activePSX = !activePSX;
        PSX.SetActive(activePSX);
        psxCheckbox.SetActive(activePSX);
    }

    public void ShowSetting()
    {
        isOpen = true;

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1, .25f);
    }

    public void HideSetting()
    {
        isOpen = false;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.DOFade(0, .25f);
    }
}
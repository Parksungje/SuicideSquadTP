using UnityEngine;
using DG.Tweening;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private CanvasGroup mainMenuCanvas;
    [SerializeField] private CanvasGroup WaringCanvas;
    [SerializeField] private CanvasGroup WaringTextCanvas;
    [SerializeField] private GameObject menuCamera;
    [SerializeField] private GameObject selectInputManager;
    private CanvasGroup baseCanvas;

    private void Start()
    {
        baseCanvas = GetComponent<CanvasGroup>();
    }

    public void Play()
    {
        mainMenuCanvas.interactable = false;
        mainMenuCanvas.blocksRaycasts = false;
        mainMenuCanvas.DOFade(0, 1);
        WaringCanvas.DOFade(1, 1).SetDelay(.5f).OnComplete(() =>
        {
            WaringTextCanvas.DOFade(1, 1).SetDelay(.5f);
            WaringCanvas.interactable = true;
            WaringCanvas.blocksRaycasts = true;
        });
    }

    public void WarningToNext()
    {
        WaringCanvas.interactable = false;
        WaringCanvas.blocksRaycasts = false;
        WaringTextCanvas.DOFade(0, 1).OnComplete(() =>
        {
            menuCamera.SetActive(false);
            baseCanvas.DOFade(0, .5f).SetDelay(5).OnComplete(() =>
            {
                selectInputManager.SetActive(true);
                baseCanvas.interactable = true;
                baseCanvas.blocksRaycasts = true;
            });
        });
    }

    public void Option()
    {

    }

    public void Quit()
    {
        Application.Quit();
    }
}
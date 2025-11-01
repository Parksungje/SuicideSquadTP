using TMPro;
using DG.Tweening;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    public bool isConfirmed = false;
    private int currentIndex = 1;
    private CharacterManager characterManager;

    [SerializeField] private bool isP1 = true;

    [Header("Components")]
    [SerializeField] private SkinnedMeshRenderer character;
    [SerializeField] private CanvasGroup confirmedMark;
    [SerializeField] private TextMeshProUGUI currentIndexText;
    [SerializeField] private SelectInputHandler selectInputHandler;

    private void Start()
    {
        isConfirmed = false;
        characterManager = CharacterManager.Instance;
    }

    public void Next()
    {
        if (isConfirmed) return;

        currentIndex++;
        if (currentIndex > characterManager.appearances.Count) currentIndex = 1;
        if (isP1)
        {
            characterManager.P1_Material = characterManager.appearances[currentIndex - 1];
            characterManager.P1_Mesh = characterManager.meshs[currentIndex - 1];
            character.sharedMaterial = characterManager.P1_Material;
            character.sharedMesh = characterManager.P1_Mesh;
        }
        else
        {
            characterManager.P2_Material = characterManager.appearances[currentIndex - 1];
            characterManager.P2_Mesh = characterManager.meshs[currentIndex - 1];
            character.sharedMaterial = characterManager.P2_Material;
            character.sharedMesh = characterManager.P2_Mesh;
        }
        currentIndexText.text = $"외형 {currentIndex}";
    }

    public void Previous()
    {
        if (isConfirmed) return;

        currentIndex--;
        if (currentIndex < 1) currentIndex = characterManager.appearances.Count;
        if (isP1)
        {
            characterManager.P1_Material = characterManager.appearances[currentIndex - 1];
            characterManager.P1_Mesh = characterManager.meshs[currentIndex - 1];
            character.sharedMaterial = characterManager.P1_Material;
            character.sharedMesh = characterManager.P1_Mesh;
        }
        else
        {
            characterManager.P2_Material = characterManager.appearances[currentIndex - 1];
            characterManager.P2_Mesh = characterManager.meshs[currentIndex - 1];
            character.sharedMaterial = characterManager.P2_Material;
            character.sharedMesh = characterManager.P2_Mesh;
        }
        currentIndexText.text = $"외형 {currentIndex}";
    }

    public void Confirm()
    {
        if (isConfirmed) return;

        isConfirmed = true;
        confirmedMark.DOFade(1f, 0.2f);
        confirmedMark.transform.DORotate(new Vector3(0f, 0f, 1080f), 0.3f, RotateMode.FastBeyond360).SetEase(Ease.OutExpo);
        confirmedMark.transform.localScale = Vector3.zero;
        confirmedMark.transform.DOScale(Vector3.one, 0.2f);

        selectInputHandler.CheckBothReady();
    }

    public void Cancel()
    {
        if (!isConfirmed || selectInputHandler.bothReady) return;

        isConfirmed = false;
        confirmedMark.DOFade(0f, 0.2f);
        confirmedMark.transform.localScale = Vector3.one;
        confirmedMark.transform.DOScale(Vector3.zero, 0.2f);
    }
}
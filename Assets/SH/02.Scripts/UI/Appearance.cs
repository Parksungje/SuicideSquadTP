using TMPro;
using UnityEngine;

public class Appearance : MonoBehaviour
{
    [SerializeField] private bool is1P = true;
    private int currentIndex = 1;
    private CharacterManager characterManager;

    [Header("Characters")]
    [SerializeField] private SkinnedMeshRenderer P1_Character;
    [SerializeField] private SkinnedMeshRenderer P2_Character;
    [SerializeField] private TextMeshProUGUI currentIndexText;

    private void Start()
    {
        characterManager = CharacterManager.Instance;
    }

    public void Next()
    {
        currentIndex++;
        if (currentIndex > characterManager.appearances.Count) currentIndex = 1;
        if (is1P)
        {
            characterManager.P1_Material = characterManager.appearances[currentIndex - 1];
            P1_Character.sharedMaterial = characterManager.P1_Material;
        }else
        {
            characterManager.P2_Material = characterManager.appearances[currentIndex - 1];
            P2_Character.sharedMaterial = characterManager.P2_Material;
        }
        currentIndexText.text = currentIndex.ToString();
    }

    public void Previous()
    {
        currentIndex--;
        if (currentIndex < 1) currentIndex = characterManager.appearances.Count;
        if (is1P)
        {
            characterManager.P1_Material = characterManager.appearances[currentIndex - 1];
            P1_Character.sharedMaterial = characterManager.P1_Material;
        }
        else
        {
            characterManager.P2_Material = characterManager.appearances[currentIndex - 1];
            P2_Character.sharedMaterial = characterManager.P2_Material;
        }
        currentIndexText.text = currentIndex.ToString();
    }
}
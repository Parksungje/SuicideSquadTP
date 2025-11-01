using UnityEngine;

public class CharacterImporter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private SkinnedMeshRenderer P1_Appearance;
    [SerializeField] private SkinnedMeshRenderer P2_Appearance;

    private void Start()
    {
        P1_Appearance.sharedMaterial = CharacterManager.Instance.P1_Material;
        P1_Appearance.sharedMesh = CharacterManager.Instance.P1_Mesh;
        P2_Appearance.sharedMesh = CharacterManager.Instance.P2_Mesh;
        P2_Appearance.sharedMaterial = CharacterManager.Instance.P2_Material;
    }
}
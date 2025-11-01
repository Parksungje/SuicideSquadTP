using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public Mesh P1_Mesh;
    public Material P1_Material;
    public Mesh P2_Mesh;
    public Material P2_Material;

    public List<Mesh> meshs;
    public List<Material> appearances;

    public static CharacterManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
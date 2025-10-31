using UnityEngine;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
    public Material P1_Material;
    public Material P2_Material;

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
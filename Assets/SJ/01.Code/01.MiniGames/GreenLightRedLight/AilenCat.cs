using DG.Tweening;
using UnityEngine;

public class AilenCat : MonoBehaviour
{
    [SerializeField] private GameObject ailenCat;
    [SerializeField] private GreenLightRedLight gameManager;
    [SerializeField] private float rotateDuration = 0.5f;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GreenLightRedLight>();
    }

    private void Update()
    {
        if (gameManager == null || ailenCat == null) return;

        if (gameManager.isGreenLight)
        {
            ailenCat.transform
                .DORotate(new Vector3(0, 0, 0), rotateDuration)
                .SetEase(Ease.OutQuad);
        }
        else
        {
            ailenCat.transform
                .DORotate(new Vector3(0, 180, 0), rotateDuration)
                .SetEase(Ease.OutQuad);
        }
    }
}

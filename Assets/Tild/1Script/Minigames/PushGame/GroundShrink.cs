using UnityEngine;
using DG.Tweening;

public class GroundShrink : MonoBehaviour
{
    [SerializeField] private GameObject groundObj;
    [SerializeField] private float shrinkDuration = 5f;
    [SerializeField] private Vector2 targetXZ = new Vector2(10f, 10f);
    private Vector3 currentScale;
    public void Shrink()
    {
        currentScale = groundObj.transform.localScale;
        Vector3 targetScale = new Vector3(targetXZ.x, currentScale.y, targetXZ.y);

        groundObj.transform.DOScale(targetScale, shrinkDuration)
            .SetEase(Ease.Linear);
    }

    public void Reset()
    {
       groundObj.transform.localScale = currentScale;
    }
    
}

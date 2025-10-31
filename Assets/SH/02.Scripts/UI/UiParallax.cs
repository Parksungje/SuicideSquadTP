using UnityEngine;
using UnityEngine.UI;

public class UiParallax : MonoBehaviour
{
    private RawImage _img;
    [SerializeField] private Vector2 direction;

    private void Start()
    {
        _img = GetComponent<RawImage>();
    }

    private void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + direction * Time.deltaTime, _img.uvRect.size);
    }
}

using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)]
public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;
    public static TransitionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<TransitionManager>();
                if (_instance == null)
                {
                    var go = new GameObject("TransitionManager");
                    _instance = go.AddComponent<TransitionManager>();
                }
            }
            return _instance;
        }
    }

    [SerializeField] private Image overlayImage;
    [SerializeField] private Material materialTemplate;
    [SerializeField] private float fadeDuration = 0.7f;
    [SerializeField] private Ease fadeEase = Ease.InOutQuad;

    private Material _mat;
    private bool _isTransitioning;
    private Tween _scrollTween, _alphaTween;
    private int _scrollId, _alphaId;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        EnsureOverlay();
        BuildMaterialInstance();

        DontDestroyOnLoad(gameObject);
        DontDestroyOnLoad(overlayImage.transform.root.gameObject);

        overlayImage.gameObject.SetActive(false);
        _mat.SetFloat(_scrollId, 0f);
        _mat.SetFloat(_alphaId, 0f);
    }
    private void Start()
    {
        StartCoroutine(Co_InitialFadeIn());
    }

    public static void Go(string sceneName)
    {
        Instance.TransitionToScene(sceneName);
    }

    public void TransitionToScene(string sceneName)
    {
        if (_isTransitioning) return;
        StartCoroutine(Co_Transition(sceneName));
    }

    private IEnumerator Co_Transition(string sceneName)
    {
        _isTransitioning = true;

        if (overlayImage == null)
        {
            EnsureOverlay();
            BuildMaterialInstance();
        }

        overlayImage.gameObject.SetActive(true);

        _mat.SetFloat(_scrollId, 0f);
        _mat.SetFloat(_alphaId, 0f);

        _scrollTween = DOTween.To(() => _mat.GetFloat(_scrollId), v => _mat.SetFloat(_scrollId, v), 3f, fadeDuration)
            .SetEase(fadeEase).SetUpdate(true);
        _alphaTween = DOTween.To(() => _mat.GetFloat(_alphaId), v => _mat.SetFloat(_alphaId, v), 1f, fadeDuration)
            .SetEase(fadeEase).SetUpdate(true);

        yield return _scrollTween.WaitForCompletion();
        yield return _alphaTween.WaitForCompletion();

        var op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;
        yield return null;

        if (_mat == null)
        {
            BuildMaterialInstance();
        }

        _mat.SetFloat(_scrollId, 3f);
        _mat.SetFloat(_alphaId, 1f);

        _scrollTween = DOTween.To(() => _mat.GetFloat(_scrollId), v => _mat.SetFloat(_scrollId, v), 0f, fadeDuration)
            .SetEase(fadeEase).SetUpdate(true);
        _alphaTween = DOTween.To(() => _mat.GetFloat(_alphaId), v => _mat.SetFloat(_alphaId, v), 0f, fadeDuration)
            .SetEase(fadeEase).SetUpdate(true);

        yield return _scrollTween.WaitForCompletion();
        yield return _alphaTween.WaitForCompletion();

        overlayImage.gameObject.SetActive(false);
        _isTransitioning = false;
    }

    private void EnsureOverlay()
    {
        if (overlayImage != null) return;

        var canvasGO = new GameObject("TransitionCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform, false);
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = short.MaxValue;

        var scaler = canvasGO.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        var overlayGO = new GameObject("Overlay", typeof(RectTransform), typeof(Image));
        overlayGO.transform.SetParent(canvasGO.transform, false);
        var rt = overlayGO.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        overlayImage = overlayGO.GetComponent<Image>();
        overlayImage.color = Color.white;
        overlayImage.raycastTarget = false;
    }

    private void BuildMaterialInstance()
    {
        if (materialTemplate == null)
        {
            Debug.LogWarning("No transition material assigned. Using default UI material.");
            materialTemplate = new Material(Shader.Find("UI/Default"));
        }

        _mat = Instantiate(materialTemplate);
        overlayImage.material = _mat;
        _scrollId = ResolveId(_mat, "_Scroll", "Scroll");
        _alphaId = ResolveId(_mat, "_Alpha", "Alpha");
    }

    private static int ResolveId(Material m, params string[] names)
    {
        foreach (var n in names)
            if (m.HasProperty(n))
                return Shader.PropertyToID(n);
        return 0;
    }


    private IEnumerator Co_InitialFadeIn()
    {
        if (overlayImage == null)
        {
            EnsureOverlay();
            BuildMaterialInstance();
        }

        overlayImage.gameObject.SetActive(true);
        _mat.SetFloat(_scrollId, 0f);
        _mat.SetFloat(_alphaId, 1f);

        _scrollTween?.Kill();
        _alphaTween?.Kill();

        _alphaTween = DOTween.To(() => _mat.GetFloat(_alphaId), v => _mat.SetFloat(_alphaId, v), 0f, fadeDuration)
            .SetEase(fadeEase)
            .SetUpdate(true);

        yield return _alphaTween.WaitForCompletion();
        overlayImage.gameObject.SetActive(false);
    }

}

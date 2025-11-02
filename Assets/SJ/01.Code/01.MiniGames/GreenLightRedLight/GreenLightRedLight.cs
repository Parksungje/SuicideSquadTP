using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Tild.Menu;

public class GreenLightRedLight : MonoBehaviour
{
    [SerializeField] private float minGreenTime;
    [SerializeField] private float maxGreenTime;
    [SerializeField] private float minRedTime;
    [SerializeField] private float maxRedTime;
    [SerializeField] private float checkInterval;
    [SerializeField] private float redCheckDelay;
    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private float moveThreshold;
    [SerializeField] private float tolerance;
    [SerializeField] private GRLightMovement movementManager;
    [SerializeField] private Light lightRenderer;
    [SerializeField] private Color greenColor = Color.green;
    [SerializeField] private Color redColor = Color.red;
    [SerializeField] private Vector3 zoneMin;
    [SerializeField] private Vector3 zoneMax;
    [SerializeField] private float pushBackDistance;
    [SerializeField] private float pushBackDuration;
    [SerializeField] private Ease pushBackEase = Ease.OutBack;
    [SerializeField] private float pushBackOvershoot;
    [SerializeField] private CanvasGroup finishPanel;
    [SerializeField] private TMP_Text finishText;
    [SerializeField] private Collider finishTrigger;
    [SerializeField] private float totalRoundTime;
    [SerializeField] private float flashDuration;
    [SerializeField] private float flashIntensityMul;
    [SerializeField] private bool isFinalMatch;

    public bool isGreenLight { get; private set; }

    private Vector3 startPosP1;
    private Vector3 startPosP2;
    private Vector3 lastPosP1;
    private Vector3 lastPosP2;
    private float timer;
    private float redCheckTimer;
    private bool gameEnded;
    private Quaternion startRotP1;
    private Quaternion startRotP2;
    private Transform winner;
    private float baseIntensity;
    private bool lockedP1;
    private bool lockedP2;
    private bool is1Pwin;
    private bool is2Pwin;
    private Collider colP1;
    private Collider colP2;
    private float checkTimer;

    private struct ForwardClamp
    {
        public bool active;
        public Transform t;
        public Vector3 f;
        public float startProj;
    }

    private ForwardClamp clampP1, clampP2;

    void Start()
    {
        if (player1 != null) { startPosP1 = player1.position; startRotP1 = player1.rotation; colP1 = player1.GetComponentInChildren<Collider>(); }
        if (player2 != null) { startPosP2 = player2.position; startRotP2 = player2.rotation; colP2 = player2.GetComponentInChildren<Collider>(); }
        baseIntensity = lightRenderer ? lightRenderer.intensity : 1f;
        if (finishPanel)
        {
            finishPanel.alpha = 0;
            finishPanel.interactable = false;
            finishPanel.blocksRaycasts = false;
        }
        checkTimer = Mathf.Max(0.01f, checkInterval);
        InitRound();
    }

    void Update()
    {
        if (gameEnded) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            isGreenLight = !isGreenLight;
            if (isGreenLight) SetGreenLight();
            else SetRedLight();
        }

        if (!isGreenLight)
        {
            if (redCheckTimer > 0f)
            {
                redCheckTimer -= Time.deltaTime;
            }
            else
            {
                CheckRedLightMovement();
            }
        }

        checkTimer -= Time.deltaTime;
        if (checkTimer <= 0f)
        {
            checkTimer = Mathf.Max(0.01f, checkInterval);
            DetectMovementSound();
            if (isGreenLight || redCheckTimer > 0f)
            {
                if (player1 != null) lastPosP1 = player1.position;
                if (player2 != null) lastPosP2 = player2.position;
            }
        }
    }

    void LateUpdate()
    {
        ApplyForwardClamp(ref clampP1);
        ApplyForwardClamp(ref clampP2);
    }

    private void DetectMovementSound()
    {
        if (player1 == null || player2 == null) return;
        float move1 = Vector3.Distance(player1.position, lastPosP1);
        float move2 = Vector3.Distance(player2.position, lastPosP2);
        if (isGreenLight)
        {
            if (move1 > moveThreshold) SafePlay("GreenRed_Walk");
            if (move2 > moveThreshold) SafePlay("GreenRed_Walk");
        }
    }

    private void ApplyForwardClamp(ref ForwardClamp c)
    {
        if (!c.active || c.t == null) return;
        Vector3 pos = c.t.position;
        float proj = Vector3.Dot(pos, c.f);
        if (proj > c.startProj)
        {
            float overshoot = proj - c.startProj;
            c.t.position = pos - c.f * overshoot;
        }
    }

    private bool IsInZone(Transform player)
    {
        if (player == null) return false;
        Vector3 pos = player.position;
        float xMin = Mathf.Min(zoneMin.x, zoneMax.x);
        float xMax = Mathf.Max(zoneMin.x, zoneMax.x);
        float zMin = Mathf.Min(zoneMin.z, zoneMax.z);
        float zMax = Mathf.Max(zoneMin.z, zoneMax.z);
        return pos.x >= xMin && pos.x <= xMax && pos.z >= zMin && pos.z <= zMax;
    }

    private void CheckRedLightMovement()
    {
        if (player1 != null && !lockedP1 && IsInZone(player1))
        {
            float p1Move = Vector3.Distance(player1.position, lastPosP1);
            if (p1Move > moveThreshold + tolerance)
            {
                SafePlay("GreenRed_Gun");
                StartCoroutine(PushBackRoutine(player1, 0.3f, 1));
            }
        }

        if (player2 != null && !lockedP2 && IsInZone(player2))
        {
            float p2Move = Vector3.Distance(player2.position, lastPosP2);
            if (p2Move > moveThreshold + tolerance)
            {
                SafePlay("GreenRed_Gun");
                StartCoroutine(PushBackRoutine(player2, 0.3f, 2));
            }
        }
    }

    private System.Collections.IEnumerator PushBackRoutine(Transform player, float duration, int id)
    {
        if (id == 1 && lockedP1) yield break;
        if (id == 2 && lockedP2) yield break;

        if (movementManager != null) movementManager.DisablePlayer(id);

        if (id == 1) lockedP1 = true; else lockedP2 = true;

        Vector3 forward0 = player.forward.normalized;
        Vector3 startPos = player.position;
        Vector3 targetPos = startPos + forward0 * pushBackDistance;
        targetPos.y = player.position.y;

        if (id == 1)
        {
            clampP1.active = true;
            clampP1.t = player;
            clampP1.f = forward0;
            clampP1.startProj = Vector3.Dot(startPos, forward0);
        }
        else
        {
            clampP2.active = true;
            clampP2.t = player;
            clampP2.f = forward0;
            clampP2.startProj = Vector3.Dot(startPos, forward0);
        }

        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            bool prevKinematic = rb.isKinematic;
            rb.isKinematic = true;
            rb.DOMove(targetPos, pushBackDuration).SetEase(pushBackEase, pushBackOvershoot, 0f);
            yield return new WaitForSeconds(pushBackDuration);
            rb.isKinematic = prevKinematic;
        }
        else
        {
            player.DOMove(targetPos, pushBackDuration).SetEase(pushBackEase, pushBackOvershoot, 0f);
            yield return new WaitForSeconds(pushBackDuration);
        }

        if (id == 1) clampP1.active = false; else clampP2.active = false;
    }

    private void InitRound()
    {
        isGreenLight = true;
        lockedP1 = false;
        lockedP2 = false;
        ResetPlayerState(player1);
        ResetPlayerState(player2);
        if (movementManager != null)
        {
            movementManager.EnablePlayer(1);
            movementManager.EnablePlayer(2);
        }
        if (player1 != null) lastPosP1 = player1.position;
        if (player2 != null) lastPosP2 = player2.position;
        SetGreenLight();
    }

    private void ResetPlayerState(Transform player)
    {
        if (player == null) return;
        player.DOKill();
        if (player == player1) { player.position = startPosP1; player.rotation = startRotP1; }
        else if (player == player2) { player.position = startPosP2; player.rotation = startRotP2; }
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void SetGreenLight()
    {
        timer = Random.Range(minGreenTime, maxGreenTime);
        SetLightColor(greenColor);
        if (movementManager != null)
        {
            movementManager.EnablePlayer(1);
            movementManager.EnablePlayer(2);
        }
        lockedP1 = false;
        lockedP2 = false;
        SafePlay("GreenRed_BGM");
        PlayFlashFX();
    }

    private void SetRedLight()
    {
        timer = Random.Range(minRedTime, maxRedTime);
        SetLightColor(redColor);
        redCheckTimer = redCheckDelay;
        PlayFlashFX();
    }

    private void SetLightColor(Color color)
    {
        if (lightRenderer != null) lightRenderer.color = color;
    }

    public void OnFinishTriggerEnter(Transform t)
    {
        if (gameEnded) return;
        if (t == null) return;
        Transform root = null;
        if (player1 != null && (t == player1 || t.IsChildOf(player1))) root = player1;
        else if (player2 != null && (t == player2 || t.IsChildOf(player2))) root = player2;
        else return;
        if (!IsPlayerInsideFinish(root)) return;
        winner = root;
        ShowFinishUI();
    }

    private bool IsPlayerInsideFinish(Transform p)
    {
        if (finishTrigger == null) return false;
        Collider pc = p == player1 ? colP1 : colP2;
        if (pc == null) return false;
        Bounds b = finishTrigger.bounds;
        var hits = Physics.OverlapBox(b.center, b.extents, finishTrigger.transform.rotation, ~0, QueryTriggerInteraction.Collide);
        bool found = false;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == pc) { found = true; break; }
        }
        if (!found) return false;
        return b.Intersects(pc.bounds);
    }

    private void ShowFinishUI()
    {
        gameEnded = true;
        if (movementManager != null)
        {
            movementManager.DisablePlayer(1);
            movementManager.DisablePlayer(2);
        }
        if (finishPanel)
        {
            finishPanel.alpha = 0;
            finishPanel.interactable = true;
            finishPanel.blocksRaycasts = true;
            finishPanel.DOFade(1, .5f);
        }
        if (finishText)
        {
            //string name = winner == player1 ? "Player 1" : "Player 2";
            //finishText.text = name + " WIN!";
            finishText.text = "게임 종료!";

            finishText.transform.localScale = Vector3.one * 20f;

            Sequence seq = DOTween.Sequence();
            seq.Join(finishText.transform.DOScale(1f, .25f).SetEase(Ease.OutExpo));
            seq.Join(finishText.transform.DORotate(new Vector3(0f, 0f, 1080f), 1f, RotateMode.FastBeyond360)
                .SetEase(Ease.OutCubic));
        }
        is1Pwin = winner == player1;
        is2Pwin = winner == player2;
        if (isFinalMatch) MinigameManager.instance.Finish(is1Pwin);
        else MinigameManager.instance?.Finish(is1Pwin);
    }

    private void PlayFlashFX()
    {
        if (lightRenderer)
        {
            DOTween.Kill(lightRenderer);
            float peak = baseIntensity * flashIntensityMul;
            Sequence s = DOTween.Sequence();
            s.Append(DOTween.To(() => lightRenderer.intensity, v => lightRenderer.intensity = v, peak, flashDuration * 0.5f));
            s.Append(DOTween.To(() => lightRenderer.intensity, v => lightRenderer.intensity = v, baseIntensity, flashDuration * 0.5f));
            s.SetTarget(lightRenderer);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.25f);
        Vector3 center = (zoneMin + zoneMax) / 2f;
        Vector3 size = zoneMax - zoneMin;
        Gizmos.DrawCube(center, size);
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(center, size);
    }

    private void SafePlay(string key)
    {
        var sm = SoundManager.Instance;
        if (sm != null) sm.Play(key);
    }
}

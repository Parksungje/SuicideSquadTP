using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

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

    [SerializeField] private AvoidMovementManager movementManager;
    [SerializeField] private Light lightRenderer;
    [SerializeField] private Color greenColor = Color.green;
    [SerializeField] private Color redColor = Color.red;

    [SerializeField] private Vector3 zoneMin;
    [SerializeField] private Vector3 zoneMax;

    [SerializeField] private float pushBackDistance;
    [SerializeField] private float pushBackDuration;
    [SerializeField] private Ease pushBackEase = Ease.OutBack;
    [SerializeField] private float pushBackOvershoot;

    [SerializeField] private TMP_Text timerText;
    [SerializeField] private CanvasGroup finishPanel;
    [SerializeField] private TMP_Text finishText;

    [SerializeField] private Collider finishTrigger;
    [SerializeField] private float totalRoundTime;

    [SerializeField] private float flashDuration;
    [SerializeField] private float flashIntensityMul;

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
    private float remainTime;
    private bool someoneFinished;
    private Transform winner;
    private float baseIntensity;

    private bool lockedP1;
    private bool lockedP2;

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
        startPosP1 = player1.position;
        startPosP2 = player2.position;
        startRotP1 = player1.rotation;
        startRotP2 = player2.rotation;
        remainTime = totalRoundTime;
        baseIntensity = lightRenderer ? lightRenderer.intensity : 1f;
        UpdateTimerUI();
        if (finishPanel) { finishPanel.alpha = 0; finishPanel.interactable = false; finishPanel.blocksRaycasts = false; }
        InitRound();
    }

    void Update()
    {
        if (gameEnded) return;

        remainTime -= Time.deltaTime;
        if (remainTime < 0f) remainTime = 0f;
        UpdateTimerUI();
        if (remainTime <= 0f && !gameEnded)
        {
            DecideWinnerByDistance();
            ShowFinishUI();
            return;
        }

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
                return;
            }
            CheckRedLightMovement();
        }

        if (Time.frameCount % Mathf.RoundToInt(checkInterval / Time.deltaTime) == 0)
        {
            if (isGreenLight || redCheckTimer > 0f)
            {
                lastPosP1 = player1.position;
                lastPosP2 = player2.position;
            }
        }
    }

    void LateUpdate()
    {
        ApplyForwardClamp(ref clampP1);
        ApplyForwardClamp(ref clampP2);
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
        Vector3 pos = player.position;
        float xMin = Mathf.Min(zoneMin.x, zoneMax.x);
        float xMax = Mathf.Max(zoneMin.x, zoneMax.x);
        float zMin = Mathf.Min(zoneMin.z, zoneMax.z);
        float zMax = Mathf.Max(zoneMin.z, zoneMax.z);
        return pos.x >= xMin && pos.x <= xMax && pos.z >= zMin && pos.z <= zMax;
    }

    private void CheckRedLightMovement()
    {
        if (IsInZone(player1) && !lockedP1)
        {
            float p1Move = Vector3.Distance(player1.position, lastPosP1);
            if (p1Move > moveThreshold + tolerance)
            {
                StartCoroutine(PushBackRoutine(player1, 0.3f, 1));
            }
        }

        if (IsInZone(player2) && !lockedP2)
        {
            float p2Move = Vector3.Distance(player2.position, lastPosP2);
            if (p2Move > moveThreshold + tolerance)
            {
                StartCoroutine(PushBackRoutine(player2, 0.3f, 2));
            }
        }
    }

    private System.Collections.IEnumerator PushBackRoutine(Transform player, float duration, int id)
    {
        if (id == 1 && lockedP1) yield break;
        if (id == 2 && lockedP2) yield break;

        movementManager.DisablePlayer(id);

        if (id == 1) lockedP1 = true;
        else lockedP2 = true;

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

        if (id == 1) clampP1.active = false;
        else clampP2.active = false;
    }

    private void InitRound()
    {
        isGreenLight = true;
        lockedP1 = false;
        lockedP2 = false;
        ResetPlayerState(player1);
        ResetPlayerState(player2);
        movementManager.EnablePlayer(1);
        movementManager.EnablePlayer(2);
        lastPosP1 = player1.position;
        lastPosP2 = player2.position;
        SetGreenLight();
    }

    private void ResetPlayerState(Transform player)
    {
        player.DOKill();
        player.position = player == player1 ? startPosP1 : startPosP2;
        player.rotation = player == player1 ? startRotP1 : startRotP2;

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
        movementManager.EnablePlayer(1);
        movementManager.EnablePlayer(2);
        lockedP1 = false;
        lockedP2 = false;
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

    private void UpdateTimerUI()
    {
        if (!timerText) return;
        int sec = Mathf.CeilToInt(remainTime);
        timerText.text = string.Format("{0:00}:{1:00}", sec / 60, sec % 60);
    }

    public void OnFinishTriggerEnter(Transform t)
    {
        if (gameEnded || someoneFinished) return;
        if (t != player1 && t != player2) return;
        someoneFinished = true;
        winner = t;
        ShowFinishUI();
    }

    private void DecideWinnerByDistance()
    {
        if (!finishTrigger)
        {
            winner = Vector3.Distance(player1.position, zoneMax) <= Vector3.Distance(player2.position, zoneMax) ? player1 : player2;
            return;
        }
        Vector3 p = finishTrigger.bounds.ClosestPoint(finishTrigger.transform.position);
        float d1 = Vector3.Distance(player1.position, p);
        float d2 = Vector3.Distance(player2.position, p);
        winner = d1 <= d2 ? player1 : player2;
    }

    private void ShowFinishUI()
    {
        gameEnded = true;
        movementManager.DisablePlayer(1);
        movementManager.DisablePlayer(2);
        if (finishPanel)
        {
            finishPanel.alpha = 1;
            finishPanel.interactable = true;
            finishPanel.blocksRaycasts = true;
        }
        if (finishText)
        {
            string name = winner == player1 ? "Player 1" : "Player 2";
            finishText.text = name + " WIN!";
        }
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
}



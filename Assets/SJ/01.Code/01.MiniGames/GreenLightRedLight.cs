using UnityEngine;

public class GreenLightRedLight : MonoBehaviour
{
    [SerializeField] private float minGreenTime = 2f;
    [SerializeField] private float maxGreenTime = 5f;
    [SerializeField] private float minRedTime = 1f;
    [SerializeField] private float maxRedTime = 3f;
    [SerializeField] private float checkInterval = 0.2f;

    [SerializeField] private Transform player1;
    [SerializeField] private Transform player2;
    [SerializeField] private float moveThreshold = 0.25f;

    [SerializeField] private Light lightRenderer;
    [SerializeField] private Color greenColor = Color.green;
    [SerializeField] private Color redColor = Color.red;

    private bool isGreenLight = true;
    private Vector3 lastPosP1;
    private Vector3 lastPosP2;
    private float timer;

    void Start()
    {
        lastPosP1 = player1.position;
        lastPosP2 = player2.position;
        SetGreenLight();
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            isGreenLight = !isGreenLight;

            if (isGreenLight)
                SetGreenLight();
            else
                SetRedLight();
        }

        if (!isGreenLight)
        {
            if (Vector3.Distance(player1.position, lastPosP1) > moveThreshold)
                Debug.Log("Player 1 탈락! 빨간불에서 움직였습니다!");

            if (Vector3.Distance(player2.position, lastPosP2) > moveThreshold)
                Debug.Log("Player 2 탈락! 빨간불에서 움직였습니다!");
        }

        if (Time.frameCount % Mathf.RoundToInt(checkInterval / Time.deltaTime) == 0)
        {
            lastPosP1 = player1.position;
            lastPosP2 = player2.position;
        }
    }

    private void SetGreenLight()
    {
        timer = Random.Range(minGreenTime, maxGreenTime);
        SetLightColor(greenColor);
        Debug.Log("초록불! 움직이세요!");
    }

    private void SetRedLight()
    {
        timer = Random.Range(minRedTime, maxRedTime);
        SetLightColor(redColor);
        Debug.Log("빨간불! 멈추세요!");
    }

    private void SetLightColor(Color color)
    {
        if (lightRenderer != null)
            lightRenderer.color = color;
    }
}

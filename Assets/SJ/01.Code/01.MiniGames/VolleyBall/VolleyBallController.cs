using UnityEngine;
using Code.Player;

public class VolleyBallController : MonoBehaviour
{
    [SerializeField] private VolleyBallSO volleyInput;
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform spawnBallTrm;
    [SerializeField] private GameObject leftPlayer;
    [SerializeField] private GameObject rightPlayer;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float smashPower = 10f;
    //[SerializeField] private float normalHitPower = 5f;

    private Rigidbody leftRb;
    private Rigidbody rightRb;
    private Rigidbody ballRb;

    //private bool whoIsServe = true;
    private float baseBallSpeed = 6f;

    private void Awake()
    {
        //whoIsServe = true;
        SpawnBall();

        leftRb = leftPlayer.GetComponent<Rigidbody>();
        rightRb = rightPlayer.GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        volleyInput.OnLLeftDir += HandleLLeftDir;
        volleyInput.OnLRightDir += HandleLRightDir;
        volleyInput.OnLJump += HandleLJump;
        volleyInput.OnLSpike += HandleLSpike;

        volleyInput.OnRLeftDir += HandleRLeftDir;
        volleyInput.OnRRightDir += HandleRRightDir;
        volleyInput.OnRJump += HandleRJump;
        volleyInput.OnRSpike += HandleRSpike;
    }

    private void OnDisable()
    {
        volleyInput.OnLLeftDir -= HandleLLeftDir;
        volleyInput.OnLRightDir -= HandleLRightDir;
        volleyInput.OnLJump -= HandleLJump;
        volleyInput.OnLSpike -= HandleLSpike;

        volleyInput.OnRLeftDir -= HandleRLeftDir;
        volleyInput.OnRRightDir -= HandleRRightDir;
        volleyInput.OnRJump -= HandleRJump;
        volleyInput.OnRSpike -= HandleRSpike;
    }

    private void SpawnBall()
    {
        GameObject ball = Instantiate(ballPrefab, spawnBallTrm.position, Quaternion.identity);
        ballRb = ball.GetComponent<Rigidbody>();
        ballRb.linearVelocity = Vector3.zero;

        BallCollider ballCol = ball.AddComponent<BallCollider>();
        ballCol.Init(this);
    }

    public void ResetBallSpeed(Rigidbody rb)
    {
        if (rb.linearVelocity.magnitude > baseBallSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * baseBallSpeed;
        }
    }

    private void HandleLLeftDir() => leftRb.linearVelocity = new Vector3(-moveSpeed, leftRb.linearVelocity.y, 0);
    private void HandleLRightDir() => leftRb.linearVelocity = new Vector3(moveSpeed, leftRb.linearVelocity.y, 0);
    private void HandleLJump()
    {
        if (Mathf.Abs(leftRb.linearVelocity.y) < 0.01f)
            leftRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void HandleLSpike()
    {
        if (ballRb != null)
        {
            Vector3 dir = (rightPlayer.transform.position - leftPlayer.transform.position).normalized;
            ballRb.linearVelocity = dir * smashPower + Vector3.up * 2f;
        }
    }

    private void HandleRLeftDir() => rightRb.linearVelocity = new Vector3(-moveSpeed, rightRb.linearVelocity.y, 0);
    private void HandleRRightDir() => rightRb.linearVelocity = new Vector3(moveSpeed, rightRb.linearVelocity.y, 0);
    private void HandleRJump()
    {
        if (Mathf.Abs(rightRb.linearVelocity.y) < 0.01f)
            rightRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void HandleRSpike()
    {
        if (ballRb != null)
        {
            Vector3 dir = (leftPlayer.transform.position - rightPlayer.transform.position).normalized;
            ballRb.linearVelocity = dir * smashPower + Vector3.up * 2f;
        }
    }
}

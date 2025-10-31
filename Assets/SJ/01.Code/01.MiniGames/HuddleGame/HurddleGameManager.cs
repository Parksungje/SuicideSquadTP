using System.Collections;
using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    public enum HurdleGameState { Ready, Countdown, Playing, Finished }

    public class HurdleGameManager : MonoBehaviour
    {
        [Header("Input (SO)")]
        [SerializeField] private HurddleGameSO inputSO;

        [Header("Players")]
        [SerializeField] private HurdlePlayerController player1;
        [SerializeField] private HurdlePlayerController player2;
        [SerializeField] private Transform finishLine;

        [Header("Run Settings")]
        [SerializeField] private float baseSpeed = 6f;
        [SerializeField] private float acceleration = 0.2f;
        [SerializeField] private float maxSpeed = 12f;
        [SerializeField] private float trackLength = 120f;

        [Header("Round Settings")]
        [SerializeField] private int totalRounds = 3;
        private int currentRound = 1;

        [Header("Countdown")]
        [SerializeField] private float countdownSeconds = 3f;

        [Header("UI (Optional)")]
        [SerializeField] private TMPro.TextMeshProUGUI countdownText;
        [SerializeField] private TMPro.TextMeshProUGUI winnerText;
        [SerializeField] private TMPro.TextMeshProUGUI roundText;

        public float CurrentSpeed { get; private set; }
        public HurdleGameState State { get; private set; } = HurdleGameState.Ready;

        private Vector3 _startPosFinish;

        private void Start()
        {
            player1.InitStartPosition();
            player2.InitStartPosition();

            StartCoroutine(Co_StartRound());
        }

        private void Awake()
        {
            _startPosFinish = finishLine.position;
            winnerText?.SetText("");
            countdownText?.SetText("");
            roundText?.SetText("");
        }

        private void OnEnable()
        {
            inputSO.OnWKeyDown += OnP1Jump;
            inputSO.OnUpArrowDown += OnP2Jump;
        }

        private void OnDisable()
        {
            inputSO.OnWKeyDown -= OnP1Jump;
            inputSO.OnUpArrowDown -= OnP2Jump;
        }

        private void Update()
        {
            if (State != HurdleGameState.Playing) return;

            CurrentSpeed = Mathf.Min(maxSpeed, CurrentSpeed + acceleration * Time.deltaTime);

            player1.DashForward(CurrentSpeed);
            player2.DashForward(CurrentSpeed);

            if (player1.transform.position.z >= finishLine.position.z)
                FinishRace(1);
            else if (player2.transform.position.z >= finishLine.position.z)
                FinishRace(2);
        }

        private IEnumerator Co_StartRound()
        {
            ResetRace();
            roundText?.SetText($"Round {currentRound}/{totalRounds}");
            yield return Co_CountdownThenStart();
        }

        private IEnumerator Co_CountdownThenStart()
        {
            State = HurdleGameState.Countdown;
            CurrentSpeed = 0f;

            float t = countdownSeconds;
            while (t > 0f)
            {
                countdownText?.SetText(Mathf.CeilToInt(t).ToString());
                yield return null;
                t -= Time.deltaTime;
            }

            countdownText?.SetText("GO!");
            yield return new WaitForSeconds(0.5f);
            countdownText?.SetText("");

            State = HurdleGameState.Playing;
            CurrentSpeed = baseSpeed;
            player1.EnableControl(true);
            player2.EnableControl(true);
        }

        private void FinishRace(int winner)
        {
            if (State == HurdleGameState.Finished) return;
            State = HurdleGameState.Finished;

            player1.EnableControl(false);
            player2.EnableControl(false);

            winnerText?.SetText($"Player {winner} WIN!");

            StartCoroutine(Co_NextRound());
        }

        private IEnumerator Co_NextRound()
        {
            yield return new WaitForSeconds(2f);

            if (currentRound < totalRounds)
            {
                currentRound++;

                yield return StartCoroutine(Co_StartRound());
            }
            else
            {
                roundText?.SetText("All Rounds Finished!");
                winnerText?.SetText("Game Over!");
            }
        }

        public void ResetRace()
        {
            State = HurdleGameState.Ready;
            CurrentSpeed = 0f;
            winnerText?.SetText("");

            player1.ResetToStart();
            player2.ResetToStart();

            finishLine.position = _startPosFinish;
        }


        private void OnP1Jump(bool isDown)
        {
            if (State != HurdleGameState.Playing) return;
            if (isDown) player1.TryJump();
        }

        private void OnP2Jump(bool isDown)
        {
            if (State != HurdleGameState.Playing) return;
            if (isDown) player2.TryJump();
        }
    }
}
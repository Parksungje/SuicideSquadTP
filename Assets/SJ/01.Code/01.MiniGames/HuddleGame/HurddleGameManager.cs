using System.Collections;
using UnityEngine;
using Tild.Menu;

namespace SJ.Minigames.Hurdle
{
    public enum HurdleGameState { Ready, Countdown, Playing, Finished }

    public class HurdleGameManager : MonoBehaviour
    {
        [SerializeField] private HurddleGameSO inputSO;
        [SerializeField] private HurdlePlayerController player1;
        [SerializeField] private HurdlePlayerController player2;
        [SerializeField] private Transform finishLine;
        [SerializeField] private float baseSpeed = 6f;
        [SerializeField] private float acceleration = 0.2f;
        [SerializeField] private float maxSpeed = 12f;
        [SerializeField] private int totalRounds = 3;
        private int currentRound = 1;
        [SerializeField] private float countdownSeconds = 3f;
        [SerializeField] private TMPro.TextMeshProUGUI countdownText;
        [SerializeField] private TMPro.TextMeshProUGUI winnerText;
        [SerializeField] private TMPro.TextMeshProUGUI roundText;

        public float CurrentSpeed { get; private set; }
        public HurdleGameState State { get; private set; } = HurdleGameState.Ready;

        private Vector3 _startPosFinish;
        private int _p1RoundWins;
        private int _p2RoundWins;

        private void Awake()
        {
            _startPosFinish = finishLine.position;
            if (winnerText) winnerText.SetText("");
            if (countdownText) countdownText.SetText("");
            if (roundText) { roundText.SetText(""); roundText.enabled = false; }
        }

        private void Start()
        {
            if (player1) player1.InitStartPosition();
            if (player2) player2.InitStartPosition();
            StartCoroutine(Co_StartRound());
            SafePlay("Hurdle_BGM");
        }

        private void OnEnable()
        {
            if (inputSO != null)
            {
                inputSO.OnWKeyDown += OnP1Jump;
                inputSO.OnUpArrowDown += OnP2Jump;
            }
        }

        private void OnDisable()
        {
            if (inputSO != null)
            {
                inputSO.OnWKeyDown -= OnP1Jump;
                inputSO.OnUpArrowDown -= OnP2Jump;
            }
        }

        private void Update()
        {
            if (State != HurdleGameState.Playing) return;
            CurrentSpeed = Mathf.Min(maxSpeed, CurrentSpeed + acceleration * Time.deltaTime);
            if (player1) player1.DashForward(CurrentSpeed);
            if (player2) player2.DashForward(CurrentSpeed);
            if (player1 && player1.transform.position.z >= finishLine.position.z) FinishRace(1);
            else if (player2 && player2.transform.position.z >= finishLine.position.z) FinishRace(2);
        }

        private IEnumerator Co_StartRound()
        {
            State = HurdleGameState.Ready;
            CurrentSpeed = 0f;
            if (winnerText) winnerText.SetText("");
            finishLine.position = _startPosFinish;
            if (player1) { player1.ResetToStart(); player1.EnableControl(false); }
            if (player2) { player2.ResetToStart(); player2.EnableControl(false); }
            if (roundText) { roundText.enabled = true; roundText.SetText($"Round {currentRound}/{totalRounds}"); }
            yield return new WaitForEndOfFrame();
            yield return Co_CountdownThenStart();
        }

        private IEnumerator Co_CountdownThenStart()
        {
            State = HurdleGameState.Countdown;
            CurrentSpeed = 0f;
            float t = countdownSeconds;
            while (t > 0f)
            {
                if (countdownText) countdownText.SetText(Mathf.CeilToInt(t).ToString());
                yield return null;
                t -= Time.unscaledDeltaTime;
            }
            if (countdownText) countdownText.SetText("GO!");
            yield return new WaitForSecondsRealtime(0.5f);
            if (countdownText) countdownText.SetText("");
            State = HurdleGameState.Playing;
            CurrentSpeed = baseSpeed;
            if (player1) player1.EnableControl(true);
            if (player2) player2.EnableControl(true);
        }

        private void FinishRace(int winner)
        {
            if (State == HurdleGameState.Finished) return;
            State = HurdleGameState.Finished;
            if (player1) player1.EnableControl(false);
            if (player2) player2.EnableControl(false);
            if (winner == 1) _p1RoundWins++; else _p2RoundWins++;
            if (roundText) roundText.enabled = false;
            if (winnerText) winnerText.SetText($"Player {winner} WIN!");
            StartCoroutine(Co_NextRound());
        }

        private IEnumerator Co_NextRound()
        {
            yield return new WaitForSecondsRealtime(2f);
            if (currentRound < totalRounds)
            {
                currentRound++;
                yield return StartCoroutine(Co_StartRound());
            }
            else
            {
                bool is1Pwin = _p1RoundWins > _p2RoundWins;
                MinigameManager.instance?.Finish(is1Pwin);
            }
        }

        public void ResetRace()
        {
            State = HurdleGameState.Ready;
            CurrentSpeed = 0f;
            if (winnerText) winnerText.SetText("");
        }

        private void OnP1Jump(bool isDown)
        {
            if (State != HurdleGameState.Playing) return;
            if (isDown && player1) player1.TryJump();
        }

        private void OnP2Jump(bool isDown)
        {
            if (State != HurdleGameState.Playing) return;
            if (isDown && player2) player2.TryJump();
        }

        private void SafePlay(string key)
        {
            var sm = SoundManager.Instance;
            if (sm != null) sm.Play(key);
        }
    }
}

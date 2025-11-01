using System.Collections;
using System.Collections.Generic;
using Code.Player;
using Tild.Minigames.BalanceGame;
using UnityEngine;
using DG.Tweening;
using Tild.Menu;
using Random = UnityEngine.Random;

namespace Tild._1Script.Minigames.Rope
{
    public class RopeManager : MonoBehaviour
    {
        [SerializeField] private BaseInputSO baseInputSO;
        [SerializeField] private Transform uiParent1P;
        [SerializeField] private Transform uiParent2P;
        [SerializeField] private RopePullButton ropePullButton1P;
        [SerializeField] private RopePullButton ropePullButton2P;
        [SerializeField] private Transform player1P;
        [SerializeField] private Transform player2P;
        [SerializeField] private ParticleSystem pullImpact1P;
        [SerializeField] private ParticleSystem pullImpact2P;
        [SerializeField] private ParticleSystem specialImpact1P;
        [SerializeField] private ParticleSystem specialImpact2P;
        [SerializeField] private ParticleSystem celebrateImpact1P;
        [SerializeField] private ParticleSystem celebrateImpact2P;
        [SerializeField] private Transform rope;
        
        [SerializeField] private GameObject _celebCamera1P;
        [SerializeField] private GameObject _celebCamera2P;
        [SerializeField] private GameObject _defaultCamera;

        private bool _isPlaying;
        private bool _is1PEnable;
        private bool _is1PCorrect;
        private bool _is2PEnable;
        private bool _is2PCorrect;

        private Control _current1PControl;
        private Control _current2PControl;
        private RopePullButton _current1PButton;
        private RopePullButton _current2PButton;

        private List<Control> _1PControls = new List<Control>();
        private List<Control> _2PControls = new List<Control>();

        private List<RopePullButton> _buttonQueue1P = new List<RopePullButton>();
        private List<RopePullButton> _buttonQueue2P = new List<RopePullButton>();

        private int percentage = 0;
        private WaitForSeconds failDelay = new WaitForSeconds(1.5f);
        private const int BUTTON_COUNT = 10;

        private Control _special1PControl;
        private Control _special2PControl;

        private const int SPECIAL_STREAK = 5;
        private const int SPECIAL_PULL_BONUS = 15;
        private int _streak1P;
        private int _streak2P;

        private void OnEnable()
        {
            _1PControls.AddRange(new Control[] { Control.W, Control.A, Control.S, Control.D });
            _2PControls.AddRange(new Control[] { Control.Left, Control.Up, Control.Down, Control.Right });

            baseInputSO.OnWKeyPressed = a => { if (_is1PEnable) Control1P(Control.W); };
            baseInputSO.OnAKeyPressed = a => { if (_is1PEnable) Control1P(Control.A); };
            baseInputSO.OnSKeyPressed = a => { if (_is1PEnable) Control1P(Control.S); };
            baseInputSO.OnDKeyPressed = a => { if (_is1PEnable) Control1P(Control.D); };

            baseInputSO.OnUpArrowPressed = a => { if (_is2PEnable) Control2P(Control.Up); };
            baseInputSO.OnLeftArrowPressed = a => { if (_is2PEnable) Control2P(Control.Left); };
            baseInputSO.OnRightArrowPressed = a => { if (_is2PEnable) Control2P(Control.Right); };
            baseInputSO.OnDownArrowPressed = a => { if (_is2PEnable) Control2P(Control.Down); };
        }

        private void UpdateRopePosition()
        {
            float minPercentage = -100f;
            float maxPercentage = 100f;
            float minX = -11f;
            float maxX = 8f;
            float t = (percentage - minPercentage) / (maxPercentage - maxPercentage + 200f);
            Vector3 pos = rope.localPosition;
            if (pos.x == minX || pos.x == maxX)
            {
                StartCoroutine(FinishGame());
            }
            pos.x = Mathf.Lerp(minX, maxX, (percentage - minPercentage) / (maxPercentage - minPercentage));
            rope.localPosition = pos;
        }

        IEnumerator FinishGame()
        {
            _defaultCamera.gameObject.SetActive(false);
            uiParent1P.gameObject.SetActive(false);
            uiParent2P.gameObject.SetActive(false);
            if (percentage < 0)
            {
                player2P.DOMoveY(-100, 4).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(2);
                _celebCamera1P.gameObject.SetActive(true);
                celebrateImpact1P.Play();
                yield return new WaitForSeconds(3);
                MinigameManager.instance.Finish(true);
            }
            else
            {
                player1P.DOMoveY(-100, 4).SetEase(Ease.OutBounce);
                yield return new WaitForSeconds(2);
                _celebCamera2P.gameObject.SetActive(true);
                celebrateImpact1P.Play();
                yield return new WaitForSeconds(3);
                MinigameManager.instance.Finish(false);
            }
        }

        private void Control1P(Control control)
        {
            if (control == _current1PControl)
            {
                _is1PCorrect = true;
                _streak1P++;
                percentage -= 5;
                percentage = Mathf.Clamp(percentage, -100, 100);
                if (_streak1P >= SPECIAL_STREAK)
                {
                    _streak1P = 0;
                    percentage -= SPECIAL_PULL_BONUS;
                    percentage = Mathf.Clamp(percentage, -100, 100);
                    specialImpact1P.Play();
                }
                player1P.DOPunchPosition(Vector3.zero, 0.5f, 10, 1);
                pullImpact1P.Play();
                UpdateRopePosition();
            }
            else
            {
                _streak1P = 0;
                StartCoroutine(FailDelay(true));
            }
        }

        private void Control2P(Control control)
        {
            if (control == _current2PControl)
            {
                _is2PCorrect = true;
                _streak2P++;
                player2P.DOPunchPosition(Vector3.zero, 0.5f, 10, 1);
                pullImpact2P.Play();
                percentage += 5;
                percentage = Mathf.Clamp(percentage, -100, 100);
                if (_streak2P >= SPECIAL_STREAK)
                {
                    _streak2P = 0;
                    percentage += SPECIAL_PULL_BONUS;
                    percentage = Mathf.Clamp(percentage, -100, 100);
                    specialImpact2P.Play();
                }
                UpdateRopePosition();
            }
            else
            {
                _streak2P = 0;
                StartCoroutine(FailDelay(false));
            }
        }

        private IEnumerator FailDelay(bool is1P)
        {
            if (is1P)
            {
                _is1PEnable = false;
                _current1PButton?.UnCorrect();
                yield return failDelay;
                _is1PEnable = true;
            }
            else
            {
                _is2PEnable = false;
                _current2PButton?.UnCorrect();
                yield return failDelay;
                _is2PEnable = true;
            }
        }

        private void Start()
        {
            _isPlaying = true;
            StartCoroutine(Flow1P());
            StartCoroutine(Flow2P());
        }

        private IEnumerator Flow1P()
        {
            _is1PEnable = true;

            for (int i = 0; i < BUTTON_COUNT; i++) CreateNew1PButton();

            while (_isPlaying && percentage < 100)
            {
                _current1PButton = _buttonQueue1P[0];
                _current1PControl = _current1PButton.currentControl;
                _current1PButton.gameObject.SetActive(true);
                _current1PButton.transform.SetAsLastSibling();
                _current1PButton.Scaling();

                yield return new WaitUntil(() => _is1PCorrect);
                _is1PCorrect = false;
                _current1PButton.Disappear();
                _buttonQueue1P.RemoveAt(0);

                CreateNew1PButton();

                _is1PEnable = false;
                yield return new WaitUntil(() => !baseInputSO.IsAnyKeyPressed());
                _is1PEnable = true;
            }
        }

        private IEnumerator Flow2P()
        {
            _is2PEnable = true;

            for (int i = 0; i < BUTTON_COUNT; i++) CreateNew2PButton();

            while (_isPlaying && percentage < 100)
            {
                _current2PButton = _buttonQueue2P[0];
                _current2PControl = _current2PButton.currentControl;
                _current2PButton.gameObject.SetActive(true);
                _current2PButton.transform.SetAsLastSibling();
                _current2PButton.Scaling();

                yield return new WaitUntil(() => _is2PCorrect);
                _is2PCorrect = false;
                _current2PButton.Disappear();
                _buttonQueue2P.RemoveAt(0);

                CreateNew2PButton();

                _is2PEnable = false;
                yield return new WaitUntil(() => !baseInputSO.IsAnyKeyPressed());
                _is2PEnable = true;
            }
        }

        private void CreateNew1PButton()
        {
            RopePullButton btn = Instantiate(ropePullButton1P, uiParent1P);
            btn.gameObject.SetActive(true);
            btn.transform.SetAsLastSibling();
            btn.Initialize(_1PControls[Random.Range(0, _1PControls.Count)]);
            _buttonQueue1P.Add(btn);
        }

        private void CreateNew2PButton()
        {
            RopePullButton btn = Instantiate(ropePullButton2P, uiParent2P);
            btn.gameObject.SetActive(true);
            btn.transform.SetAsLastSibling();
            btn.Initialize(_2PControls[Random.Range(0, _2PControls.Count)]);
            _buttonQueue2P.Add(btn);
        }
    }

    public enum Control
    {
        W,
        A,
        S,
        D,
        Left,
        Right,
        Down,
        Up,
    }
}

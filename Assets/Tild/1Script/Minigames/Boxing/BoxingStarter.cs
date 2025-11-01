using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tild.Minigames.Boxing
{
    public class BoxingStarter : MonoBehaviour
    {
        [SerializeField] private BoxingManager boxingManager;

        [SerializeField] private CanvasGroup keyGuideGroup;
        [SerializeField] private TMP_Text timeCountText;
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private TMP_Text score;
        [SerializeField] private Image background;
        IEnumerator Start()
        {
            SoundManager.Instance.Play("Boxing_BGM");
            keyGuideGroup.DOFade(1, 2);
            yield return new WaitForSeconds(5);

            keyGuideGroup.DOFade(0, 1);
            yield return new WaitForSeconds(1);

            infoText.DOFade(1, 0.5f);
            yield return new WaitForSeconds(5);

            infoText.DOFade(0, 0.5f);
            for (int i = 3; i > 0; i--)
            {
                timeCountText.text = i.ToString();
                yield return new WaitForSeconds(1);
            }

            timeCountText.text = "";
            boxingManager.enabled = true;
            background.DOFade(0, 0.2f);
        }
    }
}
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tild.Minigames.TagMatch
{
    public class TagStarter : MonoBehaviour
    {
        [SerializeField] private GameObject manager;

        [SerializeField] private CanvasGroup keyGuideGroup;
        [SerializeField] private TMP_Text timeCountText;
        [SerializeField] private TMP_Text infoText;
        [SerializeField] private TMP_Text score;
        [SerializeField] private Image background;
        IEnumerator Start()
        {
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
            SoundManager.Instance.Play("Tag_BGM");
            timeCountText.text = "";
            manager.SetActive(true);
            background.DOFade(0, 0.2f);
        }
    }
}
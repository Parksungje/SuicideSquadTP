using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Tild.Menu
{
    public class ChestCinematic : MonoBehaviour
    {
        #region Cinematic Players

        [SerializeField] PlayableDirector c1,c2,c3,c4;
        
        #endregion
        
        
        public void CommonCinematic()
        {
            StartCoroutine(CommonCinematicCoroutine());
        }

        IEnumerator CommonCinematicCoroutine()
        {
            yield return new WaitForSeconds(1f);
        }
    }
}
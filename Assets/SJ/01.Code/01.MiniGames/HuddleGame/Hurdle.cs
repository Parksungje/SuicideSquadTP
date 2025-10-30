using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    [RequireComponent(typeof(Collider))]
    public class Hurdle : MonoBehaviour
    {
        [SerializeField] private Transform playersRoot;
        [SerializeField] private float destroyBehind = 15f;

        private Transform _p1, _p2;

        void Start()
        {
            gameObject.tag = "Hurdle";
            var col = GetComponent<Collider>();
            col.isTrigger = false;

            if (playersRoot == null)
            {
                var players = FindObjectsByType<SJ.Minigames.Hurdle.HurdlePlayerController>(FindObjectsSortMode.None);
                if (players.Length >= 2)
                {
                    _p1 = players[0].transform;
                    _p2 = players[1].transform;
                }
            }
        }

        void Update()
        {
            if (_p1 == null || _p2 == null) return;
            float minZ = Mathf.Min(_p1.position.z, _p2.position.z);
            if (transform.position.z < minZ - destroyBehind)
                Destroy(gameObject);
        }
    }
}

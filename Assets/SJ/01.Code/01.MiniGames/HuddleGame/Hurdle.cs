using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    public class Hurdle : MonoBehaviour
    {
        [SerializeField] private float destroyBehind = 15f;

        private Transform _p1, _p2;

        void Start()
        {
            gameObject.tag = "Hurdle";
            var col = GetComponent<Collider>();
            col.isTrigger = true;
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

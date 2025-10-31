using System.Collections.Generic;
using UnityEngine;

namespace SJ.Minigames.Hurdle
{
    public class HurdleSpawnerStable : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private HurdleGameManager gameManager;
        [SerializeField] private Transform player1;
        [SerializeField] private Transform player2;
        [SerializeField] private Transform finishLine;

        [Header("Prefab & Size")]
        [SerializeField] private GameObject hurdlePrefab;
        [SerializeField] private Vector3 hurdleScale = new Vector3(1.3f, 1.3f, 1.3f);
        [SerializeField] private float laneOffsetX = 1.6f;

        [Header("Distribution (Z gap)")]
        [SerializeField] private float baseGap = 25f;
        [SerializeField] private float firstGap = 80f;
        [SerializeField] private float gapJitter = 0.0f;

        [Header("Track")]
        [SerializeField] private float edgeMargin = 2f;
        [SerializeField] private float maxZ = 90f;

        [Header("Debug")]
        [SerializeField] private bool regenerateOnPlay = true;
        [SerializeField] private bool drawGizmos = true;

        private readonly List<GameObject> _spawned = new();

        private void OnEnable()
        {
            if (regenerateOnPlay)
                GenerateAll();
        }

        public void GenerateAll()
        {
            for (int i = _spawned.Count - 1; i >= 0; --i)
                if (_spawned[i]) Destroy(_spawned[i]);
            _spawned.Clear();

            float startZ = Mathf.Min(player1.position.z, player2.position.z);
            float usableEnd = Mathf.Min(finishLine.position.z - edgeMargin, maxZ);
            var seqZ = BuildZSequence(startZ + firstGap, usableEnd, baseGap);

            foreach (float z in seqZ)
            {
                Spawn(new Vector3(-laneOffsetX, 0f, z));
                Spawn(new Vector3(+laneOffsetX, 0f, z));
            }
        }

        List<float> BuildZSequence(float zStart, float zEnd, float gap)
        {
            var zs = new List<float>();
            float z = zStart;
            while (z < zEnd)
            {
                zs.Add(z);
                z += gap;
            }
            return zs;
        }

        void Spawn(Vector3 pos)
        {
            var go = Instantiate(hurdlePrefab, pos, Quaternion.identity);
            go.transform.localScale = hurdleScale;

            var col = go.GetComponent<Collider>();
            col.isTrigger = true;
            if (col == null)
                col = go.AddComponent<BoxCollider>();

            var rb = go.GetComponent<Rigidbody>();
            if (rb == null)
                rb = go.AddComponent<Rigidbody>();

            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            _spawned.Add(go);
        }


        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos || gameManager == null || finishLine == null) return;
            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawLine(new Vector3(-laneOffsetX, 0f, player1 ? player1.position.z : 0f),
                            new Vector3(-laneOffsetX, 0f, finishLine.position.z));
            Gizmos.DrawLine(new Vector3(+laneOffsetX, 0f, player2 ? player2.position.z : 0f),
                            new Vector3(+laneOffsetX, 0f, finishLine.position.z));
        }
    }
}

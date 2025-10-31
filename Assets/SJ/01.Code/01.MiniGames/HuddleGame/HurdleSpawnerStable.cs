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

        [Header("Mode")]
        [SerializeField] private bool useCountMode = true;

        [Header("Count Mode")]
        [SerializeField] private int hurdlesPerLane = 14;
        [SerializeField] private float firstGapCountMode = 50f;
        [SerializeField] private float edgeMargin = 2f;

        [Header("Gap Mode")]
        [SerializeField] private float baseGap = 25f;
        [SerializeField] private float firstGap = 80f;
        [SerializeField] private float gapJitter = 0.0f;
        [SerializeField] private float densityMultiplier = 1.0f;

        [Header("Track Limit")]
        [SerializeField] private float maxZ = 90f;

        [Header("Debug")]
        [SerializeField] private bool regenerateOnPlay = true;
        [SerializeField] private bool drawGizmos = true;
        [SerializeField] private int randomSeed = 0;

        private readonly List<GameObject> _spawned = new();

        void OnEnable()
        {
            if (regenerateOnPlay)
                GenerateAll();
        }

        [ContextMenu("Generate All")]
        public void GenerateAll()
        {
            for (int i = _spawned.Count - 1; i >= 0; --i)
                if (_spawned[i]) Destroy(_spawned[i]);
            _spawned.Clear();

            if (randomSeed != 0) Random.InitState(randomSeed);

            float startZ = Mathf.Min(player1 ? player1.position.z : 0f,
                                     player2 ? player2.position.z : 0f);

            float usableEnd = Mathf.Min((finishLine ? finishLine.position.z : maxZ) - edgeMargin, maxZ);

            List<float> seqZ = useCountMode
                ? BuildZSequence_ByCount(startZ + firstGapCountMode, usableEnd, hurdlesPerLane)
                : BuildZSequence_ByGap(startZ + firstGap, usableEnd, baseGap, gapJitter, densityMultiplier);

            foreach (float z in seqZ)
            {
                Spawn(new Vector3(-laneOffsetX, 0f, z));
                Spawn(new Vector3(+laneOffsetX, 0f, z));
            }
        }

        List<float> BuildZSequence_ByCount(float zStart, float zEnd, int count)
        {
            var zs = new List<float>();
            if (count <= 0 || zEnd <= zStart) return zs;

            float length = zEnd - zStart;
            float step = length / count;
            float z = zStart;
            for (int i = 0; i < count; i++)
            {
                zs.Add(z);
                z += step;
            }
            return zs;
        }

        List<float> BuildZSequence_ByGap(float zStart, float zEnd, float gap, float jitter, float densityMul)
        {
            var zs = new List<float>();
            if (zEnd <= zStart || gap <= 0f) return zs;

            float effectiveGap = Mathf.Max(1f, gap / Mathf.Max(0.1f, densityMul));
            float z = zStart;
            while (z < zEnd)
            {
                zs.Add(z);
                float j = (jitter > 0f) ? Random.Range(-jitter, jitter) : 0f;
                z += Mathf.Max(1f, effectiveGap + j);
            }
            return zs;
        }

        void Spawn(Vector3 pos)
        {
            var go = Instantiate(hurdlePrefab, pos, Quaternion.identity);
            go.transform.localScale = hurdleScale;

            var col = go.GetComponent<Collider>();
            if (col == null) col = go.AddComponent<BoxCollider>();
            col.isTrigger = true;

            var rb = go.GetComponent<Rigidbody>();
            if (rb == null) rb = go.AddComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.useGravity = false;
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

            _spawned.Add(go);
        }

        void OnDrawGizmosSelected()
        {
            if (!drawGizmos || finishLine == null) return;
            float p1z = player1 ? player1.position.z : 0f;
            float p2z = player2 ? player2.position.z : 0f;

            Gizmos.color = new Color(1f, 1f, 1f, 0.25f);
            Gizmos.DrawLine(new Vector3(-laneOffsetX, 0f, p1z), new Vector3(-laneOffsetX, 0f, finishLine.position.z));
            Gizmos.DrawLine(new Vector3(+laneOffsetX, 0f, p2z), new Vector3(+laneOffsetX, 0f, finishLine.position.z));
        }
    }
}

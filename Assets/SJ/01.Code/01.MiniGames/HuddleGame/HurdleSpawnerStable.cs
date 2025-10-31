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

        [Header("Spawn Config")]
        [SerializeField] private GameObject hurdlePrefab;
        [SerializeField] private float laneOffsetX = 1.5f;
        [SerializeField] private float spawnAhead = 25f;
        [SerializeField] private float minGap = 7f;
        [SerializeField] private float maxGap = 13f;
        [SerializeField] private float laneJitterZ = 0.8f;

        [Header("Cleanup")]
        [SerializeField] private float destroyBehind = 12f;

        private float _nextZP1;
        private float _nextZP2;

        private void OnEnable()
        {
            ResetSpawner();
        }

        public void ResetSpawner()
        {
            _nextZP1 = player1.position.z + 6f;
            _nextZP2 = player2.position.z + 6f;
        }

        private void Update()
        {
            if (gameManager == null || gameManager.State != HurdleGameState.Playing) return;

            float finishZ = finishLine.position.z;

            float targetZ1 = Mathf.Min(player1.position.z + spawnAhead, finishZ - 2f);
            while (_nextZP1 < targetZ1)
            {
                float z = _nextZP1 + Random.Range(minGap, maxGap);
                if (z >= finishZ - 1.5f) break;
                SpawnHurdle(new Vector3(-laneOffsetX, 0f, z + Random.Range(-laneJitterZ, laneJitterZ)));
                _nextZP1 = z;
            }

            float targetZ2 = Mathf.Min(player2.position.z + spawnAhead, finishZ - 2f);
            while (_nextZP2 < targetZ2)
            {
                float z = _nextZP2 + Random.Range(minGap, maxGap);
                if (z >= finishZ - 1.5f) break;
                SpawnHurdle(new Vector3(+laneOffsetX, 0f, z + Random.Range(-laneJitterZ, laneJitterZ)));
                _nextZP2 = z;
            }

            float minZ = Mathf.Min(player1.position.z, player2.position.z) - destroyBehind;
        }

        private void SpawnHurdle(Vector3 pos)
        {
            GameObject obj = Instantiate(hurdlePrefab, pos, Quaternion.identity);
        }
    }
}

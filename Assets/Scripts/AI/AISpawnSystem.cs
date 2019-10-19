#pragma warning disable CS0649
using System.Collections;
using UnityEngine;

public class AISpawnSystem : MonoBehaviour {
    [SerializeField] private CharacterMovement player;
    [SerializeField] private AIPoolSystem aiPoolSystem = null;
    [SerializeField] private GameObject[] levels;
    [SerializeField] private AILevelData levelData = new AILevelData();
    public AILevelData GetLevelData => levelData;
    private Room[] rooms;

    private void Start() {
        if (aiPoolSystem == null)
            aiPoolSystem = GetComponent<AIPoolSystem>();

        StartLevel();
    }

    public void StartLevel() {
        rooms = Instantiate(levels[Random.Range(0, levels.Length)]).GetComponentsInChildren<Room>();
        int roomNo = Random.Range(0, rooms.Length);
        levelData.target = Instantiate(player, rooms[roomNo].SpawnPoints[Random.Range(0, rooms[roomNo].SpawnPoints.Length)], Quaternion.identity).transform;
        StartWave();
    }

    public void StartWave() {
        AIBatch[] batches = levelData.waves[Random.Range(0, levelData.waves.Length)].aiBatches;
        for (int i = 0; i < batches.Length; i++) {
            StartCoroutine(SpawnBatch(batches[i]));
        }
    }

    private IEnumerator SpawnBatch(AIBatch batch) {
        yield return new WaitForSeconds(batch.delay);
        //Debug.Log(batch.name + " has started!");
        AISpawn[] spawns = batch.aiSpawns;
        for (int i = 0; i < spawns.Length; i++) {
            StartCoroutine(SpawnAI(spawns[i]));
        }
    }

    private IEnumerator SpawnAI(AISpawn spawn) {
        yield return new WaitForSeconds(spawn.delay);
        StartCoroutine(SpawnInterval(spawn));
    }

    private IEnumerator SpawnInterval(AISpawn spawn) {
        int currentSpawn = 0;
        int roomNo = 0;
        Vector3 randomPos;
        while (currentSpawn < spawn.count) {
            AI ai = aiPoolSystem.GetAIPool(spawn.ai);
            roomNo = Random.Range(0, rooms.Length);
            randomPos = rooms[roomNo].SpawnPoints[Random.Range(0, rooms[roomNo].SpawnPoints.Length)];
            ai.Spawn(randomPos, levelData.target);
            ai.gameObject.SetActive(true);
            currentSpawn++;
            //Debug.Log(ai.name + " has spawned!");
            yield return new WaitForSeconds(spawn.interval);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 0f, 1f));
    }
}

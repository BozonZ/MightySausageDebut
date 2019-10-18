using System;
using System.Collections;
using UnityEngine;

public class AISpawnSystem : MonoBehaviour
{
    [SerializeField] private AIPoolSystem aiPoolSystem = null;
    [SerializeField] private AILevelData levelData = new AILevelData();
    public AILevelData GetLevelData => levelData;
    private int waveNo = 0;
    private int currAI;

    public Vector3 SpawnPoint {
        get {
            Vector3 p;
            p.x = UnityEngine.Random.Range(-0.5f, 0.5f);
            p.y = 0f;
            p.z = UnityEngine.Random.Range(-0.5f, 0.5f);
            return transform.TransformPoint(p);
        }
    }

    private void Start() {
        if (aiPoolSystem == null)
            aiPoolSystem = GetComponent<AIPoolSystem>();

        SetupLevel();
        StartWave();
    }

    private void CheckCount() {
        currAI--;
        if(currAI < 5) {
            StartWave();
        }
    }

    public void SetupLevel() {
        waveNo = -1;
    }

    public void StartWave() {
        waveNo++;
        AIBatch[] batches = levelData.waves[waveNo].aiBatches;
        for (int i = 0; i < batches.Length; i++) {
            StartCoroutine(SpawnBatch(batches[i]));
        }
    }

    private IEnumerator SpawnBatch(AIBatch batch) {
        yield return new WaitForSeconds(batch.delay);
        //Debug.Log(batch.name + " has started!");
        AISpawn[] spawns = batch.aiSpawns;
        for(int i = 0; i < spawns.Length; i++) {
            StartCoroutine(SpawnAI(spawns[i]));
        }
    }

    private IEnumerator SpawnAI(AISpawn spawn) {
        yield return new WaitForSeconds(spawn.delay);
        StartCoroutine(SpawnInterval(spawn));
    }

    private IEnumerator SpawnInterval(AISpawn spawn) {
        int currentSpawn = 0;
        while (currentSpawn < spawn.count) {
            AI ai = aiPoolSystem.GetAIPool(spawn.ai);
            ai.OnDeathEvent = CheckCount;
            ai.Spawn(SpawnPoint, levelData.target);
            //Debug.Log(ai.name + " has spawned!");
            currAI++;
            currentSpawn++;
            yield return new WaitForSeconds(spawn.interval);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(1f, 0f, 1f));
    }
}

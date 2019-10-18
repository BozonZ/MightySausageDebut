using UnityEngine;

[CreateAssetMenu(fileName = "AI", menuName = "AI/Create new AI", order = 1)]
public class AIData : ScriptableObject
{
    public new string name;
    public int health;
    public float armor;
    public float acceleration;
    public float moveSpeed;
    public float rotateSpeed;
}

[System.Serializable]
public struct AILevelData {
    public Transform target;
    public Wave[] waves;
}

[System.Serializable]
public struct AISpawn {
#if UNITY_EDITOR
    [AIType, Tooltip("AI to spawn.")]
#endif
    public int ai;
    [Tooltip("Amount of AI to spawn.")]
    public int count;
    [Tooltip("Spawn delay in batch")]
    public float delay;
    [Tooltip("Spawn interval in batch")]
    public float interval;
}

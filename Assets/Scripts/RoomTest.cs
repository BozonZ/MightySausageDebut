#pragma warning disable CS0649
using UnityEngine;

public class RoomTest : MonoBehaviour
{
        [SerializeField] private Vector3 offset;
        [SerializeField] private Vector3 size;

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position + offset, size);
    }
}

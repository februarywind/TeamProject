using UnityEngine;

public class TeleportTile : MonoBehaviour
{
    [SerializeField] Vector3 _teleportPos;

    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.CompareTag("Player")) return;
        if (CubeMove.Instance.IsRolling) return;
        other.transform.position = _teleportPos;
    }
}

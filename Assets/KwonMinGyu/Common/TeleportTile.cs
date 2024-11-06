using UnityEngine;

public class TeleportTile : MonoBehaviour
{
    [SerializeField] Vector3 _teleportPos;

    private void OnTriggerStay(Collider other)
    {
        if (!other.transform.CompareTag("Player")) return;
        if (CubeMove.Instance.IsRolling) return;
        // 순간이동 효과음
        AudioManager.Instance.PlaySfx(AudioManager.Sfx.TeleportationTile);
        other.transform.position = _teleportPos;
        CubeChecker.Instance.RePosition();
    }
}

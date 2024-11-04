using UnityEngine;

public class GetStamps : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("CubeStamp"))
        {
            AudioManager.Instance.PlaySfx(AudioManager.Sfx.GetStamp);
        }
    }
}

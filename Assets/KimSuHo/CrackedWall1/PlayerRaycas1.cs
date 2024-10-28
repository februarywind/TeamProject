using UnityEngine;

public class PlayerRaycas1 : MonoBehaviour
{
    public GameObject cube; // 합쳐진 큐브 오브젝트

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            cube.GetComponent<CrackedWall1>().Explode();
        }
    }
}
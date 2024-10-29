using UnityEngine;

enum WASD
{
    Up, Dwon, Right, Left
}
public class CreateYellowWall : MonoBehaviour
{
    [SerializeField] WASD[] wasd;
    [SerializeField] GameObject WallFrefab;

    private void Awake()
    {
        foreach (var item in wasd)
        {
            switch (item)
            {
                case WASD.Up:
                    Instantiate(WallFrefab, transform.position + new Vector3(0, 1, 1), transform.rotation, transform);
                    break;
                case WASD.Dwon:
                    Instantiate(WallFrefab, transform.position + new Vector3(0, 1, -1), transform.rotation, transform);
                    break;
                case WASD.Right:
                    Instantiate(WallFrefab, transform.position + new Vector3(1, 1, 0), transform.rotation, transform);
                    break;
                case WASD.Left:
                    Instantiate(WallFrefab, transform.position + new Vector3(-1, 1, 0), transform.rotation, transform);
                    break;
            }
        }
    }
}

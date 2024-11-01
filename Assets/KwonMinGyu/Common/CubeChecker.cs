using UnityEngine;

public class CubeChecker : MonoBehaviour
{
    // CubeChecker의 사용 빈도가 높아짐에 따라 static으로 변경함
    public static CubeChecker Instance;

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CubeMove move;
    [SerializeField] private StampType _activeStamp;
    [SerializeField] private StampType.Type _activeStampType;
    [SerializeField] private BlueMover _blue;
    [SerializeField] private YellowMover _yellow;
    [SerializeField] private RedStamp _red;

    // 능력 사용을 제한하는 트리거
    public bool IsStampUse { get; private set; } = true;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        _blue = GetComponent<BlueMover>();
        _yellow = GetComponent<YellowMover>();
        _red = GetComponent<RedStamp>();
    }
    public bool IsGround()
    {
        RaycastHit hit;
        bool _IsGround = Physics.Raycast(transform.position, -transform.up, out hit, 1f, layerMask);
        //Debug.Log(hit.transform);
        // Debug.Log(Vector3.Angle(Vector3.up, hit.normal)); // 경사로 각도 확인
        return _IsGround;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        _activeStamp = other.GetComponent<StampType>();
        if (_activeStamp == null) return;
        _activeStampType = _activeStamp.GetStampType;
        switch (_activeStampType)
        {
            case StampType.Type.None:
                ActiveNone();
                break;
            case StampType.Type.Blue:
                ActiveNone();
                _blue.enabled = true;
                break;
            case StampType.Type.Yellow:
                ActiveNone();
                _yellow.enabled = true;
                break;
            case StampType.Type.Red:
                ActiveNone();
                _red.enabled = true;
                break;
        }
    }
    private void ActiveNone()
    {
        if (_blue) _blue.enabled = false;
        if (_yellow) _yellow.enabled = false;
        if (_red) _red.enabled = false;
    }

    public void RePosition(Vector3 Cubeposition)
    {
        transform.position = Cubeposition + Vector3.up * 0.4f;
    }

    public void StampControll(bool possibility)
    {
        IsStampUse = possibility;
    }
}

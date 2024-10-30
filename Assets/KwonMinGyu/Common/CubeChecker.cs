using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeChecker : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CubeMove move;
    [SerializeField] private StampType _activeStamp;
    [SerializeField] private StampType.Type _activeStampType;
    [SerializeField] private BlueMover _blue;
    [SerializeField] private YellowMover _yellow;
    private void Start()
    {
        _blue = GetComponent<BlueMover>();
        _yellow = GetComponent<YellowMover>();
    }
    public bool IsGround()
    {
        RaycastHit hit;
        bool _IsGround = Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, layerMask);
        //Debug.Log(hit.transform);
        // Debug.Log(Vector3.Angle(Vector3.up, hit.normal)); // 경사로 각도 확인
        return _IsGround;
    }
    private void OnTriggerEnter(Collider other)
    {
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
        }
    }
    private void ActiveNone()
    {
        _blue.enabled = false;
        _yellow.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StampType2;

public class CubeChecker : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private CubeMove4 move;
    [SerializeField] private StampType2 _activeStamp;
    [SerializeField] private StampType2.StampType _activeStampType;
    [SerializeField] private BlueMover3 _blue;
    [SerializeField] private YellowMover3 _yellow;
    private void Start()
    {
        _blue = GetComponent<BlueMover3>();
        _yellow = GetComponent<YellowMover3>();
    }
    public bool IsGround()
    {
        RaycastHit hit;
        bool _IsGround = Physics.Raycast(transform.position, -transform.up, out hit, 1.5f, layerMask);
        // Debug.Log(Vector3.Angle(Vector3.up, hit.normal)); // 경사로 각도 확인
        return _IsGround;
    }
    private void OnTriggerEnter(Collider other)
    {
        _activeStamp = other.GetComponent<StampType2>();
        if (_activeStamp == null) return;
        _activeStampType = _activeStamp.GetStampType;
        switch (_activeStampType) 
        { 
            case StampType2.StampType.None:
                ActiveNone();
                break;
            case StampType2.StampType.Blue:
                ActiveNone();
                _blue.enabled = true; 
                break;
            case StampType2.StampType.Yellow:
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

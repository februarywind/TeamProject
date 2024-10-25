using UnityEngine;
public enum CameraPos
{
    Up, Down, Right, Left
}
public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _cubeTransform;  // Cube 오브젝트
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -8);  // 카메라 기본 위치
    [SerializeField] private float _mouseSensitivity = 200f;  // 마우스 민감도
    [SerializeField] private float _rotationUpLimit = 50f;  // 카메라가 회전할 수 있는 최대 각도 (상하 제한)
    [SerializeField] private float _rotationDownLimit = -20f;  // 카메라가 회전할 수 있는 최대 각도 (상하 제한)

    private float _pitch = 0f;  // 위아래 각도 (X축 회전)
    private float _yaw = 0f;  // 좌우 각도 (Y축 회전)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // 매 프레임마다 호출되는 카메라 업데이트
    void LateUpdate()
    {
        // 마우스 입력 받기
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        // 마우스 입력을 기준으로 카메라의 각도 조정
        _yaw += mouseX;
        _pitch -= mouseY;

        // 위아래 각도 제한 (큐브를 넘어가지 않도록)
        _pitch = Mathf.Clamp(_pitch, _rotationDownLimit, _rotationUpLimit);

        // 카메라의 회전 적용 (큐브를 중심으로 회전) 
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        // 카메라가 큐브를 중심으로 회전하도록 위치 계산
        transform.position = _cubeTransform.position + rotation * _offset;  // Quaternion * position = postion의 각도 변경
        // 카메라가 항상 큐브를 바라보도록 설정
        transform.LookAt(_cubeTransform.position);
    }

    // 카메라가 큐브를 기준으로 어느 방향에 있는지 구하는 함수
    public CameraPos CameraPosition()
    {
        CameraPos _cameraPos = new CameraPos();
        // 큐브를 기준점으로 한 카메라의 x좌표
        float tempX = transform.position.x - _cubeTransform.position.x;
        // 카메라의 첫 y좌표를 기준으로 이동한 y좌표
        float tempY = transform.position.y - (_offset.y + _cubeTransform.position.y);

        // 카메라의 좌표값이 대략 +-_offset.z - 2 일때 방향 전환
        // y축이 변할 때 x축 값이 변하기 때문에 tempY로 보강
        if (tempX + tempY > -_offset.z - 2)
            _cameraPos = CameraPos.Right;
        else if (tempX - tempY < _offset.z + 2)
            _cameraPos = CameraPos.Left;
        // 카메라의 위치가 양 옆이 아니고 z좌표값이 양수라면 카메라는 큐브의 윗 방향에 있음
        else if (transform.position.z - _cubeTransform.position.z > 0)
            _cameraPos = CameraPos.Up;
        else
            _cameraPos = CameraPos.Down;
        //Debug.Log(_cameraPos);
        return _cameraPos;
    }
}

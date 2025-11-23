using UnityEngine;

public class RotationEnemy : MonoBehaviour
{
    [Header("회전 설정")]
    public float rotationSpeed = 180.0f;

    void Start()
    {
        // 초기화 로직 없음
    }

    void Update()
    {
        // Z축 중심으로 일정한 속도로 회전
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

}
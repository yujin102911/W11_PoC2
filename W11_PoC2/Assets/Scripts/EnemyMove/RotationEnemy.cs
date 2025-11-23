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

    // 2D 트리거 충돌 감지 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 대상이 "Player" 태그인지 확인
        if (other.gameObject.CompareTag("Player"))
        {
            // 데미지 로그 출력
            Debug.Log("플레이어에게 데미지를 줍니다: " + other.gameObject.name);
        }
    }
}
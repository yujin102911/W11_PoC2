using UnityEngine;

public class HorizontalEnemy : MonoBehaviour
{
    [Header("이동 설정")]
    public Vector3 pointStart;
    public Vector3 pointEnd;
    public float speed = 2.0f;

    [Header("회전 설정")]
    public bool snapToPathAngle = false;

    private Vector3 targetPoint;
    private bool movingToStart = false;

    void Start()
    {
        // 초기 위치와 목표 지점 설정
        transform.position = pointStart;
        targetPoint = pointEnd;

        // 경로 스냅 옵션이 켜져 있으면 초기 회전 적용
        if (snapToPathAngle)
        {
            Vector3 initialDirection = pointEnd - pointStart;
            float initialAngle = Mathf.Atan2(initialDirection.y, initialDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, initialAngle));
        }
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector3 moveDirection = targetPoint - transform.position;

        // 목표 지점으로 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, step);

        // 2D 회전 로직
        if (snapToPathAngle)
        {
            if (moveDirection.sqrMagnitude > 0)
            {
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
        }

        // 목표 지점 도착 시 반전 (왕복 로직)
        if (Vector3.Distance(transform.position, targetPoint) < 0.001f)
        {
            if (movingToStart)
            {
                targetPoint = pointEnd;
                movingToStart = false;
            }
            else
            {
                targetPoint = pointStart;
                movingToStart = true;
            }
        }
    }

    // 2D 트리거 충돌 감지 및 데미지 로직
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 데미지를 줍니다: " + other.gameObject.name);
        }
    }

    // Scene 뷰에 이동 경로 시각화 (기즈모)
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointStart, pointEnd);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(pointStart, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(pointEnd, 0.1f);
    }
}
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerShooter : MonoBehaviour
{
    [Header("ID")]
    public int playerID = 0;

    private Rigidbody2D rb;
    private LineRenderer line;

    [Header("Direction")]
    public GameObject Direction;

    [Header("Angle Bounce Settings")]
    public float aimLength = 2f;     // 막대기 길이
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float rotateSpeed = 60f;  // 1초에 회전하는 속도

    private bool isAiming = false;   // 각도 조절 중
    private bool angleIncreasing = true; // 각도 왔다갔다
    private float currentAngle = 0f;

    [Header("Trajectory")]
    public int simulationSteps = 30;    // 궤적 길이
    public float timeStep = 0.05f;      // 1스텝의 물리 deltaTime
    public LayerMask collisionMask;     // 벽, 장애물
    public float Maxlength = 0;

    [Header("Shooting Settings")]
    public float powerMultiplier = 5f;
    public float maxPower = 12f;

    public float stopVelocity = 0.08f;
    public float stopDrag = 3f;

    private bool isCollisioned = false;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    void Start()
    {
        // StartAngleAim();
    }


    

    void Update()
    {
        AutoStop();

        if (isAiming)
            UpdateAimLine();
        //UpdateAimAngle();

        // 스페이스로 확정 발사
        if (isAiming && Input.GetKeyDown(KeyCode.Space))
        {
            ConfirmAngleShoot(currentAngle);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("충돌했어요~");

        if (collision.gameObject.CompareTag("Node"))
        {
            Debug.Log("사라질게요~");
            Destroy(this.gameObject);
        }

        isCollisioned = true;
        Invoke(nameof(ApplyStopDrag), 0.1f);
    }

    // 궤적 그리기(나중에 필요해지면 사용)
    public void ShowTrajectory(Vector2 startPos, Vector2 dragVector)
    {
        if (line == null) return;

        line.positionCount = simulationSteps;

        // 출발 속도 계산
        Vector2 dir = -dragVector.normalized;
        float power = Mathf.Min(dragVector.magnitude * powerMultiplier, maxPower);
        Vector2 velocity = dir * power;  // weight 없다면 제거

        Vector2 pos = startPos;

        for (int i = 0; i < simulationSteps; i++)
        {
            line.SetPosition(i, pos);

            // 다음 위치 예측
            Vector2 nextPos = pos + velocity * timeStep;

            // 충돌 체크(벽, 장애물)
            RaycastHit2D hit = Physics2D.Raycast(pos, velocity.normalized, velocity.magnitude * timeStep, collisionMask);
            if (hit.collider != null)
            {
                // 충돌 지점 표시
                line.SetPosition(i, hit.point);

                // 반사 처리
                velocity = Vector2.Reflect(velocity, hit.normal);

                // pos 업데이트
                pos = hit.point;

                continue;
            }

            // === 정확한 Drag 감속 적용 ===
            float dampingFactor = 1f / (1f + stopDrag * timeStep);
            velocity *= dampingFactor;

            if (velocity.magnitude < stopVelocity)
            {
                velocity = Vector2.zero;
            }

            pos = nextPos;
        }
    }

    // 공기 저항
    private void ApplyStopDrag()
    {
        rb.linearDamping = stopDrag;
    }

    // 각도 반복 로직 함수
    private void UpdateAimAngle()
    {
        // 좌↔우 반복
        if (angleIncreasing)
            currentAngle += rotateSpeed * Time.deltaTime;
        else
            currentAngle -= rotateSpeed * Time.deltaTime;

        // 범위 체크 후 방향 반전
        if (currentAngle >= maxAngle)
            angleIncreasing = false;
        else if (currentAngle <= minAngle)
            angleIncreasing = true;

        // 실제 오브젝트 회전 적용
        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    private void UpdateAimLine()
    {
        // 각도 증가/감소
        if (angleIncreasing)
            currentAngle += rotateSpeed * Time.deltaTime;
        else
            currentAngle -= rotateSpeed * Time.deltaTime;

        // 범위 도달 시 반전
        if (currentAngle >= maxAngle)
            angleIncreasing = false;
        else if (currentAngle <= minAngle)
            angleIncreasing = true;

        // currentAngle 기준으로 방향 벡터 계산
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        // 라인 위치 업데이트
        Vector2 start = transform.position;
        Vector2 end = start + dir * aimLength;

        line.SetPosition(0, start);
        line.SetPosition(1, end);
    }


    // 조준 시작 함수 (드래그 대신 각도 모드로)
    public void StartAngleAim()
    {
        isAiming = true;
        angleIncreasing = true;
        currentAngle = minAngle;

        //Direction.SetActive(true);
        line.enabled = true;
        line.positionCount = 2;
    }

    // 스페이스 누르면 각도 확정 후 발사
    public void ConfirmAngleShoot(float angle)
    {
        isAiming = false;
        line.enabled = false;

        // currentAngle → 방향 벡터
        float rad = angle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        float power = maxPower;

        rb.linearDamping = 0;
        rb.AddForce(dir * power, ForceMode2D.Impulse);

        Invoke(nameof(ApplyStopDrag), 0.1f);
    }

    // 감속 및 정지
    private void AutoStop()
    {
        if (rb.linearVelocity.magnitude < stopVelocity)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}




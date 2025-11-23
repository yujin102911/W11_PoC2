using UnityEngine;

public class Command_node : MonoBehaviour
{
    [Header("ID")]
    public int Node_ID = 0;

    private Rigidbody2D rb;
    private LineRenderer line;

    [Header("Player_Prefab")]
    [SerializeField]
    private GameObject _player;

    [Header("Angle Bounce Settings")]
    public float aimLength = 2f;     // 막대기 길이
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float rotateSpeed = 60f;  // 1초에 회전하는 속도
    public float spawnDistance = 1.5f;

    public bool isAiming = false;   // 각도 조절 중
    private bool angleIncreasing = true; // 각도 왔다갔다
    private float currentAngle = 0f;

    [Header("Shooting Settings")]
    public float powerMultiplier = 5f;
    public float maxPower = 12f;

    public float stopVelocity = 0.08f;
    public float stopDrag = 3f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("스택 올려요~");
            StartAngleAim();
        }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Update()
    {
        if (isAiming)
            UpdateAimLine();
        //UpdateAimAngle();

        // 스페이스로 확정 발사
        if (isAiming && Input.GetKeyDown(KeyCode.Space))
        {
            ShootPlayer();
            
        }
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

    // 스페이스 누르면 각도 확정 후 발사
    private void ShootPlayer()
    {
        isAiming = false;
        line.enabled = false;

        // currentAngle 기준으로 방향 벡터 계산
        float rad = currentAngle * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        Vector2 spawnPos = (Vector2)this.transform.position + dir * spawnDistance;

        GameObject player = Instantiate(_player, spawnPos, Quaternion.identity);
        player.GetComponent<PlayerShooter>().ConfirmAngleShoot(currentAngle);
    }
}

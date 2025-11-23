using UnityEngine;

public class RevolutionEnemy : MonoBehaviour
{
    [Header("공전 설정 (Revolution Settings)")]
    public float revolutionRadius = 2.0f;
    public float revolutionSpeed = 90.0f;

    [Header("자식 오브젝트 설정 (Child Object Settings)")]
    public GameObject revolvingChild;

    private float currentAngle;

    void Start()
    {
        currentAngle = 0f;

        if (revolvingChild == null)
        {
            Debug.LogError("자식 오브젝트를 할당해 주세요.");
            enabled = false;
            return;
        }

        // 초기 위치 설정 및 자식 관계 설정
        Vector3 initialPosition = transform.position + new Vector3(revolutionRadius, 0, 0);
        revolvingChild.transform.position = initialPosition;
        revolvingChild.transform.SetParent(this.transform);
    }

    void Update()
    {
        // 현재 각도 업데이트 (라디안)
        currentAngle += revolutionSpeed * Time.deltaTime * Mathf.Deg2Rad;

        // 원형 공전 궤도의 X, Y 위치 계산 (삼각함수 사용)
        float x = Mathf.Cos(currentAngle) * revolutionRadius;
        float y = Mathf.Sin(currentAngle) * revolutionRadius;

        // 자식 오브젝트의 로컬 위치를 설정하여 공전 운동 구현
        revolvingChild.transform.localPosition = new Vector3(x, y, 0);
    }

    // --- 기즈모 로직 ---

    private void OnDrawGizmos()
    {
        // 공전 반경을 원형 기즈모로 시각화
        if (revolutionRadius > 0)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, revolutionRadius);
        }

        // 자식 오브젝트의 위치를 점으로 표시
        if (revolvingChild != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(revolvingChild.transform.position, 0.1f);
        }
    }
}
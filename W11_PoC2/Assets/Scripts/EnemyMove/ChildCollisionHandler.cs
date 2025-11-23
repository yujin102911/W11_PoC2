// (자식 오브젝트에 부착될 스크립트)
using UnityEngine;

public class ChildCollisionHandler : MonoBehaviour
{
    // 2D 트리거 충돌 감지 및 데미지 로직
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("플레이어에게 데미지를 줍니다: " + other.gameObject.name);
        }
    }
}
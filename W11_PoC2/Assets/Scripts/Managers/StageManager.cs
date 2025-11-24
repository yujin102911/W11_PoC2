using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    [Header("프리팹")]
    public GameObject playerPrefab;

    [Header("현재 스테이지 상태")]
    public int currentPlayerHP;
    public int currentEnemyHP;

    private GameObject currentPlayer;
    private StageData currentStage;


    #region UnityLifecycle
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    #endregion

    #region Initialization
    /// <summary>
    /// 스테이지 시작 전 초기화(준비)하는 함수
    /// </summary>
    public void InitializeStage(StageData stage)
    {
        currentStage = stage;

        Debug.Log($"StageManager: Stage {stage.stageNumber} 초기화 시작");

        currentEnemyHP = stage.EnemyMaxHP;
        currentPlayerHP = stage.PlayerMaxHP;

        SpawnPlayer();
    }
    #endregion

    /// <summary>
    /// 플레이어의 프리팹을 생성하는 함수
    /// </summary>
    private void SpawnPlayer()
    {
        if (playerPrefab != null)
        {
            Vector3 pos = currentStage.spawnPoint;
            currentPlayer = Instantiate(playerPrefab, pos, Quaternion.identity);

            var shooter = currentPlayer.GetComponent<PlayerShooter>();
            shooter.StartAngleAim();
        }
    }

    /// <summary>
    /// 플레이어 죽었을 때 목숨 보고 게임오버 or 플레이어 리스폰 결정
    /// </summary>
    public void OnPlayerDied()
    {
        currentPlayerHP--;
        CommandStackManager.Instance.ResetStack();
        if (currentPlayerHP <= 0) // 만약 플레이어의 라이프가 남아있지 않다면
        {
            Debug.Log("플레이어 라이프 0 => 게임 오버");
            GameManager.Instance.OnGameOver();
            // 추후 GameOver UI 등 추가
            return;
        }
        // 남아 있다면
        SpawnPlayer();
    }
    
    public void OnEnemyAttack(int damage)
    {
        currentEnemyHP -= damage;
        Debug.Log($"적 남은 체력: {currentEnemyHP}");
    }

    /// <summary>
    /// 적 체력 다 까면 호출
    /// </summary>
    public void OnEnemyDied()
    {
        Debug.Log("적 체력 0 미만 => 스테이지 클리어");
        GameManager.Instance.OnGameOver();
        // 추후 다음 스테이지 이동 등 구현
        return;
    }

}

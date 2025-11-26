using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("모든 스테이지 데이터 SO")]
    public StageData[] stages;

    public int currentStageIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartStage(currentStageIndex);
    }

    /// <summary>
    /// 특정 스테이지 시작
    /// </summary>
    public void StartStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stages.Length)
        {
            Debug.LogError("스테이지 번호 범위 밖");
            return;
        }
        currentStageIndex = stageIndex;
        Debug.Log($"===== 스테이지 {stageIndex} 시작 =====");

        StageManager stageManager = FindAnyObjectByType<StageManager>();
        if (stageManager == null)
        {
            Debug.LogError("스테이지 매니저가 씬에 없음");
            return;
        }
        stageManager.InitializeStage(stages[stageIndex]);
    }

    /// <summary>
    /// StageManager가 호출 할 게임 클리어 함수
    /// </summary>
    public void OnStageClear()
    {
        Debug.Log("스테이지 클리어");
        currentStageIndex++;
        if (currentStageIndex >= stages.Length)
        {
            Debug.Log("모든 스테이지 클리어! 게임 끗");
            // 추후 엔딩 처리
            return;
        }
        StartStage(currentStageIndex);
    }

    /// <summary>
    /// StageManager가 호출 할 게임 오버 함수
    /// </summary>
    public void OnGameOver()
    {
        Debug.Log("게임 오버");
        Time.timeScale = 0.0f;
        // 추후 게임 오버 처리
        return;
    }


}

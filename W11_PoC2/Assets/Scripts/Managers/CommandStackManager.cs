using UnityEngine;
using System.Collections.Generic;

public class CommandStackManager : MonoBehaviour
{
    public static CommandStackManager Instance;

    [Header("스택 유지 시간")]
    public float maxDuration = 5f;
    private float timer = 0f;

    [Header("현재 누적된 커맨드 문자열")]
    public string stackSequence = "";

    [Header("DB - 정답 패턴 목록")]
    public List<string> validPatterns = new List<string>();

    private bool timerActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        if (!timerActive) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ResetStack();
        }
    }
    /// <summary>
    /// 노드가 충돌을 알려줄 때 호출
    /// </summary>
    public void PushCommand(string nodeID)
    {
        // 타이머 리셋
        timer = maxDuration;
        timerActive = true;

        // 스택 문자 누적
        stackSequence += nodeID;
        Debug.Log("현재 스택: " + stackSequence);

        // 패턴 확인
        CheckPatternMatch();
    }
    /// <summary>
    /// DB 패턴 검사
    /// </summary>
    private void CheckPatternMatch()
    {
        foreach (var pattern in validPatterns)
        {
            if (stackSequence == pattern)
            {
                Debug.Log($"패턴 {pattern} 완성");
                // TODO: 원하는 효과 실행
                ResetStack();
                return;
            }
        }
    }
    public void ResetStack()
    {
        Debug.Log("스택 초기화!");
        stackSequence = "";
        timerActive = false;
    }

}

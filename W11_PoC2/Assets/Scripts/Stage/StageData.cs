using UnityEngine;

/// <summary>
/// 매 스테이지 마다의 적 HP, 내 HP, 리스폰 장소를 저장하고 있을 SO
/// </summary>
[CreateAssetMenu(fileName = "StageData", menuName = "Game/Stage Data")]
public class StageData : ScriptableObject
{
    [Tooltip("스테이지 번호")]
    public int stageNumber;

    [Tooltip("적의 최대 체력")]
    public int EnemyMaxHP = 30; // 적의 최대 체력
    [Tooltip("플레이어의 최대 체력")]
    public int PlayerMaxHP = 3; // 플레이어의 최대 체력

    [Tooltip("플레이어가 리스폰될 지역")]
    public Vector3 spawnPoint = Vector3.zero; // 플레이어가 죽음 트리거 됐을 때 리스폰 될 지역


}

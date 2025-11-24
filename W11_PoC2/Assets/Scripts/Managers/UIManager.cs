using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("HP_UI")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;

    private List<GameObject> heartList = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHearts(int currentHP)
    {
        // 기존 하트 초기화
        foreach (var heart in heartList)
            Destroy(heart);

        heartList.Clear();

        // 새로 그리기
        for (int i = 0; i < currentHP; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            heartList.Add(newHeart);
        }
    }
}

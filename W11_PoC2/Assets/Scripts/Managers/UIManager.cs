using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("PlayerHP_UI")]
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;

    [Header("EnemyHP_UI")]
    [SerializeField] private Slider HP_Slider;
    [SerializeField] private TextMeshProUGUI _hpRateTxt;

    [Header("Node_UI")]
    [SerializeField] private GameObject[] nodePrefab;
    [SerializeField] private Transform nodeContainer;

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
    public void UpdateEnemy(int currentHP, int max)
    {
        HP_Slider.value = (float)currentHP / (float)max;
        _hpRateTxt.text = $"{currentHP} / {max}";
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


    public void UpdateNode(string stackSequence, float duration)
    {
        ClearNode();

        foreach (var id in stackSequence)
        {
            switch (id)
            {
                case 'R':
                    GameObject R_node = Instantiate(nodePrefab[0], nodeContainer);
                    R_node.GetComponent<node_UI>().StartFillReduce(duration);
                    break;

                case 'G':
                    GameObject G_node = Instantiate(nodePrefab[1], nodeContainer);
                    G_node.GetComponent<node_UI>().StartFillReduce(duration);
                    break;

                case 'B':
                    GameObject B_node = Instantiate(nodePrefab[2], nodeContainer);
                    B_node.GetComponent<node_UI>().StartFillReduce(duration);
                    break;

                case 'Y':
                    GameObject Y_node = Instantiate(nodePrefab[3], nodeContainer);
                    Y_node.GetComponent<node_UI>().StartFillReduce(duration);
                    break;
            }
            
        }
    }

    public void ClearNode()
    {
        for (int i = nodeContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(nodeContainer.GetChild(i).gameObject);
        }
    }
}

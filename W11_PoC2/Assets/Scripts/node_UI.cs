using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class node_UI : MonoBehaviour
{
    [SerializeField] private Image fillImage;

    public void StartFillReduce(float duration)
    {
        if (duration == 0f) return;

        StartCoroutine(FillReduceCoroutine(duration));
    }

    private IEnumerator FillReduceCoroutine(float duration)
    {
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // t = 0~1 비율
            float t = timer / duration;

            // 1 -> 0 으로 감소
            fillImage.fillAmount = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        // 안전하게 0으로 보정
        fillImage.fillAmount = 0f;
    }
}

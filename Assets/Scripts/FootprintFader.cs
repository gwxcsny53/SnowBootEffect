using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class FootprintFader : MonoBehaviour
{

    [Tooltip("脚印可见的持续时间")]
    public float fadeDuration = 3f;

    private Renderer rend;
    private Color originalColor;
    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    private IObjectPool<FootprintFader> objectPool;

    public void SetPool(IObjectPool<FootprintFader> pool)
    {
        objectPool = pool;
    }

    void Awake()
    {
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.GetColor(BaseColorID);

        }
    }

    void OnEnable()
    {
        if (rend != null)
        {
            Color resetColor = originalColor;
            rend.material.SetColor(BaseColorID, resetColor);
            StartCoroutine(FadeOutCoroutine());
        }
    }


    private IEnumerator FadeOutCoroutine()
    {

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            float currentAlpha = Mathf.Lerp(1f, 0f, t);// 线性过渡
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, currentAlpha);
            // rend.material.color = newColor;

            rend.material.SetColor(BaseColorID, newColor);


            // 挂起携程 等待下一帧继续执行
            yield return null;
        }

        if (objectPool != null)
        {
            objectPool.Release(this);
        }
    }


}

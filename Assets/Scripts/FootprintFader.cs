using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.Rendering.Universal;

public class FootprintFader : MonoBehaviour
{

    [Tooltip("脚印可见的持续时间")]
    public float fadeDuration = 3f;

    private DecalProjector decalProjector;
    private Color originalColor;
    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");

    /** 缓存贴图属性的ID  在URP中主贴图叫 _BaseMap */
    private static readonly int BaseMapID = Shader.PropertyToID("_BaseMap");

    private IObjectPool<FootprintFader> objectPool;

    public void SetPool(IObjectPool<FootprintFader> pool)
    {
        objectPool = pool;
    }

    public void SetPootprintType(Boolean isLeftFoot)
    {
        if (decalProjector != null)
        {
            float offsetX = isLeftFoot ? 0f : 0.5f;
            // 使用SetTextureOffset 专门修改贴图的偏移值
            decalProjector.material.SetTextureOffset(BaseMapID, new Vector2(offsetX, 0f));
        }
    }

    void Awake()
    {
        decalProjector = GetComponent<DecalProjector>();
        if (decalProjector != null)
        {
            originalColor = decalProjector.material.GetColor(BaseColorID);
        }


    }

    void OnEnable()
    {
        if (decalProjector != null)
        {
            Color resetColor = originalColor;
            decalProjector.material.SetColor(BaseColorID, resetColor);
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

            decalProjector.material.SetColor(BaseColorID, newColor);


            // 挂起携程 等待下一帧继续执行
            yield return null;
        }

        if (objectPool != null)
        {
            objectPool.Release(this);
        }
    }


}

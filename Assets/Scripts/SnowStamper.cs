using UnityEngine;

public class SnowStamper : MonoBehaviour
{
    [Header("引用设置")]
    [Tooltip("Render texture")]
    public RenderTexture snowRT;

    [Tooltip("笔刷贴图")]
    public Texture2D brushTexture;

    [Header("雪地参数")]
    [Tooltip("Plane 中心点的世界坐标")]
    public Vector3 planeCenter = Vector3.zero;

    public float planeSize = 10f;

    public float brushSize = 0.5f;
    //用于处理图片透明通道叠加的隐藏材质
    private Material blendMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RenderTexture.active = snowRT;
        GL.Clear(false, true, Color.black);
        RenderTexture.active = null;
        // 、、、
        blendMaterial = new Material(Shader.Find("Custom/SnowStamp"));
    }

    public void StampFootprint(Vector3 worldPos)
    {
        if (snowRT == null || blendMaterial == null) return;
        float uvX = (worldPos.x - planeCenter.x) / planeSize + 0.5f;
        float uvZ = (worldPos.z - planeCenter.z) / planeSize + 0.5f;

        if (uvX < 0.0f || uvX > 1.0f || uvZ < 0.0f || uvZ > 1.0f) return;


        RenderTexture tempRT = RenderTexture.GetTemporary(snowRT.width, snowRT.height, 0, snowRT.graphicsFormat);
        Graphics.Blit(snowRT, tempRT);

        blendMaterial.SetTexture("_PrevTex", tempRT);
        blendMaterial.SetTexture("_MainTex", brushTexture);
        blendMaterial.SetVector("_StampCenter", new Vector4(uvX, 1.0f - uvZ, 0, 0));
        blendMaterial.SetFloat("_StampSize", brushSize / planeSize);
        blendMaterial.SetFloat("_Strength", 1.0f);

        Graphics.Blit(tempRT, snowRT, blendMaterial);

        RenderTexture.ReleaseTemporary(tempRT);



    }
}

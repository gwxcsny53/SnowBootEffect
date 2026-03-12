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

        // 加载unity 内置 UI Shader ，他自带完美的透明混合模式
        blendMaterial = new Material(Shader.Find("UI/Default"));
    }

    // Update is called once per frame
    void Update()
    {
        // StampFootprint(transform.position);
    }


    public void StampFootprint(Vector3 worldPos)
    {
        if (snowRT == null || blendMaterial == null) return;
        float uvX = (worldPos.x - planeCenter.x) / planeSize + 0.5f;
        float uvZ = (worldPos.z - planeCenter.z) / planeSize + 0.5f;

        if (uvX < 0.0f || uvX > 1.0f || uvZ < 0.0f || uvZ > 1.0f) return;

        // 算出再 RT 上的位置
        float pixelX = uvX * snowRT.width;
        float pixelY = uvZ * snowRT.height;

        // 算出笔刷像素大小
        float pixelSize = (brushSize / planeSize) * snowRT.width;

        RenderTexture.active = snowRT;
        GL.PushMatrix();

        // untiy GL  Y 轴是从下往上画的，需要将Y反转
        GL.LoadPixelMatrix(0, snowRT.width, snowRT.height, 0);

        // 计算笔刷图片绘制的矩形范围
        Rect drawRect = new Rect(
         (snowRT.width - pixelX) - pixelSize * 0.5f,
            pixelY - pixelSize * 0.5f,
            pixelSize,
            pixelSize
            );

        // 盖章
        Graphics.DrawTexture(drawRect, brushTexture, blendMaterial);
        GL.PopMatrix();
        RenderTexture.active = null;

    }
}

using System;
using UnityEngine;

public class FootprintMaker : MonoBehaviour
{
    public GameObject footprintPrefab;
    public float footprintLifetime = 10f;
    public float stepInterval = 0.5f;
    public float rayDistence = 1.5f;

    public LayerMask groundLayer;

    private float stepTimer = 0f;

    private SelfCharacterController characterController;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        characterController = GetComponent<SelfCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // 简单的移动检测
        if (characterController.IsMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                TryPlaceFootprint();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    private void TryPlaceFootprint()
    {
        // 从角色脚下向下发射射线检测地面
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistence, groundLayer))
        {
            // 在地面位置生成足印，朝向角色前进方向
            // 修正后的旋转逻辑：让物体的 Z 轴对齐地面法线的反方向（向下），Y 轴对齐角色前方
            Quaternion footprintRotation = Quaternion.LookRotation(-hit.normal, transform.forward);

            GameObject footprint = Instantiate(footprintPrefab, hit.point + hit.normal * 0.01f, footprintRotation);
            Destroy(footprint, footprintLifetime);
        }
    }
}

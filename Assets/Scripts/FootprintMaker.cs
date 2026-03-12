using System;
using UnityEngine;
using UnityEngine.UIElements;
public class FootprintMaker : MonoBehaviour
{
    public float footprintLifetime = 10f;
    public float stepInterval = 0.5f;
    public float rayDistence = 0.4f;

    public LayerMask groundLayer;
    [Tooltip("脚印中心到左右脚的偏移距离")]
    public float footSpacing = 0.2f;

    [Header("脚步音效设置")]
    [Tooltip("audio source 组件")]
    public AudioSource footstepAudioSource;

    [Tooltip("声音文件")]
    public AudioClip[] footstepClips;
    [Tooltip("音高的随机下限")]
    [Range(0.8f, 1.2f)] public float minPitch = 0.9f;

    [Tooltip("音高的随机下限")]
    [Range(0.8f, 1.2f)] public float maxPitch = 1.1f;


    public SnowStamper snowStamper;

    /** 脚印管理器 具有对象池*/
    private FootprintManger footprintManger;

    private float stepTimer = 0f;

    private SelfCharacterController characterController;

    private bool isNextFootLeft = true;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        characterController = GetComponent<SelfCharacterController>();

        footprintManger = GetComponent<FootprintManger>();
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

        Vector3 footprintOffset = isNextFootLeft ? -transform.right : transform.right;
        // 从角色脚下向下发射射线检测地面
        Vector3 rayOrigin = transform.position + Vector3.up * 0.1f + footprintOffset * this.footSpacing;
        Ray ray = new Ray(rayOrigin, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, rayDistence, groundLayer))
        {


            Debug.Log("bbbbb");
            /** 使用decal的逻辑
           
                // 在地面位置生成足印，朝向角色前进方向
                // 修正后的旋转逻辑：让物体的 Z 轴对齐地面法线的反方向（向下），Y 轴对齐角色前方
                Quaternion footprintRotation = Quaternion.LookRotation(-hit.normal, transform.forward);
                // Vector3 footprintPositon = new Vector3(hit.point + hit.normal * 0.01f);
                footprintManger.SpawnFootprint(hit.point + hit.normal * 0.01f, footprintRotation);
                // GameObject footprint = Instantiate(footprintPrefab, hit.point + hit.normal * 0.01f, footprintRotation);
                // Destroy(footprint, footprintLifetime);

                isNextFootLeft = !isNextFootLeft;
     */

            // 使用 graph 
            if (snowStamper)
            {
                Debug.Log("sss");
                snowStamper.StampFootprint(transform.position);
            }
            PlayFootstepSound();
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepAudioSource == null || footstepClips == null || footstepClips.Length == 0)
        {
            return;
        }

        AudioClip clipToPlay = footstepClips[UnityEngine.Random.Range(0, footstepClips.Length)];

        footstepAudioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);

        float randomVolume = UnityEngine.Random.Range(0.8f, 1.0f);

        footstepAudioSource.PlayOneShot(clipToPlay, randomVolume);
    }
}

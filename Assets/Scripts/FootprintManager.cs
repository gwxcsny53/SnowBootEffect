using UnityEngine;
using UnityEngine.Pool;

public class FootprintManger : MonoBehaviour
{

    public FootprintFader footprintPrefab;

    private ObjectPool<FootprintFader> footprintPool;
    void Start()
    {
        footprintPool = new ObjectPool<FootprintFader>(
            createFunc: createFootprint,
            actionOnGet: OnTakeFromPool,
            actionOnRelease: OnReturnedToPool,
            actionOnDestroy: OnDestroyPoolObject,
            collectionCheck: true,
            defaultCapacity: 10,
            maxSize: 50
        );
    }

    public void SpawnFootprint(Vector3 position, Quaternion rotation)
    {
        FootprintFader footprint = footprintPool.Get();

        // footprint.gameObject.transform
        footprint.transform.position = position;
        footprint.transform.rotation = rotation;
    }

    private FootprintFader createFootprint()
    {
        FootprintFader footprint = Instantiate(footprintPrefab);
        footprint.SetPool(footprintPool);
        return footprint;
    }

    private void OnTakeFromPool(FootprintFader footprintFader)
    {
        footprintFader.gameObject.SetActive(true);
    }

    private void OnReturnedToPool(FootprintFader footprintFader)
    {
        footprintFader.gameObject.SetActive(false);
    }

    private void OnDestroyPoolObject(FootprintFader footprintFader)
    {
        Destroy(footprintFader.gameObject);
    }

}
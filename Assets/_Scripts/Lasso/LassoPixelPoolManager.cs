using System.Collections.Generic;
using UnityEngine;

public class LassoPixelPoolManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _pixelPrefab;

    private int _poolSize = 100;
    private List<GameObject> pool;

    private void Awake()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject obj = Instantiate(_pixelPrefab);
            obj.transform.SetParent(transform);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetPooledPixel()
    {
        GameObject obj = pool.Find(p => !p.activeInHierarchy);

        if(obj != null)
        {
            return obj;
        }

        return CreateNewPooledPixel();
    }

    private GameObject CreateNewPooledPixel()
    {
        GameObject newObj = Instantiate(_pixelPrefab);
        newObj.transform.SetParent(transform);
        newObj.SetActive(false);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnPooledPixel(GameObject obj)
    {
        obj.SetActive(false);
    }
}

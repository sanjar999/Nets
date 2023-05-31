using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private Queue<GameObject> _queue;
    private Vector3 _scale;

    public Pool(GameObject[] prefab, int maxSize, Transform parent)
    {
        _queue = new Queue<GameObject>();
        for (int i = 0; i < maxSize; i++)
        {
            var randomPrefab = prefab[Random.Range(0, prefab.Length)];
            var item = GameObject.Instantiate(randomPrefab, parent);
            item.SetActive(false);
            _queue.Enqueue(item);
        }
    }

    public GameObject SpawnItem()
    {
        var item = _queue.Dequeue();
        item.SetActive(true);
        _queue.Enqueue(item);
        return item;
    }
}

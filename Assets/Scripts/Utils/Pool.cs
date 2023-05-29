using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    private Queue<GameObject> _queue;
    private Vector3 _scale;

    public Pool(GameObject prefab, int maxSize, Transform parent)
    {
        _scale = prefab.transform.localScale;
        _queue = new Queue<GameObject>();
        for (int i = 0; i < maxSize; i++)
        {
            var item = GameObject.Instantiate(prefab, parent);
            item.SetActive(false);
            _queue.Enqueue(item);
        }
    }

    public GameObject SpawnItem()
    {
        var item = _queue.Dequeue();
        item.SetActive(true);
        item.transform.localScale = _scale;
        _queue.Enqueue(item);
        return item;
    }
}

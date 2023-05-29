using UnityEngine;
using Random = UnityEngine.Random;

public class NetGenerator : MonoBehaviour
{
    [SerializeField] private GameObject _net;
    [SerializeField] private Transform _player;
    [SerializeField] private int _prefabsCount = 5;
    [SerializeField] private float _spawnMaxHeight = 10;
    [SerializeField] private float _netSpawnStep = 5f;

    private Pool _netPool;
    private int _currentObstacleIndex;
    private float _lastObstacleHeight;

    private void Start()
    {
        _netPool = new Pool(_net, _prefabsCount, transform);
        _lastObstacleHeight = _net.transform.position.y;
        SpawnNet();
    }

    private void Update()
    {
        if (CheckForSpawnNet())
            SpawnNet();
    }

    private void SpawnNet()
    {
        var playerHeight = _player.position.y;
        var spawnLimit = playerHeight + _spawnMaxHeight;

        while (_lastObstacleHeight < spawnLimit)
        {
            float range = Random.Range(6, 10);
            var pos = _lastObstacleHeight + range;
            var instance = _netPool.SpawnItem();
            instance.transform.position = new Vector3(0, pos, 0.426f);
            _lastObstacleHeight = pos;
            _currentObstacleIndex++;
        }
    }

    private bool CheckForSpawnNet()
    {
        if (_player.position.y > _currentObstacleIndex * _netSpawnStep)
            return true;

        return false;
    }
}
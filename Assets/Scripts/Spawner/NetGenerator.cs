using UnityEngine;
using Random = UnityEngine.Random;

public class NetGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _nets;
    [SerializeField] private Transform _player;
    [SerializeField] private int _prefabsCount = 5;
    [SerializeField] private float _spawnMaxHeight = 10;
    [SerializeField] private float _netSpawnStep = 5f;
    private Pool _netPool;
    private int _currentObstacleIndex;
    private float _lastObstacleHeight = 2f;

    private void Start()
    {
        _netPool = new Pool(_nets, _prefabsCount, transform);
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
            float range = Random.Range(2f, 4f);
            float rangeX = Random.Range(-2f, 2f);
            var pos = _lastObstacleHeight + range;
            var instance = _netPool.SpawnItem();
            pos += instance.transform.localScale.y;
            instance.transform.position = new Vector3(rangeX, pos, 0.426f);
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
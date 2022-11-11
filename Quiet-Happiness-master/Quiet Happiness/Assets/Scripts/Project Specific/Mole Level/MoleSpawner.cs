using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MoleSpawner : MonoBehaviour
{
    [SerializeField] private Transform _minPos;
    [SerializeField] private Transform _pointA;
    [SerializeField] private Transform _pointB;
    [SerializeField] private List<MoleSpawnerInfo> _objsToSpawn;
    private List<MoleSpawnerInfo> _toSpawn;
    private float _totalProbability;

    [SerializeField] [Range(0,1)] private float _delayDifference;

    private int _spawnCount;
    [SerializeField] private float _delayBetweenMoles;
    [SerializeField] private float _timed;

    private Vector3 _smallest;
    private Vector3 _biggest;

    private float _timer;
    private float _timerAll;

    private bool _menuOpened;

    private MoleMinigameManager _manager;

    private void Awake()
    {
        _spawnCount = (int) (_timed * 1/_delayBetweenMoles);
        foreach(MoleSpawnerInfo info in _objsToSpawn)
        {
            _totalProbability += info.Probability;
            info.ProbabilityInContext = _totalProbability;
        }
        _toSpawn = _objsToSpawn.OrderBy(info => info.ProbabilityInContext).ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
        _manager = ModuleManager.GetModule<MoleMinigameManager>();
        _manager.MaxMoles = _spawnCount;
        _smallest = new Vector3(_pointA.position.x < _pointB.position.x ? _pointA.position.x : _pointB.position.x, 0, _pointA.position.z < _pointB.position.z ? _pointA.position.z : _pointB.position.z);
        _biggest = new Vector3(_pointA.position.x > _pointB.position.x ? _pointA.position.x : _pointB.position.x, 0, _pointA.position.z > _pointB.position.z ? _pointA.position.z : _pointB.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timed < _timerAll)
        {
            if (!_menuOpened)
            {
                _menuOpened = true;
                _manager.GameDone();
            }
            return;
        }

        _timer -= Time.deltaTime;
        _timerAll += Time.deltaTime;
        _manager.UpdateTimer(Mathf.FloorToInt(_timed-_timerAll));
        if (_timer < 0)
        {
            GameObject _objToSpawn = null;
            float selected = Random.Range(0, _totalProbability);
            foreach (MoleSpawnerInfo info in _toSpawn)
            {
                if(selected <= info.ProbabilityInContext)
                {
                    _objToSpawn = info.Mole;
                    break;
                }
            }
            GameObject clone = Instantiate(_objToSpawn, new Vector3(Random.Range(_smallest.x, _biggest.x), _minPos.position.y, Random.Range(_smallest.z, _biggest.z)), Quaternion.Euler(0, 0, 0));
            clone.GetComponentInChildren<MoleController>().enabled = true;
            _timer = Random.Range(_delayBetweenMoles * _delayDifference, _delayBetweenMoles);
        }
    }
}

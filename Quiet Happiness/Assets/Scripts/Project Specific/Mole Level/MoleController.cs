using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleController : MonoBehaviour
{
    public MoleType TypeOfMole;
    private bool _isHit;
    public bool IsHit { 
        get { return _isHit; } 
        set
        {
            _isHit = value;
        }  
    }

    [SerializeField] private Transform _transformToMove;
    [SerializeField] private Transform _minPos;
    [SerializeField] private Transform _maxPos;
    public GameObject CorrectHammer;

    [SerializeField] private float _timeToStay;
    private bool _goBack;

    public bool GoBack
    {
        get { return _goBack; }
        set
        {
            _goBack = value;
        }
    }

    private float _timer;

    // Start is called before the first frame update
    void Start()
    {
        _transformToMove.position = new Vector3(_transformToMove.position.x, _minPos.position.y, _transformToMove.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_goBack && _transformToMove.position.y < _maxPos.position.y)
        {
            _transformToMove.position = Vector3.Lerp(_transformToMove.position, new Vector3(_transformToMove.position.x, _maxPos.position.y + 0.1f, _transformToMove.position.z), Time.deltaTime * 2f);
        }

        if (_transformToMove.position.y >= _maxPos.position.y)
        {
            _timer += Time.deltaTime;
        }

        if (_transformToMove.position.y >= _maxPos.position.y && _timeToStay < _timer)
        {
            _goBack = true;
            this.GetComponentInParent<Animator>().SetBool("IsTimeOver", true);
        }

        if (_goBack && (_isHit || this.GetComponentInParent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !GetComponentInParent<Animator>().IsInTransition(0)) && _transformToMove.position.y >= _minPos.position.y)
        {
            _transformToMove.position = 
                Vector3.Lerp(_transformToMove.position,
                new Vector3(_transformToMove.position.x, _minPos.position.y - 0.1f,
                _transformToMove.position.z), Time.deltaTime * 2f);
        }

        if(_goBack && _transformToMove.position.y <= _minPos.position.y)
        {
            Destroy(gameObject);
        }
    }
}
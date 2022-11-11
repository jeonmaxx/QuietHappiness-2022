using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations;

public class HammerController : MonoBehaviour
{
    [SerializeField] private GameObject _hammerModel;
    [SerializeField] private List<GameObject> _hammerModels;

    [SerializeField] private PlayerInput _input;
    [SerializeField] private string _hitActionName;
    [SerializeField] private string _firstSelectActionName;
    [SerializeField] private string _secondSelectActionName;
    [SerializeField] private string _wheelSelectActionName;

    [SerializeField] private LayerMask _moveLayerMask;
    [SerializeField] private LayerMask _hitLayerMask;

    private Vector3 _originalRotation;
    private bool _isRotated;
    private bool _isSwitching;
    private int _currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        _originalRotation = new Vector3(_hammerModel.transform.eulerAngles.x, _hammerModel.transform.eulerAngles.y, _hammerModel.transform.eulerAngles.z);
        ModuleManager.GetModule<MoleMinigameManager>().UpdateHammerUI(_currentIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if (_input.actions[_hitActionName].triggered && !_isRotated)
        {
            _isRotated = true;
            StartCoroutine(Rotate(0.1f));
        }

        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, 50, _moveLayerMask))
        {
            transform.position =  Vector3.Lerp(hit.point, transform.position, 0.2f);
        }

        if (_input.actions[_firstSelectActionName].triggered)
        {
            StartCoroutine(SwitchHammer(0));
        }

        if (_input.actions[_secondSelectActionName].triggered)
        {
            StartCoroutine(SwitchHammer(1));
        }

        Vector2 wheel = _input.actions[_wheelSelectActionName].ReadValue<Vector2>();
        if (wheel.y != 0)
        {
            switch (_currentIndex)
            {
                case 0:
                    StartCoroutine(SwitchHammer(1));
                    break;

                case 1:
                    StartCoroutine(SwitchHammer(0));
                    break;
            }
        }
    }

    IEnumerator Rotate(float delay)
    {
        float passedTime = 0;
        Vector3 currentRot = _hammerModel.transform.eulerAngles;
        while (passedTime < delay)
        {
            passedTime += Time.deltaTime;
            _hammerModel.transform.eulerAngles = Vector3.Lerp(_originalRotation + new Vector3(90,0,0), currentRot, passedTime / delay);
            yield return null;
        }
        StartCoroutine(RotateBack(delay));
    }

    IEnumerator RotateBack(float delay)
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 50, _hitLayerMask))
        {
            MoleController info = hit.collider.GetComponent<MoleController>();
            if (info != null && !info.IsHit)
            {
                StartCoroutine(PlayMoleDeathAnimation(info));
                ModuleManager.GetModule<MoleMinigameManager>().UpdateScore(info.TypeOfMole, _hammerModel.Equals(info.CorrectHammer));
            }
        }
        else
        {
            ModuleManager.GetModule<MoleMinigameManager>().UpdateScore(MoleType.None, true);
        }

        float passedTime = 0;
        Vector3 currentRot = _hammerModel.transform.eulerAngles;
        while (passedTime < delay)
        {
            passedTime += Time.deltaTime;
            _hammerModel.transform.eulerAngles = Vector3.Lerp(currentRot, _originalRotation, passedTime / delay);
            yield return null;
        }
        _isRotated = false;
    }


    IEnumerator PlayMoleDeathAnimation(MoleController info)
    {
        info.IsHit = true;
        info.GetComponentInParent<Animator>().SetBool("IsHit", true);
        yield return new WaitForSeconds(1f);
        info.GoBack = true;
    }
    IEnumerator SwitchHammer(int index)
    {
        if (_isSwitching)
        {
            yield break;
        }

        _isSwitching = true;
        while (_isRotated)
        {
            yield return null;
        }

        _hammerModel.SetActive(false);
        _hammerModel = _hammerModels[index];
        _hammerModel.SetActive(true);
        _currentIndex = index;
        ModuleManager.GetModule<MoleMinigameManager>().UpdateHammerUI(_currentIndex);

        yield return new WaitForSeconds(0.25f);

        _isSwitching = false;
    }
}

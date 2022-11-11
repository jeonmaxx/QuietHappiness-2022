using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
[ExecuteInEditMode]
public class HUE_Shift : MonoBehaviour
{
    private Renderer _renderer;
    private MaterialPropertyBlock mBlock;

    [SerializeField] private bool _invert;
    [SerializeField, Range(0, 1)] private float _rangeMin = 0;
    [SerializeField, Range(0, 1)] private float _rangeMax = 1;
    [SerializeField, Range(0, 1)] private float _shift;
    [SerializeField, Range(-1, 1)] private float _saturation;
    [SerializeField, Range(-1, 1)] private float _luminance;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        mBlock = new MaterialPropertyBlock();
    }

    private void Update()
    {
        _renderer.GetPropertyBlock(mBlock);
        mBlock.SetInteger("_Invert", _invert ? 1 : 0);
        mBlock.SetFloat("_Shift", _shift);
        mBlock.SetFloat("_RangeMin", _rangeMin);
        mBlock.SetFloat("_RangeMax", _rangeMax);
        mBlock.SetFloat("_Saturation", _saturation);
        mBlock.SetFloat("_Luminance", _luminance);
        _renderer.SetPropertyBlock(mBlock);
    }
}
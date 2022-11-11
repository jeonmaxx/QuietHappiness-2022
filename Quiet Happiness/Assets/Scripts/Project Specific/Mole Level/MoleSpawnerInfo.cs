using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class MoleSpawnerInfo
{
    [Range(0, 1)] public float Probability;
    [ReadOnly] public float ProbabilityInContext;
    public GameObject Mole;
}

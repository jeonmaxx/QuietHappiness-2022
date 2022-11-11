using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowFunFact : MonoBehaviour
{
    [SerializeField] private List<string> _facts = new List<string>();
    [SerializeField] private Text _textDisplay;

    private void Start()
    {
        GetComponentInParent<Menu>().OnActiveChanged += SetText;
    }

    private void OnDestroy()
    {
        GetComponentInParent<Menu>().OnActiveChanged -= SetText;
    }

    private void SetText(bool isActive)
    {
        _textDisplay.text = _facts[Random.Range(0, _facts.Count - 1)];
    }
}

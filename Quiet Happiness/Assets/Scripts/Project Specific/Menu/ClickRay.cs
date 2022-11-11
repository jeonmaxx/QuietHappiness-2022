using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickRay : MonoBehaviour
{
    [SerializeField] private string _mouseClickAction;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private LayerMask _placementMask;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (Physics.Raycast(ray, out RaycastHit hit, 50, _placementMask))
        {
            if (_input.actions[_mouseClickAction].triggered)
            {
                //TODO zusammenfassen
                if (hit.collider.gameObject.GetComponent<ZoomMenuItemTriggerButton>() != null)
                {
                    hit.collider.gameObject.GetComponent<ZoomMenuItemTriggerButton>().Click();
                }
                else if (hit.collider.gameObject.GetComponent<UIToggleInventory>() != null)
                {
                    hit.collider.gameObject.GetComponent<UIToggleInventory>().Click();
                }
                else if (hit.collider.gameObject.GetComponent<Item>() != null)
                {
                    hit.collider.gameObject.GetComponent<Item>().Click();
                }
            }
        }
    }
}

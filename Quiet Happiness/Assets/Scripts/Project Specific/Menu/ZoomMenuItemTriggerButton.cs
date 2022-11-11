using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ZoomMenuItemTriggerButton : MonoBehaviour
{
    [SerializeField] private bool inMenu;
    [SerializeField] private ZoomMenuItem _zoom;
    [SerializeField] private AudioMixer _clickSounds;
    private bool _isEnabled;
    [SerializeField] [DropDown(nameof(_menuTypes))] private int _menuToBeAllowed;
    private List<string> _menuTypes;

    public void Awake()
    {
        GetComponentInParent<Menu>().OnActiveChanged += (isActive) => { _isEnabled = isActive; };
    }

    public void Click()
    {
        if (!(inMenu ^ _zoom.Zoomed) && !(_isEnabled ^ !_zoom.Zoomed) && ModuleManager.GetModule<MenuManager>().CurrentActiveMenuList.MenuTypeID.Equals(_menuToBeAllowed))
        {
            if(inMenu) _zoom.ZoomOut();
            else _zoom.ZoomIn();
            GetComponentInParent<Menu>().IsActive = !_zoom.Zoomed;

            if (_clickSounds != null)
            {
                _clickSounds.PlayRandomSource();
            }
        }
    }

    private void OnValidate()
    {
        _menuTypes = MenuManager.MenuTypesForEditor;
    }
}

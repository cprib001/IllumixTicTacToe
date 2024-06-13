using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationButton : ButtonBase
{
    // Members

    private MenuManager _menuManager;
    [SerializeField]
    private MenuManager.PanelType _panelType;

    // Monobehaviour

    private void Start()
    {
        _menuManager = MenuManager.Instance;
    }

    // Private Methods

    protected override void OnClick()
    {
        _menuManager.TurnOnPanel(_panelType);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameButton : ButtonBase
{
    // Members

    private MenuManager _menuManager;
    [SerializeField]
    private GameManager.GameMode _gameMode;

    // Monobehaviour

    private void Start()
    {
        _menuManager = MenuManager.Instance;
    }

    // Private Methods

    protected override void OnClick()
    {
        _menuManager.StartGame(_gameMode);
    }
}

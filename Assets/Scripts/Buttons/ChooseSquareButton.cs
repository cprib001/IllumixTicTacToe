using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseSquareButton : ButtonBase
{
    // Members

    private EventManager _eventManager;
    private GameManager _gameManager;
    [SerializeField]
    private TMP_Text _choice;
    private bool _selected;
    [SerializeField]
    private int _row,
        _column;

    // Monobehaviour

    private void Start()
    {
        _eventManager = EventManager.Instance;
        _gameManager = GameManager.Instance;

        _eventManager.ResetBoardEvent.AddListener(OnResetBoard);
        _eventManager.GameEndEvent.AddListener(OnGameEnd);
        _eventManager.BeginAITurn.AddListener(OnBeginAITurn);
        _eventManager.EndAITurn.AddListener(OnEndAITurn);
    }

    // Private Methods

    protected override void OnClick()
    {
        UpdateVisuals();

        _gameManager.RemoveFromButtons(_myButton);

        _gameManager.PlayMove(_row, _column);

        _selected = true;
    }

    private void OnResetBoard()
    {
        _choice.text = "";
        _myButton.interactable = true;
        _selected = false;
    }

    private void OnGameEnd()
    {
        _myButton.interactable = false;
    }

    private void OnBeginAITurn()
    {
        _myButton.interactable = false;
    }

    private void OnEndAITurn()
    {
        if(!_selected)
        {
            _myButton.interactable = true;
        }
    }

    private void UpdateVisuals()
    {
        if(_gameManager.FirstPlayersTurn)
        {
            _choice.text = "X";
        }
        else
        {
            _choice.text = "O";
        }

        _myButton.interactable = false;
    }
}

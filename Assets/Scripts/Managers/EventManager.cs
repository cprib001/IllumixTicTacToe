using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    // Members

    private static EventManager _instance;
    private UnityEvent _resetBoardEvent = new UnityEvent(),
        _gameEndEvent = new UnityEvent(),
        _beginAITurn = new UnityEvent(),
        _endAITurn = new UnityEvent();

    public static EventManager Instance
    {
        get { return _instance; }
    }
    public UnityEvent ResetBoardEvent
    {
        get { return _resetBoardEvent; }
        private set { _resetBoardEvent = value; }
    }
    public UnityEvent GameEndEvent
    {
        get { return _gameEndEvent; }
        private set { _gameEndEvent = value; }
    }
    public UnityEvent BeginAITurn
    {
        get { return _beginAITurn; }
        private set { _beginAITurn = value; }
    }
    public UnityEvent EndAITurn
    {
        get { return _endAITurn; }
        private set { _endAITurn = value; }
    }

    // Monobehaviour

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void OnDestroy()
    {
        _resetBoardEvent.RemoveAllListeners();
        _gameEndEvent.RemoveAllListeners();
        _beginAITurn.RemoveAllListeners();
        _endAITurn.RemoveAllListeners();
    }
}

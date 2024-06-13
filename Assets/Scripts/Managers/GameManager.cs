using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Members

    [SerializeField]
    private Button[] _allChooseSquareButtons;
    private EventManager _eventManager;
    [SerializeField]
    private GameObject _playAgainButtonGameObject;
    private List<Button> _unselectedButtons = new List<Button>();
    [SerializeField]
    private TMP_Text _statusTMP;
    private static GameManager _instance;
    public GameMode _gameMode;
    private bool _firstPlayersTurn = true;
    private const int BoardSize = 3;
    private int[] _rowsCount = new int[3],
        _columnsCount = new int[3],
        _diagonalsCount = new int[2];
    private const string PlayerOnesTurn = "Player One's Turn",
        PlayerTwosTurn = "Player Two's Turn",
        PlayersTurn = "Player's Turn",
        ComputersTurn = "Computer's Turn",
        PlayerOneWins = "Player One Wins!",
        PlayerTwoWins = "Player Two Wins!",
        PlayerWins = "Player Wins!",
        ComputerWins = "Computer Wins",
        Draw = "Draw!";

    public static GameManager Instance
    {
        get { return _instance; }
    }
    public bool FirstPlayersTurn
    {
        get { return _firstPlayersTurn; }
        private set { _firstPlayersTurn = value; }
    }

    public enum GameMode
    {
        OnePlayer,
        TwoPlayer
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

    private void Start()
    {
        _eventManager = EventManager.Instance;
    }

    private void ResetBoard()
    {
        _playAgainButtonGameObject.SetActive(false);
        UpdateStatus();
        _rowsCount = new int[3];
        _columnsCount = new int[3];
        _diagonalsCount = new int[2];
        _unselectedButtons = new List<Button>(_allChooseSquareButtons);
        _firstPlayersTurn = true;

        if (_eventManager == null)
        {
            _eventManager = EventManager.Instance;
        }
        _eventManager.ResetBoardEvent.Invoke();
    }

    private void UpdateStatus()
    {
        if(_gameMode == GameMode.OnePlayer)
        {
            if (_firstPlayersTurn)
            {
                _statusTMP.text = PlayersTurn;
            }
            else
            {
                _statusTMP.text = ComputersTurn;
            }
        }
        else if(_gameMode == GameMode.TwoPlayer)
        {
            if (_firstPlayersTurn)
            {
                _statusTMP.text = PlayerOnesTurn;
            }
            else
            {
                _statusTMP.text = PlayerTwosTurn;
            }
        }
    }

    private void EndGame(bool wasDraw)
    {
        if(wasDraw)
        {
            _statusTMP.text = Draw;
        }
        else
        {
            if (_gameMode == GameMode.OnePlayer)
            {
                if (_firstPlayersTurn)
                {
                    _statusTMP.text = PlayerWins;
                }
                else
                {
                    _statusTMP.text = ComputerWins;
                }
            }
            else if (_gameMode == GameMode.TwoPlayer)
            {
                if (_firstPlayersTurn)
                {
                    _statusTMP.text = PlayerOneWins;
                }
                else
                {
                    _statusTMP.text = PlayerTwoWins;
                }
            }
        }

        _playAgainButtonGameObject.SetActive(true);
    }

    /// <summary>
    /// Check if any rows, columns or diagonals equals +BoardSize or -BoardSize
    /// If any of these conditions are true it means the current player has won the game!
    /// Best part of this function is that it easily handles any N x N size board
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    /// <param name="increment"></param>
    /// <returns></returns>
    private bool CheckIfCurrentPlayerWon(int row, int column)
    {
        int increment;

        if (_firstPlayersTurn)
        {
            increment = 1;
        }
        else
        {
            increment = -1;
        }

        // Increment (or decrement) the corresponding row/column

        _rowsCount[row] += increment;

        if (_rowsCount[row] == BoardSize || _rowsCount[row] == -BoardSize)
        {
            return true;
        }

        _columnsCount[column] += increment;

        if (_columnsCount[column] == BoardSize || _columnsCount[column] == -BoardSize)
        {
            return true;
        }

        // If row == column, it is on our principal diagonal (top left to bottom right)
        if (row == column)
        {
            _diagonalsCount[0] += increment;

            if (_diagonalsCount[0] == BoardSize || _diagonalsCount[0] == -BoardSize)
            {
                return true;
            }
        }

        // If row + column == BoardSize - 1, it is on the secondary diagonal (top right to bottom left)
        if ((row + column) == (BoardSize - 1))
        {
            _diagonalsCount[1] += increment;

            if (_diagonalsCount[1] == BoardSize || _diagonalsCount[1] == -BoardSize)
            {
                return true;
            }
        }

        return false;
    }

    private bool CheckIfDraw()
    {
        return _unselectedButtons.Count == 0;
    }

    private void AITurn()
    {
        StopCoroutine("AITurnCoroutine");
        StartCoroutine("AITurnCoroutine");
    }

    private IEnumerator AITurnCoroutine()
    {
        _eventManager.BeginAITurn.Invoke();

        yield return new WaitForSeconds(1);

        _eventManager.EndAITurn.Invoke();

        _unselectedButtons[Random.Range(0, _unselectedButtons.Count)].onClick.Invoke();
    }

    // Public Methods

    /// <summary>
    /// Plays a move with index starting at 0.
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    public void PlayMove(int row, int column)
    {
        if(CheckIfCurrentPlayerWon(row, column))
        {
            _eventManager.GameEndEvent.Invoke();

            EndGame(false);
        }
        else
        {
            if(CheckIfDraw())
            {
                _eventManager.GameEndEvent.Invoke();

                EndGame(true);
            }
            else
            {
                _firstPlayersTurn = !_firstPlayersTurn;

                UpdateStatus();

                if (!_firstPlayersTurn && _gameMode == GameMode.OnePlayer)
                {
                    AITurn();
                }
            }
        }
    }

    public void StartGame(GameMode mode)
    {
        _gameMode = mode;

        ResetBoard();
    }

    public void RemoveFromButtons(Button button)
    {
        for (int i = 0; i < _unselectedButtons.Count; i++)
        {
            if (_unselectedButtons[i] == button)
            {
                _unselectedButtons.RemoveAt(i);
            }
        }
    }
}

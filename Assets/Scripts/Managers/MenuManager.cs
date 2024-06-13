using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Members

    private GameManager _gameManager;
    [SerializeField]
    private GameObject _startPanel,
        _gamePanel;
    private static MenuManager _instance;

    public static MenuManager Instance
    {
        get { return _instance; }
    }
    public enum PanelType
    {
        Start,
        Game,
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
        _gameManager = GameManager.Instance;

        TurnOnPanel(PanelType.Start);
    }

    // Private Methods

    private void TurnOffAllPanels()
    {
        _startPanel.SetActive(false);
        _gamePanel.SetActive(false);
    }

    // Public Methods

    public void TurnOnPanel(PanelType type)
    {
        TurnOffAllPanels();

        if(type == PanelType.Start)
        {
            _startPanel.SetActive(true);
        }
        else if(type == PanelType.Game)
        {
            _gamePanel.SetActive(true);
        }
    }

    public void StartGame(GameManager.GameMode mode)
    {
        _gameManager.StartGame(mode);
    }
}

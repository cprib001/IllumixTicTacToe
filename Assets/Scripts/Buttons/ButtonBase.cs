using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBase : MonoBehaviour
{
    // Members

    protected Button _myButton;

    // Monobehaviour

    private void Awake()
    {
        _myButton = GetComponent<Button>();
        _myButton.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _myButton.onClick.RemoveAllListeners();
    }

    // Private Methods

    protected virtual void OnClick() { }
}

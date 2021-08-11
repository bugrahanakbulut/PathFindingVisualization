using System;
using InputSystem;
using UnityEngine;

public class InputListener_KeyDown : InputListenerBase<InputData_Key>
{
    [SerializeField] private KeyCode[] _targetKeyCodes = null;

    #region Events

    public override Action<InputData_Key> OnInputEventTriggered { get; set; }

    #endregion
    
    public override EInputEvent GetInputEventType()
    {
        return EInputEvent.KeyDown;
    }

    private void Update()
    {
        foreach (KeyCode targetKeyCode in _targetKeyCodes)
        {
            if (Input.GetKeyDown(targetKeyCode))
            {
                OnInputEventTriggered?.Invoke(
                    new InputData_Key(
                        GetInputEventType(),
                        targetKeyCode));
            }
        }
    }
}

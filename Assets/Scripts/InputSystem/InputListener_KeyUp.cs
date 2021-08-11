using System;
using UnityEngine;

namespace InputSystem
{
    public class InputData_Key : InputData
    {
        public KeyCode KeyCode { get; } = KeyCode.None;

        public InputData_Key(EInputEvent inputEventType, KeyCode keyCode) 
            : base(inputEventType)
        {
            KeyCode = keyCode;
        }
    }

    public class InputListener_KeyUp : InputListenerBase<InputData_Key>
    {
        [SerializeField] private KeyCode[] _targetKeyCodes = null;
        
        #region Events

        public override Action<InputData_Key> OnInputEventTriggered { get; set; }

        #endregion
        
        public override EInputEvent GetInputEventType()
        {
            return EInputEvent.KeyUp;
        }
        
        private void Update()
        {
            foreach (KeyCode keyCode in _targetKeyCodes)
            {
                if (Input.GetKeyUp(keyCode))
                {
                    OnInputEventTriggered?.Invoke(new InputData_Key(EInputEvent.KeyUp, keyCode));
                }
            }
        }
    }
}
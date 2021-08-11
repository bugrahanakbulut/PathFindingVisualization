using System;
using UnityEngine;

namespace InputSystem
{
    public class InputData_Mouse : InputData
    {
        public int MouseButton { get; }
        
        public Vector3 CursorPosition { get; }

        public InputData_Mouse(EInputEvent inputEventType, int mouseButton, Vector3 cursorPosition) 
            : base(inputEventType)
        {
            MouseButton = mouseButton;
            CursorPosition = cursorPosition;
        }
    }
    
    public class InputListener_MouseUp : InputListenerBase<InputData_Mouse>
    {
        [SerializeField] private int[] _targetMouseButtons = null;
        
        #region Events

        public override Action<InputData_Mouse> OnInputEventTriggered { get; set; }

        #endregion
        
        public override EInputEvent GetInputEventType()
        {
            return EInputEvent.MouseButtonUp;
        }
        
        private void Update()
        {
            foreach (int targetMouseButton in _targetMouseButtons)
            {
                if (Input.GetMouseButtonUp(targetMouseButton))
                {
                    OnInputEventTriggered?.Invoke(
                        new InputData_Mouse(
                            EInputEvent.MouseButtonUp,
                            targetMouseButton,
                            Input.mousePosition));
                }
            }
        }
    }
}
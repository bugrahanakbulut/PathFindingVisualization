using System;
using UnityEngine;

namespace InputSystem
{
    public class InputListener_MouseButton : InputListenerBase<InputData_Mouse>
    {
        [SerializeField] private int[] _targetMouseButtons = null;

        #region Events

        public override Action<InputData_Mouse> OnInputEventTriggered { get; set; }

        #endregion

        public override EInputEvent GetInputEventType()
        {
            return EInputEvent.MouseButtonHold;
        }

        private void Update()
        {
            foreach (int targetMouseButton in _targetMouseButtons)
            {
                if (Input.GetMouseButton(targetMouseButton))
                {
                    OnInputEventTriggered?.Invoke(
                        new InputData_Mouse(
                            GetInputEventType(),
                            targetMouseButton,
                            Input.mousePosition));
                }
            }
        }
    }
}

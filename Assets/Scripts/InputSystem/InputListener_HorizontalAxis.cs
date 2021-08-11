using System;
using UnityEngine;

namespace InputSystem
{
    public class InputData_HorizontalAxis : InputData
    {
        public float Amount { get; }

        public InputData_HorizontalAxis(EInputEvent inputEventType, float amount) 
            : base(inputEventType)
        {
            Amount = amount;
        }
    }
    
    public class InputListener_HorizontalAxis : InputListenerBase<InputData_HorizontalAxis>
    {
        #region Constants

        private const string AXIS_NAME = "Horizontal";

        #endregion

        #region Events

        public override Action<InputData_HorizontalAxis> OnInputEventTriggered { get; set; }

        #endregion
        
        public override EInputEvent GetInputEventType()
        {
            return EInputEvent.HorizontalAxis;
        }

        private void Update()
        {
            float inputAmount = Input.GetAxis(AXIS_NAME);

            if (inputAmount != 0)
            {
                OnInputEventTriggered?.Invoke(new InputData_HorizontalAxis(
                    EInputEvent.HorizontalAxis, inputAmount));
            }
        }

        
    }
}
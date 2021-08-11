using System;
using System.Linq;
using UnityEngine;

namespace InputSystem
{
    public class InputManager : MonoBehaviour
    {
        #region Singleton

        private static InputManager _instance;
        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<InputManager>();
                }

                return _instance;
            }
        }

        #endregion

        private InputListenerBase[] _inputListeners;
        private InputListenerBase[] _InputListeners
        {
            get
            {
                if (_inputListeners == null)
                {
                    _inputListeners = GetComponents<InputListenerBase>();
                }

                return _inputListeners;
            }
        }

        public bool TryRegisterToInputListener<T>(EInputEvent inputEvent, Action<T> callback)
            where T : InputData
        {
            InputListenerBase listener = _InputListeners.SingleOrDefault(i => i.GetInputEventType() == inputEvent);

            if (listener != null)
            {
                InputListenerBase<T> inputListener = (InputListenerBase<T>) listener;
                
                inputListener.OnInputEventTriggered += callback;

                return true;
            }

            return false;
        }

        public bool TryUnregisterFromInputListener<T>(EInputEvent inputEvent, Action<T> callback)
            where T : InputData
        {
            InputListenerBase listener = _InputListeners.SingleOrDefault(i => i.GetInputEventType() == inputEvent);

            if (listener != null)
            {
                InputListenerBase<T> inputListener = (InputListenerBase<T>) listener;
                
                inputListener.OnInputEventTriggered -= callback;

                return true;
            }

            return false;
        }
    }
}

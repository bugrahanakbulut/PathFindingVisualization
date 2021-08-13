using System;
using InputSystem;
using UnityEngine;

namespace ResetSystem
{
    public class ResetManager : MonoBehaviour
    {
        [SerializeField] private KeyCode _targetKeyCode = KeyCode.R;


        private Resetable[] _Resetables => FindObjectsOfType<Resetable>();

        private void Awake()
        {
            InputManager.Instance.TryRegisterToInputListener<InputData_Key>(EInputEvent.KeyUp, OnPressedKey);
        }

        private void OnDestroy()
        {
            if (InputManager.Instance == null)
            {
                return;
            }
            
            InputManager.Instance.TryUnregisterFromInputListener<InputData_Key>(EInputEvent.KeyUp, OnPressedKey);
        }

        private void OnPressedKey(InputData_Key inputData)
        {
            if (inputData.KeyCode != _targetKeyCode)
            {
                return;
            }

            foreach (Resetable resetable in _Resetables)
            {
                if (resetable.gameObject.activeInHierarchy)
                {
                    resetable.Reset();
                }
            }
        }
    }
}
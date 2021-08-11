using System;
using InputSystem;
using UnityEngine;

namespace TileSystem
{
    public class TileSelector<T> : MonoBehaviour
        where T : ITilePositionProvider
    {
        [SerializeField] private KeyCode _targetKey = KeyCode.None;

        [SerializeField] private int _targetMouseButton = 0;
        
        private Camera _mainCamera;

        private Camera _MainCamera
        {
            get
            {
                if (_mainCamera == null)
                {
                    _mainCamera = Camera.main;
                }

                return _mainCamera;
            }
        }
    
        #region Events

        public Action<T, Vector3, Vector2Int> OnTileSelected { get; set; }

        #endregion
    
        private void Awake()
        {
            InputManager.Instance.TryRegisterToInputListener<InputData_Key>(EInputEvent.KeyDown, OnKeyDown);
            InputManager.Instance.TryRegisterToInputListener<InputData_Key>(EInputEvent.KeyUp, OnKeyUp);
        }

        private void OnDestroy()
        {
            if (InputManager.Instance == null)
            {
                return;
            }
        
            InputManager.Instance.TryUnregisterFromInputListener<InputData_Key>(EInputEvent.KeyDown, OnKeyDown);
            InputManager.Instance.TryUnregisterFromInputListener<InputData_Key>(EInputEvent.KeyUp, OnKeyUp);
        }

        private void OnKeyDown(InputData_Key inputData)
        {
            if (inputData.KeyCode == _targetKey)
            {
                InputManager.Instance.TryRegisterToInputListener<InputData_Mouse>(EInputEvent.MouseButtonHold,
                    OnMouseButtonHold);
            }
        }

        private void OnKeyUp(InputData_Key inputData)
        {
            if (inputData.KeyCode == _targetKey)
            {
                InputManager.Instance.TryUnregisterFromInputListener<InputData_Mouse>(EInputEvent.MouseButtonHold,
                    OnMouseButtonHold);
            }
        }

        private void OnMouseButtonHold(InputData_Mouse inputData)
        {
            if (inputData.MouseButton != _targetMouseButton)
            {
                return;
            }
            
            RaycastHit hit;

            Ray ray = _MainCamera.ScreenPointToRay(inputData.CursorPosition);

            if (Physics.Raycast(ray, out hit))
            {
                T selectableTile = hit.transform.GetComponent<T>();

                if (selectableTile != null)
                {
                    OnTileSelected?.Invoke(selectableTile, hit.transform.position, selectableTile.TilePos);
                }
            }
        }
    }
}

using UnityEngine;

namespace InputSystem
{
    public class InputSystem_Sample : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody = null;

        [SerializeField] private float _movementSpeed = 5f;
        
        [SerializeField] private float _jumpPower = 10f;
        
        private void Awake()
        {
            RegisterToInputManager();
        }

        private void OnDestroy()
        {
            UnregisterFromInputManager();
        }

        private void RegisterToInputManager()
        {
            /*InputManager.Instance.TryRegisterToInputListener<InputData_HorizontalAxis>(OnHorizontalInputPerformed);
            InputManager.Instance.TryRegisterToInputListener<InputData_Key>(OnKeyUp);*/
        }

        private void UnregisterFromInputManager()
        {
            if (InputManager.Instance == null)
            {
                return;
            }
            
            /*InputManager.Instance.TryUnregisterFromInputListener<InputData_HorizontalAxis>(OnHorizontalInputPerformed);
            InputManager.Instance.TryUnregisterFromInputListener<InputData_Key>(OnKeyUp);*/
        }

        private void OnHorizontalInputPerformed(InputData_HorizontalAxis horizontalAxisInputData)
        {
            _rigidbody.position += Vector3.right * (horizontalAxisInputData.Amount * _movementSpeed * Time.deltaTime);
        }
        
        private void OnKeyUp(InputData_Key keyData)
        {
            if (keyData.KeyCode != KeyCode.Space)
            {
                return;
            }
            
            _rigidbody.AddForce(Vector3.up * _jumpPower, ForceMode.VelocityChange);
        }
    }
}
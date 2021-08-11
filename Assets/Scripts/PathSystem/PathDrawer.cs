using InputSystem;
using TileSystem;
using UnityEngine;

namespace PathSystem
{
    public class PathDrawer : MonoBehaviour
    {
        [SerializeField] private KeyCode _pathDrawKeyCode = KeyCode.P;

        [SerializeField] private Ground _ground = null;

        [SerializeField] private PathFinder _pathFinder = null;

        [SerializeField] private Tile _sourceTile = null;

        [SerializeField] private Tile _destinationTile = null;
        
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

        private void OnPressedKey(InputData_Key inputDataKey)
        {
            if (inputDataKey.KeyCode != _pathDrawKeyCode)
            {
                return;
            }
            
            _pathFinder.FindPath(_ground.GroundTiles, _sourceTile, _destinationTile);
        }
    }
}
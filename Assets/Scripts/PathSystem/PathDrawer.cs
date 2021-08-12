using System;
using System.Linq;
using InputSystem;
using TileSystem;
using UnityEngine;
using UnityEngine.UI;

namespace PathSystem
{
    [Serializable]
    public class PathFinderTypeNameMapping 
    {
        [SerializeField] private String _name = "";
        public string Name => _name;

        [SerializeField] private EPathFinder _ePathFinder = EPathFinder.None;
        public EPathFinder EPathFinder => _ePathFinder;
    }
    
    public class PathDrawer : MonoBehaviour
    {
        [SerializeField] private KeyCode _pathDrawKeyCode = KeyCode.P;

        [SerializeField] private Ground _ground = null;
        
        [SerializeField] private Tile _sourceTile = null;

        [SerializeField] private Tile _destinationTile = null;

        [SerializeField] private Dropdown _dropdown = null;

        [SerializeField] private PathFinderTypeNameMapping[] _mappings = null;
        
        private PathFinder[] _pathFinders;

        private PathFinder[] _PathFinders
        {
            get
            {
                if (_pathFinders == null)
                {
                    _pathFinders = GetComponents<PathFinder>();
                }

                return _pathFinders;
            }
        }
        
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

            string optionStr = _dropdown.options[_dropdown.value].text;

            EPathFinder pathFinder = _mappings.SingleOrDefault(i => i.Name.Equals(optionStr)).EPathFinder;

            /*_pathFinder.FindPath(_ground.GroundTiles, _sourceTile, _destinationTile);*/

            PathFinder pf = _PathFinders.SingleOrDefault(i => i.GetPathFinderType() == pathFinder);

            pf.FindPath(_ground.GroundTiles, _sourceTile, _destinationTile);
        }
    }
}
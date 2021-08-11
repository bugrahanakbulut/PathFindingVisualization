using UnityEngine;

namespace PathSystem
{
    public class PathObject : MonoBehaviour
    {
        [SerializeField] private Color _selectedColor = Color.clear;  

        [SerializeField] private Color _discoveredColor = Color.clear;  
        
        [SerializeField] private Color _includedPathColor = Color.clear;

        private MeshRenderer _meshRenderer;

        private MeshRenderer _MeshRenderer
        {
            get
            {
                if (_meshRenderer == null)
                {
                    _meshRenderer = GetComponentInChildren<MeshRenderer>();
                }

                return _meshRenderer;
            }
        }
        
        public void PathObjectSelected()
        {
            UpdatePathObjectColor(_selectedColor);
        }

        public void PathObjectDiscovered()
        {
            UpdatePathObjectColor(_discoveredColor);
        }

        public void PathObjectIncludedPath()
        {
            UpdatePathObjectColor(_includedPathColor);
        }

        private void UpdatePathObjectColor(Color color)
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();

            block.SetColor("_BaseColor", color);
            
            _MeshRenderer.SetPropertyBlock(block);
        }
    }
}
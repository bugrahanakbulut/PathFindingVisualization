using UnityEngine;

namespace ResetSystem
{
    public class ColorResetable : Resetable
    {
        [SerializeField] private Color _defaultColor = Color.white;
        
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
        
        public override void Reset()
        {
            MaterialPropertyBlock mpb = new MaterialPropertyBlock();
            
            mpb.SetColor("_BaseColor", _defaultColor);

            _MeshRenderer.SetPropertyBlock(mpb);
        }
    }
}
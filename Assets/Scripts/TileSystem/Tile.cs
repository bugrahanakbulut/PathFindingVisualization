using UnityEngine;

namespace TileSystem
{
    public static class TileExtensions
    {
        public static bool CheckTilePositionIsValid(this Tile tile)
        {
            if (tile.TilePos.x == -1 || tile.TilePos.y == -1)
            {
                return false;
            }

            return true;
        } 
    }
    
    public class Tile : MonoBehaviour
    {
        [SerializeField] private float _size = 1f;

        [SerializeField] private Material _tileMat = null;

        [SerializeField] private Transform _visualTransform = null;
        
        public Vector2Int TilePos { get; protected set; }

        private bool _isVisuallyInited = false;
        
        public void InitTile(Vector2Int tilePos)
        {
            TilePos = tilePos;

            if (_isVisuallyInited)
            {
                return;
            }

            _isVisuallyInited = true;
            
            Vector3[] vertices = GetVertices();

            int[] triangles = GetTriangles();

            MeshFilter mf = _visualTransform.gameObject.AddComponent<MeshFilter>();
            MeshRenderer mr = _visualTransform.gameObject.AddComponent<MeshRenderer>();            

            Mesh mesh = new Mesh();
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = GetUvs();
            mesh.Optimize();
            mesh.RecalculateNormals();

            mf.mesh = mesh;

            mr.material = _tileMat;
        }

        private Vector3[] GetVertices()
        {
            return new Vector3[]
            {
                new Vector3(-_size/2, _size/2, -_size/2),
                new Vector3(-_size/2, -_size/2, -_size/2),
                new Vector3(_size/2, _size/2, -_size/2),
                new Vector3(_size/2, -_size/2, -_size/2),
                
                new Vector3(-_size/2, -_size/2, _size/2),
                new Vector3(_size/2, -_size/2, _size/2),
                new Vector3(-_size/2, _size/2, _size/2),
                new Vector3(_size/2, _size/2, _size/2),
                
                new Vector3(-_size/2, _size/2, -_size/2),
                new Vector3(_size/2, _size/2, -_size/2),

                new Vector3(-_size/2, _size/2, -_size/2),
                new Vector3(-_size/2, _size/2, _size/2),

                new Vector3(_size/2, _size/2, -_size/2),
                new Vector3(_size/2, _size/2, _size/2),

            };
        }

        private int[] GetTriangles()
        {
            return new int[]
            {
                // front face
                0, 2, 1, 
                1, 2, 3,
                
                // back face
                4, 5, 6, 
                5, 7, 6,
                
                //top face
                6, 7, 8,
                7, 9 ,8, 
                
                //bottom face
                1, 3, 4, 
                3, 5, 4,
                
                // left face
                1, 11,10,
                1, 4, 11,
                
                //right face
                3, 12, 5,
                5, 12, 13
            
            };
        }

        private Vector2[] GetUvs()
        {
            Vector2[] uvs = {
                new Vector2(0, 0.66f),
                new Vector2(0.25f, 0.66f),
                new Vector2(0, 0.33f),
                new Vector2(0.25f, 0.33f),

                new Vector2(0.5f, 0.66f),
                new Vector2(0.5f, 0.33f),
                new Vector2(0.75f, 0.66f),
                new Vector2(0.75f, 0.33f),

                new Vector2(1, 0.66f),
                new Vector2(1, 0.33f),

                new Vector2(0.25f, 1),
                new Vector2(0.5f, 1),

                new Vector2(0.25f, 0),
                new Vector2(0.5f, 0),
            };

            return uvs;
        }

        private void OnDrawGizmos()
        {
            /*Vector3[] vert = GetVertices();

            Gizmos.color = Color.red;
            Handles.color = Color.red;

            for (int i = 0; i < 8; i++)
            {
                Gizmos.DrawWireSphere(vert[i], 0.1f);
                Handles.Label(vert[i], i.ToString());
            }*/
        }
    }
}

using UnityEngine;

namespace PoolingSystem
{
    public class PoolingSystem_Sample : MonoBehaviour
    {
        [SerializeField] private Pool_MB _pool = null;
        
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
            {
                TryUsePool();
            }

            if (Input.GetKeyUp(KeyCode.Backspace))
            {
                _pool.FreePool();
            }

            if (Input.GetKeyUp(KeyCode.R))
            {
                _pool.ResetPool();
            }
        }
        
        private bool TryUsePool()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity))
            {
                int randPool = Random.Range(0, _pool.PoolCount);

                PoolObject poolObject = _pool.AllocatePoolObject(randPool);

                poolObject.transform.position = hitInfo.point;

                return true;
            }

            return false;
        }
    }
}

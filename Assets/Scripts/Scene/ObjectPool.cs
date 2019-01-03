using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scene
{
    public class ObjectPool<T> : MonoBehaviour where T: MonoBehaviour
    {
        [SerializeField]
        private GameObject _prefab;

        private Stack<T> _pool = new Stack<T>();
        
        public void WarmUp(int initialPoolSize)
        {                        
            while (_pool.Count < initialPoolSize)
            {
                _pool.Push(GetNewInstance());                   
            }
        }

        public T Get()
        {
            if (_pool.Count > 0)
            {
                return _pool.Pop();
            }
            else
            {
                return GetNewInstance();
            }
        }

        public void Release(T instance)
        {
            instance.gameObject.SetActive(false);
            instance.transform.parent = transform;
            _pool.Push(instance);
        }


        T GetNewInstance()
        {
            GameObject instance = Instantiate(_prefab, transform);
            instance.gameObject.SetActive(false);
            return instance.GetComponent<T>();
        }
    }
}
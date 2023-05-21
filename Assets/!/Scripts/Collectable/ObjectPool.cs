using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class ObjectPool: MonoBehaviour
    {
        // This is a generic object pool that can be used for any type of object
        // It is used to avoid instantiating and destroying objects at runtime

        [SerializeField] private GameObject _objectPrefab;
        [SerializeField] private int _initialPoolSize = 10;

        private Queue<GameObject> _pooledObjects;

        private void Awake()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            _pooledObjects = new Queue<GameObject>();

            // Instantiate the initial pool of objects
            for (int i = 0; i < _initialPoolSize; i++)
            {
                GameObject obj = Instantiate(_objectPrefab);
                obj.SetActive(false);
                _pooledObjects.Enqueue(obj);
                obj.transform.SetParent(transform);
            }
        }

        /// <summary>
        /// Returns an object from the pool
        /// </summary>
        public GameObject GetObject()
        {
            // If there are no objects left in the pool, instantiate a new one
            if (_pooledObjects.Count == 0)
            {
                GameObject obj = Instantiate(_objectPrefab);
                obj.SetActive(false);
                _pooledObjects.Enqueue(obj);
                obj.transform.SetParent(transform);
            }

            // Return the first object in the queue
            GameObject pooledObject = _pooledObjects.Dequeue();
            pooledObject.SetActive(true);

            return pooledObject;
        }

        /// <summary>
        /// Returns an object to the pool
        /// </summary>
        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            _pooledObjects.Enqueue(obj);
        }
    }
}
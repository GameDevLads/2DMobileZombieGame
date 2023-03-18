using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Collectable
{
    public class XPPool : MonoBehaviour
    {
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

            for (int i = 0; i < _initialPoolSize; i++)
            {
                GameObject obj = Instantiate(_objectPrefab);
                obj.SetActive(false);
                _pooledObjects.Enqueue(obj);
                obj.transform.SetParent(transform);
            }
        }

        public GameObject GetObject()
        {
            if (_pooledObjects.Count == 0)
            {
                GameObject obj = Instantiate(_objectPrefab);
                obj.SetActive(false);
                _pooledObjects.Enqueue(obj);
                obj.transform.SetParent(transform);
            }

            GameObject pooledObject = _pooledObjects.Dequeue();
            pooledObject.SetActive(true);

            return pooledObject;
        }

        public void ReturnObject(GameObject obj)
        {
            obj.SetActive(false);
            _pooledObjects.Enqueue(obj);
        }
    }
}
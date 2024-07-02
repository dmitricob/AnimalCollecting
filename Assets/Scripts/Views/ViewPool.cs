using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Views
{
    public class ViewPool
    {
        private Dictionary<GameObject, Queue<GameObject>> _pushed = new Dictionary<GameObject, Queue<GameObject>>();
        private Dictionary<GameObject, HashSet<GameObject>> _active = new Dictionary<GameObject, HashSet<GameObject>>();

        private Factory _factory;
        
        public ViewPool(DiContainer container)
        {
            _factory = new Factory(container);
        }   
        
        public void Push(GameObject gameObject)
        {
            if (_active.TryGetValue(gameObject, out var  allACtive))
            {
                allACtive.Remove(gameObject);
            }
            if(_pushed.TryGetValue(gameObject, out var pushed) == false)
            {
                pushed = new Queue<GameObject>();
                _pushed.Add(gameObject, pushed);
            }
            pushed.Enqueue(gameObject);
        }
        
        public GameObject Pop(GameObject gameObject, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (_pushed.TryGetValue(gameObject, out var pushed) == false)
            {
                pushed = new Queue<GameObject>();
                _pushed.Add(gameObject, pushed);
            }

            if (pushed.TryDequeue(out var objectToPop))
            {
                objectToPop.transform.position = position;
                objectToPop.transform.rotation = rotation;
                objectToPop.transform.SetParent(parent);
            }else
            {
                objectToPop = _factory.Create(gameObject, position, rotation, parent);
            }
            
            if (_active.TryGetValue(gameObject, out var allActive) == false)
            {
                allActive = new HashSet<GameObject>();
                _active.Add(gameObject, allActive);
            }
            allActive.Add(objectToPop);
            
            return objectToPop;
        }
        


        public class Factory
        {
            private DiContainer _container;

            public Factory(DiContainer container)
            {
                _container = container;
            }

            public GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
            {
                return _container.InstantiatePrefab(prefab, position, rotation, parent);
            }
        }
    }
}
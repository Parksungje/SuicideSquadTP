using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Agents
{
    public abstract class Agent : MonoBehaviour
    {
        protected Dictionary<Type, IComponent> _components;

        public virtual void Awake()
        {
            //_components = new Dictionary<Type, IComponent>();
            AddComponent();
            InitializeComponents();
        }

        private void AddComponent()
            => _components = GetComponentsInChildren<IComponent>()
                .ToDictionary(compo => compo.GetType());

        protected virtual void OnDestroy()
        {

        }

        private void InitializeComponents()
        {
            _components.Values.ToList()
                .ForEach(compo => compo.Initialize(this));
        }

        public T GetCompo<T>()
        {
            // 이 메서드는 나중에 변경되니까 일단은 함수로 만들어둔다.
            if (_components.TryGetValue(typeof(T), out IComponent component))
                return (T)component;

            foreach (var compo in _components.Values)
            {
                if (compo is T t)
                    return t;
            }

            return default;
        }
    }
}
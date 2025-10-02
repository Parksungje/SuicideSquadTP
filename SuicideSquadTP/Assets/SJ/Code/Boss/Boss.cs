using System;
using System.Collections.Generic;
using System.Linq;
using SJ.Code.Core;
using UnityEngine;
using UnityEngine.Events;

namespace SJ.Code.Boss
{
    public class Boss : MonoBehaviour
    {
        public bool IsDead { get; set; }
        public UnityEvent OnDeadEvent;
        
        protected Dictionary<Type, IBossComponent> _components;

        protected virtual void Awake()
        {
            _components = new Dictionary<Type, IBossComponent>();
            AddComponents();
            InitializeComponents();
            AfterInitialize();
        }
        
        protected virtual void AddComponents()
        {
            GetComponentsInChildren<IBossComponent>().ToList()
                .ForEach(component => _components.Add(component.GetType(), component));
        }

        protected virtual void InitializeComponents()
        {
            _components.Values.ToList().ForEach(component => component.Initialize(this));
        }
        
        protected virtual void AfterInitialize()
        {
            _components.Values.OfType<IAfterInitialize>()
                .ToList().ForEach(compo => compo.AfterInitialize());
        }

        public T GetCompo<T>() where T : IBossComponent
            => (T)_components.GetValueOrDefault(typeof(T));
        public IBossComponent GetCompo(Type type)
            => _components.GetValueOrDefault(type);

        public void DestroyEntity()
        {
            Destroy(gameObject);
        }
    }
}

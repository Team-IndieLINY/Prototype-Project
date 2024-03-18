using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using IndieLINY.Singleton;

namespace IndieLINY.Event
{
    public class CollisionInteraction : MonoBehaviour
    {
        public event Action<ActorContractInfo> OnContractActor;
        public event Action<ObjectContractInfo> OnContractObject;
        public event Action<ClickContractInfo> OnContractClick;
        public BaseContractInfo ContractInfo { get; private set; }

        public void SetContractInfo(BaseContractInfo info)
        {
            ContractInfo = info;
        }

        public bool IsEnabled
        {
            get => enabled;
            set => enabled = value;
        }

        public T GetContractInfoOrNull<T>() where T : BaseContractInfo
            => ContractInfo as T;

        public bool TryGetContractInfo<T>(out T info) where T : BaseContractInfo
        {
            info = GetContractInfoOrNull<T>();
            return info is not null;
        }

        public void Activate(BaseContractInfo info)
        {
            switch (info)
            {
                case ActorContractInfo actorContractInfo:
                    OnContractActor?.Invoke(actorContractInfo);
                    break;
                case ClickContractInfo clickContractInfo:
                    OnContractClick?.Invoke(clickContractInfo);
                    break;
                case ObjectContractInfo objectContractInfo:
                    OnContractObject?.Invoke(objectContractInfo);
                    break;
                default:
                    //throw new ArgumentOutOfRangeException(nameof(ContractInfo));
                    break;
            }
        }

        public void ClearContractEvent()
        {
            OnContractActor = null;
            OnContractObject = null;
            OnContractClick = null;
        }

        private CollisionBridge _collisionBridge;
        private void Start()
        {
            _collisionBridge = Singleton.Singleton.GetSingleton<EventController>().GetBridge<CollisionBridge>();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!IsEnabled) return;
            if (other.gameObject.TryGetComponent<CollisionInteraction>(out var com))
            {
                if (!com.IsEnabled) return;
                Activate(com.ContractInfo);

                _collisionBridge.Push(this, com);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!IsEnabled) return;
            if (other.gameObject.TryGetComponent<CollisionInteraction>(out var com))
            {
                if (!com.IsEnabled) return;
                Activate(com.ContractInfo);

                _collisionBridge.Push(this, com);
            }
        }
    }
}
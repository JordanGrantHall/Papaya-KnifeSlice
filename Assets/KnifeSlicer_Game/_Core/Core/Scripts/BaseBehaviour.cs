using KnifeSlicer.Core.Events;
using UnityEngine;

namespace KnifeSlicer.Core
{
    public class BaseBehaviour : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            ServiceBus.RegisterInstance(this);
        }

        protected virtual void OnDisable()
        {
            ServiceBus.UnregisterInstance(this);
        }

        public void Publish(string topic, params object[] args)
        {
            Debug.Log("Publishing: " + topic);
            ServiceBus.Publish(topic, this, args);
        }
    }
}
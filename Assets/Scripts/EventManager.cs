using UnityEngine;
using System;
using System.Collections.Generic;

namespace Insection
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager instance; // Singleton instance

        // Dictionary to map event names (strings) to corresponding events
        private Dictionary<string, Action> eventDictionary = new Dictionary<string, Action>();

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Method to subscribe a function to a specific event name
        public void SubscribeToEvent(string eventName, Action function)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] += function;
            }
            else
            {
                eventDictionary[eventName] = function;
            }
        }

        // Method to unsubscribe a function from a specific event name
        public void UnsubscribeFromEvent(string eventName, Action function)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName] -= function;
            }
        }

        // Method to trigger an event based on the event name
        public void TriggerEvent(string eventName)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName]?.Invoke();
            }
        }
    }
}
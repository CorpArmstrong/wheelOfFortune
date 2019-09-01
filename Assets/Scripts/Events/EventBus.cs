using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Events
{
    public class EventBus : MonoBehaviour
    {
        private static EventBus _eventBus;

        private Dictionary<string, UnityEvent> _eventDictionary;
        private Dictionary<string, Action<int>> _eventParamsDictionary;

        public static EventBus Instance
        {
            get
            {
                if (_eventBus == null)
                {
                    _eventBus = FindObjectOfType(typeof(EventBus)) as EventBus;

                    if (_eventBus == null)
                    {
                        Debug.LogError("There needs to be at least one active EventBus script on a GameObject in your scene.");
                    }
                    else
                    {
                        _eventBus.Initialize();
                    }
                }

                return _eventBus;
            }
        }

        private void Initialize()
        {
            if (_eventDictionary == null)
            {
                _eventDictionary = new Dictionary<string, UnityEvent>();
            }

            if (_eventParamsDictionary == null)
            {
                _eventParamsDictionary = new Dictionary<string, Action<int>>();
            }
        }

        public static void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;

            if (Instance._eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                Instance._eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, UnityAction listener)
        {
            if (_eventBus != null && Instance._eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(string eventName)
        {
            if (Instance._eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        public static void StartListening(string eventName, Action<int> listener)
        {
            if (Instance._eventParamsDictionary.TryGetValue(eventName, out Action<int> thisEvent))
            {
                thisEvent += listener;
            }
            else
            {
                thisEvent = new Action<int>(listener);
                Instance._eventParamsDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(string eventName, Action<int> listener)
        {
            if (_eventBus != null && Instance._eventParamsDictionary.TryGetValue(eventName, out Action<int> thisEvent))
            {
                thisEvent -= listener;
            }
        }

        public static void TriggerEvent(string eventName, int param)
        {
            if (Instance._eventParamsDictionary.TryGetValue(eventName, out Action<int> thisEvent))
            {
                thisEvent.Invoke(param);
            }
        }
    }
}

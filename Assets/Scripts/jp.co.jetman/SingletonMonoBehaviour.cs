using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace jp.co.jetman
{
    public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static volatile T _instance;

        private static object syncObj = new object();

        public static T instance
        {
            get
            {
                if (applicationIsQuitting)
                    return null;

                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>() as T;

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        return _instance;
                    }
                    if (_instance == null)
                    {
                        lock (syncObj)
                        {
                            GameObject singleton = new GameObject();
                            singleton.name = typeof(T).ToString() + " (singleton)";
                            _instance = singleton.AddComponent<T>();
                            DontDestroyOnLoad(singleton);
                        }
                    }

                }
                return _instance;
            }
            private set
            {
                _instance = value;
            }
        }

        static bool applicationIsQuitting = false;

        void OnApplicationQuit()
        {
            applicationIsQuitting = true;
        }

        void OnDestroy()
        {
            instance = null;
        }

        protected SingletonMonoBehaviour() { 
        }
    }

}

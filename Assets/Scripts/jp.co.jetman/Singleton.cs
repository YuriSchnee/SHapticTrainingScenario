using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace jp.co.jetman
{
    public class Singleton<T> where T : class, new()
    {
        private static readonly T _instance = new T();

        public static T Instance {
            get {
                return _instance;
            }
        }

        protected Singleton() { }
    }
}

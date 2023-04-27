using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieAttack.Exceptions
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject newGo = new GameObject();
                        _instance = newGo.AddComponent<T>();
                        newGo.name = "" + typeof(T).Name;
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            _instance = this as T;
        }
    }
}
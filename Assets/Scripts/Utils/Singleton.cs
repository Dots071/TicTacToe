using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Utils
{
    /*  SUMMARY
     *  A generic abstract class to inherit the Singleton behavior. 
    */
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance;
        public static T Instance
        {
            get { return instance; }
        }

        public static bool isInitialized
        {
            get { return instance != null; }
        }

        protected virtual void Awake()
        {
            if (instance != null)
            {
                Debug.LogError("[Singleton] trying to instantiate a second instance in a singleton.");
            }
            else
            {
                instance = (T)this;
            }
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }
    }

}

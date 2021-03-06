using UnityEngine;

namespace Woska.Core
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance;
        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = (T) FindObjectOfType(typeof(T));
 
                    if (instance == null)
                    {
                        Debug.LogError("Singleton error");
                    }
                }
                return instance;
            }
        }
    }
}
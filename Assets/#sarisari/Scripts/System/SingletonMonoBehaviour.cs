using System;
using Sirenix.OdinInspector;
using UnityEngine;
public abstract class SingletonMonoBehaviour<T> : SerializedMonoBehaviour  where T : SerializedMonoBehaviour 
{
   private static T instance;
   public static T Instance
   {
       get
       {
           if (instance == null)
           {
               Type t = typeof(T);
               instance = (T)FindObjectOfType(t);
               if (instance == null)
               {
                   Debug.LogError(t + " をアタッチしているGameObjectはありません");
               }
           }
           return instance;
       }
   }
   protected virtual void Awake()
   {
       CheckInstance();
   }
   protected bool CheckInstance()
   {
       if (instance == null)
       {
           instance = this as T;
           return true;
       }
       else if (Instance == this)
       {
           return true;
       }
       Destroy(this);
       return false;
   }
}
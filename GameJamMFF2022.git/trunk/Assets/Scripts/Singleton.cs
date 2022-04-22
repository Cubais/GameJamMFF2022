using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T innerInstance;

    public static T instance
    {
        get
        {
            CreateSingletonInstance();
            return innerInstance;
        }
    }

    // Manual instance assign if object has to be disabled in 'Awake' so 'FindObjectOfType' wouldn't find it then
    public static void CreateSingletonInstance()
    {
        if (!innerInstance)
            innerInstance = FindObjectOfType<T>();
    }
}
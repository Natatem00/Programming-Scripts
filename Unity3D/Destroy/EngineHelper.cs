using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineExtensions : Behaviour
{
    static Queue<Object> objectsToDestroy = new Queue<Object>();
    static Coroutine currentCoroutine = null;

    static Queue<Object> spareToDestroy = new Queue<Object>();
    static Coroutine spareCoroutine = null;

    public static void Destroy(Object obj, MonoBehaviour mono)
    {
        if (objectsToDestroy.Count < 500)
        {
            objectsToDestroy.Enqueue(obj);
            if (currentCoroutine == null)
            {
                currentCoroutine = mono.StartCoroutine(Destroing());
            }
        }
        else
        {
            spareToDestroy.Enqueue(obj);
            if (spareCoroutine == null)
            {
                spareCoroutine = mono.StartCoroutine(SpareDestroing());
            }
        }
    }

    static IEnumerator Destroing()
    {
        while(objectsToDestroy.Count > 0)
        {
            Object obj = objectsToDestroy.Dequeue();
            if (obj != null)
            {
                Object.Destroy(obj);
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
        currentCoroutine = null;
    }

    static IEnumerator SpareDestroing()
    {
        while (spareToDestroy.Count > 0)
        {
            Object obj = spareToDestroy.Dequeue();
            if (obj != null)
            {
                Object.Destroy(obj);
            }
            yield return new WaitForSecondsRealtime(0.001f);
        }
        spareCoroutine = null;
    }
}

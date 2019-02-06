using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SOPool", menuName = "SOPRO/Pool")]
[Serializable]
public class SOPool : ScriptableObject
{
    public int ElementsStored { get { return elements.Count; } }

    [Tooltip("Pool prefab instance")]
    public GameObject Prefab;
    /// <summary>
    /// True if you want the pool to check whenever pooled objects have been destroyed for a scene change or for other reasons
    /// </summary>
    [Tooltip("True checks whenever pooled objects have been destroyed for a scene change or for other reasons")]
    public bool PersistentPoolInScenes = true;

    private Queue<GameObject> elements = new Queue<GameObject>();

    /// <summary>
    /// Recycles the given instance
    /// </summary>
    /// <param name="toRecycle">object to recycle</param>
    public void Recycle(GameObject toRecycle)
    {
        elements.Enqueue(toRecycle);
        if (toRecycle.activeSelf)
        {
            toRecycle.SetActive(false);
        }
    }
    /// <summary>
    /// Recycles the given instance. The object will not be disabled
    /// </summary>
    /// <param name="toRecycle">object to recycle</param>
    public void DirectRecycle(GameObject toRecycle)
    {
        elements.Enqueue(toRecycle);
    }
    /// <summary>
    /// Recycles the given instance
    /// </summary>
    /// <param name="toRecycle">object to recycle</param>
    /// <param name="onRecycle">action called on element after deactivation</param>
    public void Recycle(GameObject toRecycle, Action<GameObject> onRecycle)
    {
        elements.Enqueue(toRecycle);
        if (toRecycle.activeSelf)
        {
            toRecycle.SetActive(false);
        }

        onRecycle(toRecycle);
    }
    /// <summary>
    /// Requests an element from the pool.
    /// </summary>
    /// <param name="onGet">action called on element before activation</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <returns>the requested element instance</returns>
    public GameObject Get(Action<GameObject> onGet, out int nullObjsRemoved)
    {
        nullObjsRemoved = 0;
        bool instantiated;
        GameObject res = elements.Count == 0 ? GameObject.Instantiate(Prefab) : (PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated) : elements.Dequeue());
        onGet(res);
        if (!res.activeSelf)
        {
            res.SetActive(true);
        }
        return res;
    }
    /// <summary>
    /// Requests an element from the pool.
    /// </summary>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <returns>the requested element instance</returns>
    public GameObject Get(out int nullObjsRemoved)
    {
        nullObjsRemoved = 0;
        bool instantiated;
        GameObject res = elements.Count == 0 ? GameObject.Instantiate(Prefab) : (PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated) : elements.Dequeue());
        if (!res.activeSelf)
        {
            res.SetActive(true);
        }
        return res;
    }
    /// <summary>
    /// Requests an element from the pool. The object will not be enabled
    /// </summary>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <returns>the requested element instance</returns>
    public GameObject DirectGet(out int nullObjsRemoved)
    {
        nullObjsRemoved = 0;
        bool instantiated;
        return elements.Count == 0 ? GameObject.Instantiate(Prefab) : (PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated) : elements.Dequeue());
    }
    /// <summary>
    /// Requests an element from the pool. The object will not be enabled
    /// </summary>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <param name="hasBeenParented">true if obj was instantiated and parent setted, false otherwise</param>
    /// <returns>the requested element instance</returns>
    public GameObject DirectGet(Transform parent, out int nullObjsRemoved, out bool hasBeenParented)
    {
        nullObjsRemoved = 0;
        hasBeenParented = false;
        if (elements.Count == 0)
        {
            hasBeenParented = true;
            return GameObject.Instantiate(Prefab, parent);
        }
        else
        {
            return PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out hasBeenParented, parent) : elements.Dequeue();
        }
    }
    /// <summary>
    /// Requests an element from the pool. The object will not be enabled
    /// </summary>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="position">object position</param>
    /// <param name="rotation">object rotation</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <param name="hasBeenParentedAndPositioned">true if obj was instantiated and parent and pos/rot have been setted, false otherwise</param>
    /// <returns>the requested element instance</returns>
    public GameObject DirectGet(Transform parent, Vector3 position, Quaternion rotation, out int nullObjsRemoved, out bool hasBeenParentedAndPositioned)
    {
        nullObjsRemoved = 0;
        hasBeenParentedAndPositioned = false;
        if (elements.Count == 0)
        {
            hasBeenParentedAndPositioned = true;
            return GameObject.Instantiate(Prefab, position, rotation, parent);
        }
        else
        {
            return PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out hasBeenParentedAndPositioned, parent, position, rotation) : elements.Dequeue();
        }
    }
    /// <summary>
    /// Requests an element from the pool.
    /// </summary>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="onGet">action called on element before activation</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <param name="parentAlways">false to set parent only when instantiating obj, true to set parent always</param>
    /// <returns>the requested element instance</returns>
    public GameObject Get(Transform parent, Action<GameObject> onGet, out int nullObjsRemoved, bool parentAlways = false)
    {
        nullObjsRemoved = 0;
        GameObject res;
        if (elements.Count == 0)
        {
            res = GameObject.Instantiate(Prefab, parent);
        }
        else
        {
            bool instantiated = false;
            res = PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated, parent) : elements.Dequeue();
            if (!instantiated && parentAlways)
                res.transform.parent = parent;
        }
        onGet(res);
        if (!res.activeSelf)
        {
            res.SetActive(true);
        }
        return res;
    }
    /// <summary>
    /// Requests an element from the pool.
    /// </summary>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <param name="parentAlways">false to set parent only when instantiating obj, true to set parent always</param>
    /// <returns>the requested element instance</returns>
    public GameObject Get(Transform parent, out int nullObjsRemoved, bool parentAlways = false)
    {
        nullObjsRemoved = 0;
        GameObject res;
        if (elements.Count == 0)
        {
            res = GameObject.Instantiate(Prefab, parent);
        }
        else
        {
            bool instantiated = false;
            res = PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated, parent) : elements.Dequeue();
            if (!instantiated && parentAlways)
                res.transform.parent = parent;
        }
        if (!res.activeSelf)
        {
            res.SetActive(true);
        }
        return res;
    }
    /// <summary>
    /// Requests an element from the pool.
    /// </summary>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="position">object position</param>
    /// <param name="rotation">object rotation</param>
    /// <param name="onGet">action called on element before activation</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <param name="parentAlways">false to set parent only when instantiating obj, true to set parent always</param>
    /// <returns>the requested element instance</returns>
    public GameObject Get(Transform parent, Vector3 position, Quaternion rotation, Action<GameObject> onGet, out int nullObjsRemoved, bool parentAlways = false)
    {
        nullObjsRemoved = 0;
        GameObject res = null;
        if (elements.Count == 0)
        {
            res = GameObject.Instantiate(Prefab, position, rotation, parent);
        }
        else
        {
            bool instantiated = false;
            res = PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated, parent, position, rotation) : elements.Dequeue();
            if (!instantiated)
            {
                res.transform.SetPositionAndRotation(position, rotation);
                if (parentAlways)
                    res.transform.parent = parent;
            }
        }
        onGet(res);
        if (!res.activeSelf)
        {
            res.SetActive(true);
        }
        return res;
    }
    /// <summary>
    /// Requests an element from the pool.
    /// </summary>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="position">object position</param>
    /// <param name="rotation">object rotation</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <param name="parentAlways">false to set parent only when instantiating obj, true to set parent always</param>
    /// <returns>the requested element instance</returns>
    public GameObject Get(Transform parent, Vector3 position, Quaternion rotation, out int nullObjsRemoved, bool parentAlways = false)
    {
        nullObjsRemoved = 0;
        GameObject res = null;
        if (elements.Count == 0)
        {
            res = GameObject.Instantiate(Prefab, position, rotation, parent);
        }
        else
        {
            bool instantiated = false;
            res = PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated, parent, position, rotation) : elements.Dequeue();
            if (!instantiated)
            {
                res.transform.SetPositionAndRotation(position, rotation);
                if (parentAlways)
                    res.transform.parent = parent;
            }
        }
        if (!res.activeSelf)
        {
            res.SetActive(true);
        }
        return res;
    }

    /// <summary>
    /// Requests an element from the pool.
    /// </summary>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="position">object position</param>
    /// <param name="nullObjsRemoved">Number of objs removed when PersistentPoolInScenes is true</param>
    /// <param name="parentAlways">false to set parent only when instantiating obj, true to set parent always</param>
    /// <returns>the requested element instance</returns>
    public GameObject Get(Transform parent, Vector3 position, out int nullObjsRemoved, bool parentAlways = false)
    {
        nullObjsRemoved = 0;
        GameObject res = null;
        if (elements.Count == 0)
        {
            res = GameObject.Instantiate(Prefab, position, Prefab.transform.rotation, parent);
        }
        else
        {
            bool instantiated = false;
            res = PersistentPoolInScenes ? GetRemoveNullRefs(out nullObjsRemoved, out instantiated, parent, position, Prefab.transform.rotation) : elements.Dequeue();
            if (!instantiated)
            {
                res.transform.SetPositionAndRotation(position, Prefab.transform.rotation);
                if (parentAlways)
                    res.transform.parent = parent;
            }
        }
        if (!res.activeSelf)
        {
            res.SetActive(true);
        }
        return res;
    }
    /// <summary>
    /// Clears the pool invoking an action on each element
    /// </summary>
    /// <param name="onDestroy">action invoked on each element in the pool</param>
    public void Clear(Action<GameObject> onDestroy)
    {
        while (elements.Count != 0)
        {
            GameObject obj = elements.Dequeue();
            if (obj)
            {
                onDestroy(obj);
                GameObject.Destroy(obj);
            }
        }
    }
    /// <summary>
    /// Clears the pool
    /// </summary>
    public void Clear()
    {
        while (elements.Count != 0)
        {
            GameObject obj = elements.Dequeue();
            if (obj)
                GameObject.Destroy(obj);
        }
    }
    /// <summary>
    /// Resizes the pool to the given length, invoking an action on each destroyed element (if there are any) and each created element (if there are any)
    /// </summary>
    /// <param name="onDestroy">action invoked on each destroyed element in the pool</param>
    /// <param name="parent">transform to use as the requested element parent.</param>
    /// <param name="position">object position</param>
    /// <param name="rotation">object rotation</param>
    /// <param name="onRecycle">action called on element after deactivation</param>
    /// <param name="value">target length</param>
    public void ReSize(uint value, Action<GameObject> onDestroy = null, Transform parent = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion), Action<GameObject> onRecycle = null)
    {
        while (elements.Count > value)
        {
            GameObject obj = elements.Dequeue();
            if (obj)
            {
                if (onDestroy != null)
                    onDestroy.Invoke(obj);
                GameObject.Destroy(obj);
            }
        }
        while (elements.Count < value)
        {
            GameObject obj = GameObject.Instantiate(Prefab, position, rotation, parent);
            if (obj.activeSelf)
            {
                obj.SetActive(false);
            }
            if (onRecycle != null)
                onRecycle.Invoke(obj);
            elements.Enqueue(obj);
        }
    }

    private GameObject GetRemoveNullRefs(out int nullObjsRemoved, out bool instantiated, Transform parent = null, Vector3 position = default(Vector3), Quaternion rotation = default(Quaternion))
    {
        //Check needed to remove null objects. Objects will be automatically destroyed when changing scenes
        nullObjsRemoved = -1;
        instantiated = false;
        GameObject obj = null;

        do
        {
            obj = elements.Dequeue();
            nullObjsRemoved++;
        }
        while (!obj && elements.Count > 0);

        if (!obj)
        {
            obj = GameObject.Instantiate(Prefab, position, rotation, parent);
            instantiated = true;
        }

        return obj;
    }
}


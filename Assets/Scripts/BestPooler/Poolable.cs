using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poolable : MonoBehaviour, ObjectPooler.IPoolable
{
    public GameObject prefab { get; set; }
}

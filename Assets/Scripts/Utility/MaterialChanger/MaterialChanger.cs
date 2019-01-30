using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Utility/MaterialChanger")]
public class MaterialChanger : ScriptableObject
{
    public List<GameObject> ObjectToChangeMaterial;
    public Material MaterialWithChange;

    public void ChangeMaterial()
    {
        for (int i = 0; i < ObjectToChangeMaterial.Count; i++)
        {
            ObjectToChangeMaterial[i].GetComponent<Renderer>().material = MaterialWithChange;
        }
    }
}

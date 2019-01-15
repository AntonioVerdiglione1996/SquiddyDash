using UnityEngine;
public abstract class IIndexable : MonoBehaviour {

    public int Index { get; set; }
    public int GetIndex()
    {
        return this.Index;
    }
    public void SetIndex(int index)
    {
        this.Index = index;
    }
}
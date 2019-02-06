using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public enum EConsumeInput
{
    None,
    ConsumeUIOnly,
    Consume
}
public abstract class IClickableUI : MonoBehaviour
{
    public RectTransform Self;

    public CustomClickInput Input
    {
        get
        {
            if(!input)
            {
                FindObjectOfType<CustomClickInput>();
            }
            return input;
        }
    }

    public EConsumeInput ConsumeClickDown = EConsumeInput.Consume;
    public EConsumeInput ConsumeClickUp = EConsumeInput.Consume;
    public EConsumeInput ConsumeClickPressed = EConsumeInput.Consume;

    protected LinkedListNode<IClickableUI> selfNode;
    [SerializeField]
    private CustomClickInput input;
    public abstract void OnClickPressed();
    public abstract void OnClickDown();
    public abstract void OnClickUp();
    protected virtual void OnEnable()
    {
        selfNode = Input.Ui.AddLast(this);
    }
    protected virtual void OnDisable()
    {
        if (selfNode != null)
        {
            Input.Ui.Remove(selfNode);
        }
    }
    protected virtual void OnValidate()
    {
        if(Self == null)
        {
            Self = GetComponent<RectTransform>();
        }
        if(input == null)
        {
            input = FindObjectOfType<CustomClickInput>();
        }
    }
}
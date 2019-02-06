using UnityEngine.Events;
using System;
public class CustomButton : IClickableUI
{
    public event Action OnClickedDown;
    public event Action OnClickedUp;
    public event Action OnClickedPressed;
    public UnityEvent OnClickDownEvent;
    public UnityEvent OnClickUpEvent;
    public UnityEvent OnClickPressedEvent;
    public override void OnClickDown()
    {
        OnClickDownEvent.Invoke();
        if(OnClickedDown != null)
        {
            OnClickedDown();
        }
    }

    public override void OnClickPressed()
    {
        OnClickPressedEvent.Invoke();
        if (OnClickedPressed != null)
        {
            OnClickedPressed();
        }
    }

    public override void OnClickUp()
    {
        OnClickUpEvent.Invoke();
        if (OnClickedUp != null)
        {
            OnClickedUp();
        }
    }
}
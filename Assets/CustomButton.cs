using UnityEngine.Events;
public class CustomButton : IClickableUI
{
    public UnityEvent OnClickDownEvent;
    public UnityEvent OnClickUpEvent;
    public UnityEvent OnClickPressedEvent;
    public override void OnClickDown()
    {
        OnClickDownEvent.Invoke();
    }

    public override void OnClickPressed()
    {
        OnClickPressedEvent.Invoke();
    }

    public override void OnClickUp()
    {
        OnClickUpEvent.Invoke();
    }
}
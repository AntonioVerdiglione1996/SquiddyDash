using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EClickState
{
    None = 0,
    Down = 1,
    Up = 2,
    Pressed = 3,
}
public class CustomClickInput : MonoBehaviour
{
    public BasicEvent OnClickDown;
    public BasicEvent OnClickUp;
    public BasicEvent OnClickPressed;
    public LinkedList<IClickableUI> Ui = new LinkedList<IClickableUI>();
    public int MouseButtonKey = 0;
    public int InputTouchId = 0;
    public EClickState ClickState { get; private set; }
    public Vector2 ScreenPosition { get; private set; }
    public Vector2 PreviousScreenPosition { get; private set; }
    private void Awake()
    {
        ClickState = EClickState.None;
    }
    void Update()
    {
        UpdateInput();

        if (ClickState == EClickState.None)
        {
            return;
        }

        if (!UpdateUI())
        {
            if (ClickState == EClickState.Down)
            {
                OnClickDown.Raise();
            }
            else if (ClickState == EClickState.Up)
            {
                OnClickUp.Raise();
            }
            else
            {
                OnClickPressed.Raise();
            }
        }
    }
    private bool UpdateUI()
    {
        LinkedListNode<IClickableUI> currentNode = Ui.First;
        LinkedListNode<IClickableUI> nextNode = null;

        bool inputConsumed = false;
        bool inputUiConsumed = false;

        while (currentNode != null && !inputUiConsumed)
        {
            nextNode = currentNode.Next;
            IClickableUI currentUi = currentNode.Value;

            if (currentUi)
            {
                if (IsUiClicked(ScreenPosition, currentUi))
                {
                    if (ClickState == EClickState.Down)
                    {
                        currentUi.OnClickDown();
                        inputConsumed = currentUi.ConsumeClickDown == EConsumeInput.Consume;
                        inputUiConsumed = inputConsumed || currentUi.ConsumeClickDown == EConsumeInput.ConsumeUIOnly;
                    }
                    else if (ClickState == EClickState.Up)
                    {
                        currentUi.OnClickUp();
                        inputConsumed = currentUi.ConsumeClickUp == EConsumeInput.Consume;
                        inputUiConsumed = inputConsumed || currentUi.ConsumeClickUp == EConsumeInput.ConsumeUIOnly;
                    }
                    else
                    {
                        currentUi.OnClickPressed();
                        inputConsumed = currentUi.ConsumeClickPressed == EConsumeInput.Consume;
                        inputUiConsumed = inputConsumed || currentUi.ConsumeClickPressed == EConsumeInput.ConsumeUIOnly;
                    }
                }
            }
            else
            {
                Ui.Remove(currentNode);
            }
            currentNode = nextNode;
        }

        return inputConsumed;
    }
    private void UpdateInput()
    {
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(MouseButtonKey))
        {
            ClickState = EClickState.Down;
            ScreenPosition = Input.mousePosition;
            //In this case screen position equals previous screen position
            PreviousScreenPosition = ScreenPosition;
        }
        else if (Input.GetMouseButtonUp(MouseButtonKey))
        {
            ClickState = EClickState.Up;
            PreviousScreenPosition = ScreenPosition;
            ScreenPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(MouseButtonKey))
        {
            ClickState = EClickState.Pressed;
            PreviousScreenPosition = ScreenPosition;
            ScreenPosition = Input.mousePosition;
        }
        else
        {
            ClickState = EClickState.None;
        }
#elif (UNITY_IOS || UNITY_ANDROID)
        Touch touch = Input.GetTouch(InputTouchId);
        if (touch.phase == TouchPhase.Began)
        {
            ClickState = EClickState.Down;
            ScreenPosition = touch.position;
            //In this case screen position equals previous screen position
            PreviousScreenPosition = ScreenPosition;
        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            ClickState = EClickState.Up;
            PreviousScreenPosition = ScreenPosition;
            ScreenPosition = touch.position;
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            ClickState = EClickState.Pressed;
            PreviousScreenPosition = ScreenPosition;
            ScreenPosition = touch.position;
        }
        else
        {
            ClickState = EClickState.None;
        }
#else
        throw new UnityException("Custom click not handled for current platform");
#endif
    }
    private bool IsUiClicked(Vector2 screenPosition, IClickableUI ui)
    {
        Vector3 size = ui.Self.rect.size;
        Vector2 pivot = ui.Self.pivot;
        size.z = 10f;
        Vector3 position = ui.Self.transform.position;
        position.x += size.x * (0.5f - pivot.x);
        position.y += size.y * (0.5f - pivot.y);
        position.z = 0f;
        Bounds bound = new Bounds(position, size);
        return bound.Contains(screenPosition);
    }
}

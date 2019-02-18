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
    public const float DefaultBoundsSizeZ = 10f;
    public BasicEvent OnClickDown;
    public BasicEvent OnClickUp;
    public BasicEvent OnClickPressed;
    public LinkedList<IClickableUI> Ui = new LinkedList<IClickableUI>();
    public int MouseButtonClickKey = 0;
    public int InputTouchClickId = 0;
    public EClickState ClickState { get; private set; }
    public Vector2 ScreenPosition { get; private set; }
    public Vector2 PreviousScreenPosition { get; private set; }
    public bool DebugEnabled = true;
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
            if (OnClickPressed && ClickState == EClickState.Pressed)
            {
                OnClickPressed.Raise();
            }
            else if (OnClickDown && ClickState == EClickState.Down)
            {
                OnClickDown.Raise();
            }
            else if (OnClickUp && ClickState == EClickState.Up)
            {
                OnClickUp.Raise();
            }
        }
    }
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!DebugEnabled)
        {
            return;
        }
        LinkedListNode<IClickableUI> currentNode = Ui.First;
        LinkedListNode<IClickableUI> nextNode = null;

        while (currentNode != null)
        {
            nextNode = currentNode.Next;
            IClickableUI currentUi = currentNode.Value;

            if (currentUi)
            {
                Vector3 size = currentUi.Self.rect.size;
                size.z = DefaultBoundsSizeZ;
                Vector2 pivot = currentUi.Self.pivot;
                Vector3 position = currentUi.Self.transform.position;
                position.x += size.x * (0.5f - pivot.x);
                position.y += size.y * (0.5f - pivot.y);
                position.z = 0f;
                Bounds bound = new Bounds(position, size);

                Gizmos.DrawWireCube(bound.center, bound.size);
            }
            currentNode = nextNode;
        }
#endif
    }
    private void UpdateInput()
    {
        PreviousScreenPosition = ScreenPosition;
#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(MouseButtonClickKey))
        {
            ClickState = EClickState.Down;
            ScreenPosition = Input.mousePosition;
            //In this case screen position equals previous screen position
            PreviousScreenPosition = ScreenPosition;
        }
        else if (Input.GetMouseButtonUp(MouseButtonClickKey))
        {
            ClickState = EClickState.Up;
            ScreenPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(MouseButtonClickKey))
        {
            ClickState = EClickState.Pressed;
            ScreenPosition = Input.mousePosition;
        }
        else
        {
            ClickState = EClickState.None;
        }
#elif (UNITY_IOS || UNITY_ANDROID)
        int touches = Input.touchCount;
        Touch touch = new Touch();
        if (touches > 0)
        {
            touch = Input.GetTouch(InputTouchClickId);
        }
        if (touches > 0 && touch.phase == TouchPhase.Began)
        {
            ClickState = EClickState.Down;
            ScreenPosition = touch.position;
            //In this case screen position equals previous screen position
            PreviousScreenPosition = ScreenPosition;
        }
        else if (touches > 0 && touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            ClickState = EClickState.Up;
            ScreenPosition = touch.position;
        }
        else if (touches > 0 && touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {
            ClickState = EClickState.Pressed;
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
    private bool UpdateUI()
    {
        LinkedListNode<IClickableUI> currentNode = Ui.First;
        LinkedListNode<IClickableUI> nextNode = null;

        bool inputUiConsumed = false;
        bool inputPlayerConsumed = false;

        while (currentNode != null && !inputUiConsumed)
        {
            nextNode = currentNode.Next;
            IClickableUI currentUi = currentNode.Value;

            if (currentUi)
            {
                if (IsUiClicked(ScreenPosition, currentUi))
                {
                    EConsumeInput consumeType = EConsumeInput.None;
                    if (ClickState == EClickState.Down)
                    {
                        currentUi.OnClickDown();
                        consumeType = currentUi.ConsumeClickDown;
                    }
                    else if (ClickState == EClickState.Up)
                    {
                        currentUi.OnClickUp();
                        consumeType = currentUi.ConsumeClickUp;
                    }
                    else
                    {
                        currentUi.OnClickPressed();
                        consumeType = currentUi.ConsumeClickPressed;
                    }
                    //if the following booleans are ever set to true they will remain true no matter what the consume type is
                    inputPlayerConsumed = inputPlayerConsumed || consumeType == EConsumeInput.ConsumePlayerOnly || consumeType == EConsumeInput.Consume;
                    inputUiConsumed = inputUiConsumed || consumeType == EConsumeInput.ConsumeUIOnly || consumeType == EConsumeInput.Consume;
                }
            }
            else
            {
                Ui.Remove(currentNode);
            }
            currentNode = nextNode;
        }

        return inputPlayerConsumed;
    }
    private bool IsUiClicked(Vector2 screenPosition, IClickableUI ui)
    {
        Vector3 size = ui.Self.rect.size;
        size.z = DefaultBoundsSizeZ;
        Vector2 pivot = ui.Self.pivot;
        Vector3 position = ui.Self.transform.position;
        position.x += size.x * (0.5f - pivot.x);
        position.y += size.y * (0.5f - pivot.y);
        position.z = 0f;
        Bounds bound = new Bounds(position, size);
        return bound.Contains(screenPosition);
    }
}

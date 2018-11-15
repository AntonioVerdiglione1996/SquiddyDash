using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

//the body of our scroll rect(needed for the mask Component)
//[RequireComponent(typeof(Image))]
//the mask in wich we want to see out images
//[RequireComponent(typeof(Mask))]
//the component for the level selection swipe
[RequireComponent(typeof(ScrollRect))]
//the component event trigger for various Events(Begin Drag,End Drag and so on)
[RequireComponent(typeof(EventTrigger))]
public class ReusableLevelSelection : MonoBehaviour
{
    //Set starting page index - starting from 0
    public int startingPage = 0;
    //Threshold time for fast swipe in seconds
    public float fastSwipeThresholdTime = 0.3f;
    //Threshold time for fast swipe in (unscaled) pixels
    public int fastSwipeThresholdDistance = 100;
    //How fast will page lerp to target position
    public float decelerationRate = 10f;
    //this is the value you use to divided your container max Width in order to set the center position of you icon
    public int CenterPositionValue = 2;
    //if u want your images to be distantiated than u have t kee this over 1 else u have to keep this as a percentage
    public float MultiplierOfScaling = 2;
  
    //Button to go to the previous page (optional)
    public GameObject prevButton;
    //Button to go to the next page (optional)
    public GameObject nextButton;
    //Sprite for unselected page (optional)
    public Sprite unselectedPage;
    //Sprite for selected page (optional)
    public Sprite selectedPage;
    //Container with page images (optional)
    public Transform pageSelectionIcons;

    public RawImage image;

    //---------------------PRIVATES------------------------------

    // fast swipes should be fast and short. If too long, then it is not fast swipe
    private int fastSwipeThresholdMaxLimit;

    private ScrollRect scrollRectComponent;
    private RectTransform scrollRectRect;
    private RectTransform container;

    private bool horizontal;

    // number of pages in container
    private int pageCount;
    private int currentPage;

    // whether lerping is in progress and target lerp position
    private bool lerp;
    private Vector2 lerpTo;

    // target position of every page
    private List<Vector2> pagePositions = new List<Vector2>();

    // in draggging, when dragging started and where it started
    private bool dragging;
    private float timeStamp;
    private Vector2 startPosition;

    // for showing small page icons
    private bool showPageSelection;
    private int previousPageSelectionIndex;
    // container with Image components - one Image for each page
    private List<Image> pageSelectionImages;

    //------------------------------------------------------------------------
    void Start()
    {
        scrollRectComponent = GetComponent<ScrollRect>();
        scrollRectRect = GetComponent<RectTransform>();
        container = scrollRectComponent.content;
        pageCount = container.childCount;

        // is it horizontal or vertical scrollrect
        if (scrollRectComponent.horizontal && !scrollRectComponent.vertical)
        {
            horizontal = true;
        }
        else if (!scrollRectComponent.horizontal && scrollRectComponent.vertical)
        {
            horizontal = false;
        }
        else
        {
            //Default set to horizontal.  
            horizontal = true;
        }

        lerp = false;

        // initialize the list of positions we will need i our setup
        SetPagePositions();
        //set your initial page(childcount 0 )
        SetPage(startingPage);
        //this is for indicators benith the scroll component
        InitPageSelection();
        SetPageSelection(startingPage);

        // prev and next buttons are not nulls
        if (nextButton)
            nextButton.GetComponent<Button>().onClick.AddListener(() => { NextScreen(); });

        if (prevButton)
            prevButton.GetComponent<Button>().onClick.AddListener(() => { PreviousScreen(); });
    }

    //------------------------------------------------------------------------
    void Update()
    {
        // if moving to target position
        if (lerp)
        {
            // prevent overshooting with values greater than 1
            float decelerate = Mathf.Min(decelerationRate * Time.deltaTime, 1f);
            container.anchoredPosition = Vector2.Lerp(container.anchoredPosition, lerpTo, decelerate);
            // time to stop lerping?
            if (Vector2.SqrMagnitude(container.anchoredPosition - lerpTo) < 0.25f)
            {
                // snap to target and stop lerping
                container.anchoredPosition = lerpTo;
                lerp = false;
                // clear also any scrollrect move that may interfere with our lerping
                scrollRectComponent.velocity = Vector2.zero;
            }

            // switches selection icon exactly to correct page
            if (showPageSelection)
            {
                SetPageSelection(GetNearestPage());
            }
        }
    }

    //------------------------------------------------------------------------
    private void SetPagePositions()
    {
        int width = 0;
        int height = 0;
        int offsetX = 0;
        int offsetY = 0;
        int containerWidth = 0;
        int containerHeight = 0;

        if (horizontal)
        {
            // screen width in pixels of scrollrect window
            width = (int)scrollRectRect.rect.width;
            // center position of all pages
            offsetX = width / CenterPositionValue;
            // total width
            containerWidth = width * pageCount;
            // limit fast swipe length - beyond this length it is fast swipe no more
            fastSwipeThresholdMaxLimit = width;
        }
        //vertical set
        else
        {
            height = (int)scrollRectRect.rect.height;
            offsetY = height / 2;
            containerHeight = height * pageCount;
            fastSwipeThresholdMaxLimit = height;
        }

        // set width of container
        Vector2 newSize = new Vector2(containerWidth, containerHeight);
        container.sizeDelta = newSize;
        Vector2 newPosition = new Vector2(containerWidth / 2, containerHeight / 2);
        container.anchoredPosition = newPosition;

        // delete any previous settings
        pagePositions.Clear();

        // iterate through all container childern and set their positions
        for (int i = 0; i < pageCount; i++)
        {
            RectTransform child = container.GetChild(i).GetComponent<RectTransform>();
            Vector2 childPosition;
            if (horizontal)
            {
                childPosition = new Vector2(i * width - containerWidth / MultiplierOfScaling + offsetX , 0f);
            }
            //vertical setup
            else
            {
                childPosition = new Vector2(0f, -(i * height - containerHeight / MultiplierOfScaling + offsetY ));
            }
            child.anchoredPosition = childPosition;
            pagePositions.Add(-childPosition);
        }
    }

    //------------------------------------------------------------------------
    private void SetPage(int PageIndex)
    {
        PageIndex = Mathf.Clamp(PageIndex, 0, pageCount - 1);
        container.anchoredPosition = pagePositions[PageIndex];
        currentPage = PageIndex;
    }

    //------------------------------------------------------------------------
    private void LerpToPage(int PageIndex)
    {
        PageIndex = Mathf.Clamp(PageIndex, 0, pageCount - 1);
        lerpTo = pagePositions[PageIndex];
        lerp = true;
        currentPage = PageIndex;
    }

    //------------------------------------------------------------------------
    private void InitPageSelection()
    {
        // page selection - only if defined sprites for selection icons
        showPageSelection = unselectedPage != null && selectedPage != null;
        if (showPageSelection)
        {
            // also container with selection images must be defined and must have exatly the same amount of items as pages container
            if (pageSelectionIcons == null || pageSelectionIcons.childCount != pageCount)
            {
                Debug.LogWarning("Different count of pages and selection icons - will not show page selection");
                showPageSelection = false;
            }
            else
            {
                previousPageSelectionIndex = -1;
                pageSelectionImages = new List<Image>();

                // cache all Image components into list
                for (int i = 0; i < pageSelectionIcons.childCount; i++)
                {
                    Image image = pageSelectionIcons.GetChild(i).GetComponent<Image>();
                    if (image == null)
                    {
                        Debug.LogWarning("Page selection icon at position " + i + " is missing Image component");
                    }
                    pageSelectionImages.Add(image);
                }
            }
        }
    }

    //------------------------------------------------------------------------
    private void SetPageSelection(int PageIndex)
    {
        // nothing to change
        if (previousPageSelectionIndex == PageIndex)
        {
            return;
        }

        // unselect old
        if (previousPageSelectionIndex >= 0)
        {
            pageSelectionImages[previousPageSelectionIndex].sprite = unselectedPage;
            pageSelectionImages[previousPageSelectionIndex].SetNativeSize();
        }

        // select new
        pageSelectionImages[PageIndex].sprite = selectedPage;
        pageSelectionImages[PageIndex].SetNativeSize();

        previousPageSelectionIndex = PageIndex;
    }

    //If we have button initialized and ------ clicked Right
    private void NextScreen()
    {
        LerpToPage(currentPage + 1);
    }
    //If we have button initialized and ------clicked Left
    private void PreviousScreen()
    {
        LerpToPage(currentPage - 1);
    }
    //return the index of the nearest content item inside the container
    private int GetNearestPage()
    {
        // based on distance from current position, find nearest page
        Vector2 currentPosition = container.anchoredPosition;

        float distance = float.MaxValue;
        int nearestPage = currentPage;

        for (int i = 0; i < pagePositions.Count; i++)
        {
            float testDist = Vector2.SqrMagnitude(currentPosition - pagePositions[i]);
            if (testDist < distance)
            {
                distance = testDist;
                nearestPage = i;
            }
        }

        return nearestPage;
    }

    //Events called with the event trigger component
    public void OnBeginDrag()
    {
        // if currently lerping, then stop it as user is draging
        lerp = false;
        // not dragging yet
        dragging = false;
    }
    public void OnEndDrag()
    {
        // how much was container's content dragged
        float difference;
        if (horizontal)
        {
            difference = startPosition.x - container.anchoredPosition.x;
        }
        //vertical setup
        else
        {
            difference = -(startPosition.y - container.anchoredPosition.y);
        }

        // test for fast swipe - swipe that moves only +/- 1 item
        if (Time.unscaledTime - timeStamp < fastSwipeThresholdTime &&
            Mathf.Abs(difference) > fastSwipeThresholdDistance &&
            Mathf.Abs(difference) < fastSwipeThresholdMaxLimit)
        {
            if (difference > 0)
            {
                NextScreen();
            }
            else
            {
                PreviousScreen();
            }
        }
        else
        {
            // if not fast time, look to which page we got to
            LerpToPage(GetNearestPage());
        }
        dragging = false;
    }

    //Not used yet 
    // this is for little icons under images if we want
    public void OnDrag()
    {
        if (!dragging)
        {
            // dragging started
            dragging = true;
            // save time - unscaled so pausing with Time.scale should not affect it
            timeStamp = Time.unscaledTime;
            // save current position of cointainer
            startPosition = container.anchoredPosition;
        }
        else
        {
            if (showPageSelection)
            {
                SetPageSelection(GetNearestPage());
            }
        }
    }
}


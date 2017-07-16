using UnityEngine;
using UnityEngine.UI;

public class ScrollPanel : MonoBehaviour
{

    [SerializeField]
    private RectTransform groupOfComponents;
    [SerializeField]
    private RectTransform center;
    [SerializeField]
    private float lerpSpeed = 5f;
    [SerializeField]
    private float scrollRectMinSpeed = 250f;

    private RectTransform[] components;
    private ScrollRect myScrollRect;

    private float[] distances;

    private bool dragging = false;

    private int closestComponentIndex;
    private int amountOfComponents;

    private float maxGroupDistance;
    private float minGroupDistance;

    private void Awake()
    {
        myScrollRect = GetComponent<ScrollRect>();
    }

    void Start()
    {
        amountOfComponents = groupOfComponents.childCount;
        components = new RectTransform[amountOfComponents];
        distances = new float[amountOfComponents];

        for (int i = 0; i < amountOfComponents; i++)
        {
            components[i] = groupOfComponents.GetChild(i).GetComponent<RectTransform>();
        }

        float distanceBetweenComponents = (components[1].anchoredPosition.x - components[0].anchoredPosition.x);
        minGroupDistance = distanceBetweenComponents / 2;
        maxGroupDistance = -distanceBetweenComponents * amountOfComponents + (-minGroupDistance);
        Debug.Log("Min:" + minGroupDistance + "   Max:" + maxGroupDistance);
        closestComponentIndex = 0;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch firstTouch = Input.GetTouch(0);
            if (firstTouch.phase == TouchPhase.Began)
            {
                dragging = true;
            }
            if (firstTouch.phase == TouchPhase.Ended)
            {
                dragging = false;
            }
        }

        if (!dragging)
        {
            //Debug.Log(myScrollRect.velocity.x);
            if (Mathf.Abs(myScrollRect.velocity.x) < scrollRectMinSpeed)
            {
                LerpToComponent(-components[closestComponentIndex].GetComponent<RectTransform>().anchoredPosition.x);
                LevelMaster.ins.CurrentLevelIndex = closestComponentIndex;
            }
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < amountOfComponents; i++)
        {
            distances[i] = Mathf.Abs(center.GetComponent<RectTransform>().position.x - components[i].GetComponent<RectTransform>().position.x);
        }

        float minDistance = Mathf.Min(distances);

        for (int i = 0; i < amountOfComponents; i++)
        {
            if (minDistance == distances[i])
            {
                closestComponentIndex = i;
            }
        }

        if (groupOfComponents.anchoredPosition.x < maxGroupDistance || groupOfComponents.anchoredPosition.x > minGroupDistance)
        {
            myScrollRect.velocity = Vector2.zero;
        }
    }

    void LerpToComponent(float position)
    {
        float newX = Mathf.Lerp(groupOfComponents.anchoredPosition.x, position, Time.deltaTime * lerpSpeed);

        Vector2 newPosition = new Vector2(newX, groupOfComponents.anchoredPosition.y);
        groupOfComponents.anchoredPosition = newPosition;
    }

}

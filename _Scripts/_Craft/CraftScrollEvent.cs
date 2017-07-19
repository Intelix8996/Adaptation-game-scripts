using UnityEngine;
using UnityEngine.EventSystems;

public class CraftScrollEvent : MonoBehaviour,IScrollHandler {

    [SerializeField]
    [Range(1, 500)]
    private float scrollSensitivity = 50;

    [SerializeField]
    private Vector3 Origin;

    [SerializeField]
    private CraftEventHandler[] Crafts;

    private void Start()
    {
        Origin = transform.position;
    }

    public void OnScroll(PointerEventData data)
    {
        float currY = transform.position.y;

        transform.position += new Vector3(0, data.scrollDelta.y * scrollSensitivity, 0);
        Crafts = GetComponentsInChildren<CraftEventHandler>();

        if (transform.position.y < Origin.y)
            transform.position = Origin;

        if (Crafts[Crafts.Length - 1].transform.position.y >= 160)
            transform.position = new Vector3(Origin.x, currY, 0);
    }
}

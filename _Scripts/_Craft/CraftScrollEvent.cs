using UnityEngine;
using UnityEngine.EventSystems;

public class CraftScrollEvent : MonoBehaviour,IScrollHandler {

    [SerializeField]
    [Range(1, 500)]
    private float scrollSensitivity = 50;

    [SerializeField]
    private CraftEventHandler[] Crafts;

    public void OnScroll (PointerEventData data)
    {
        transform.position += new Vector3(0, data.scrollDelta.y * scrollSensitivity, 0);
        Crafts = GetComponentsInChildren<CraftEventHandler>();
        CraftEventHandler LastCraft = Crafts[Crafts.Length - 1];

        if (transform.position.y < Screen.height) transform.position = Vector3.zero + new Vector3(0, Screen.height, 0);
        if (LastCraft.transform.position.y >= 160) transform.position = new Vector3(0, 160 + abs(LastCraft.transform.localPosition.y), 0);
    }

    public static float abs (float val) { if (val >= 0) return val; else return -val; }
}

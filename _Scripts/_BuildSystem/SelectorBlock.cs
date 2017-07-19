using UnityEngine;
using UnityEngine.UI;

public class SelectorBlock : MonoBehaviour {

    [SerializeField]
    private Text Name;
    [SerializeField]
    private Text Description;
    [SerializeField]
    private Image Icon;

    [SerializeField]
    private BuildHandler Handler;

    private void Start()
    {
        Icon.sprite = Resources.Load("NullOnEmpty", typeof(Sprite)) as Sprite;
        Description.text = "";
        Name.text = "";
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            Icon.sprite = Resources.Load(Handler.BlockList[Handler.CurrentBlock].IconPath, typeof(Sprite)) as Sprite;
            Description.text = Handler.BlockList[Handler.CurrentBlock].Description;
            Name.text = Handler.BlockList[Handler.CurrentBlock].Name;
        }
    }
}

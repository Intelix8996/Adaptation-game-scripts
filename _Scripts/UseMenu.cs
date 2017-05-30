using UnityEngine;

public class UseMenu : MonoBehaviour
{
    
    [SerializeField]
    private Camera Cam;
    private Ray _Ray;
    private RaycastHit Hit = new RaycastHit();
    private GameObject _Crate;

    private void Start()
    {
        _Crate = GameObject.FindGameObjectWithTag("UseableItem");
    }

    private void Update()
    {
        _Ray = Cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetButtonDown("Fire2") && Physics.Raycast(_Ray, out Hit, 10))
        {
            if (Hit.collider.gameObject.GetComponent<Animator>() != null)
            {
                if (Hit.collider.tag == "UseableItem")
                {
                    Debug.Log("OK");
                    _Crate = Hit.collider.gameObject;
                    _Crate.GetComponent<Animator>().Play("Open");
                }
            }
            if (Hit.collider.tag != "UseableItem")
            {
                _Crate.GetComponent<Animator>().Play("Close");
            }
        }

        if (Input.GetButtonDown("Fire1") && Physics.Raycast(_Ray, out Hit, 10))
        {
            if (Hit.collider.tag == "UseableItem" && Hit.collider.gameObject.GetComponent<Item>())
            {
                foreach (InventorySocket ic in GetComponent<InventoryManadger>().Inventory)
                {
                    if (ic.Item.Id == Hit.collider.gameObject.GetComponent<Item>().IdLocal || ic.Item.Id == 0)
                    {
                        ic.Item = ItemLibrary._ItemGenerator.ItemList[Hit.collider.gameObject.GetComponent<Item>().IdLocal];
                        ic.Number += Hit.collider.gameObject.GetComponent<Item>().Amount;
                        Destroy(Hit.collider.gameObject);
                        break;
                    }
                }
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour {

    [SerializeField]
    private GameObject RootMan;

    private void Start()
    {
        RootMan = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        transform.position = RootMan.transform.position + new Vector3(0, 5.7f, -1.9f);

        if (Input.GetButtonDown("Fire2"))
        {
            Cursor.visible = false;
        }
        if (Input.GetButtonUp("Fire2"))
        {
            Cursor.visible = true;
        }
    }

}

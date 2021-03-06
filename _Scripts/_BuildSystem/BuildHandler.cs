﻿using UnityEngine;
using System.Collections.Generic;
using System;

public class BuildHandler : MonoBehaviour {

    public bool isBlueprintActive = false;
    [SerializeField]
    private bool isSet = false;

    [Header("")]
    public List<BlockBase> BlockList = new List<BlockBase>();
    public int CurrentBlock = 0;
    [SerializeField]
    private GameObject BlockDescriptionBlock;

    [Header("")]
    [SerializeField]
    private GameObject Foundation;
    [SerializeField]
    private GameObject PreviewBlock;
    [SerializeField]
    private GameObject[] Markers;

    [Header("")]
    public Material BuildAllowedMaterial;
    public Material BuildNotAllowedMaterial;

    public bool BuildAllowed = true;
    public bool isCursorOnMarker = false;

    [SerializeField]
    private Ray _Ray;
    [SerializeField]
    private Camera Cam;

    [SerializeField]
    float foundationsHeight = .2f;

    private RaycastHit Hit = new RaycastHit();

    public static BuildHandler _BlockLibrary;

    void Awake()
    {
        _BlockLibrary = this;
    }

    private void Start()
    {
        MakePreview(BlockList[CurrentBlock].Prefab);
        Markers = GameObject.FindGameObjectsWithTag("SocketMarker");
    }

    public void Update()
    {
        PreviewBlock.SetActive(isBlueprintActive);

        Markers = GameObject.FindGameObjectsWithTag("SocketMarker");

        foreach (GameObject GM in Markers)
        {
            GM.GetComponent<MeshRenderer>().enabled = isBlueprintActive;
            GM.GetComponent<BoxCollider>().enabled = isBlueprintActive;
        }

        _Ray = Cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(_Ray, out Hit, 100);

        if (isBlueprintActive && !isSet)
        {
            foundationsHeight += Input.mouseScrollDelta.y / 10;
            foundationsHeight = clampFoundationsHeight(foundationsHeight);

            if (Hit.collider.gameObject.tag == "BuildingBlock")
            {
                isCursorOnMarker = false;
                PreviewBlock.transform.position = Hit.point + new Vector3(1, .2f, 1);
            }
            if (Hit.collider.gameObject.tag == "SocketMarker" && !Hit.collider.gameObject.GetComponent<SocketMarker>().isSocketOccured)
            {
                isCursorOnMarker = true;
                PreviewBlock.transform.position = GetSetTransform(BlockList[CurrentBlock].Prefab, Hit);
            }
            else
            {
                isCursorOnMarker = false;
                PreviewBlock.transform.position = Hit.point + new Vector3(0, foundationsHeight, 0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !isSet && isBlueprintActive)
        {
            if (BuildAllowed && Hit.collider.gameObject.tag != "SocketMarker" && Hit.collider.gameObject.tag != "BuildingBlock")
            {
                SetBlock(BlockList[CurrentBlock].Prefab);
                MakePreview(BlockList[CurrentBlock].Prefab);
            }else if (BuildAllowed && Hit.collider.gameObject.tag == "SocketMarker" && Hit.collider.gameObject.tag != "BuildingBlock")
            {
                Set(BlockList[CurrentBlock].Prefab, Hit);
            }
        }

        if (isBlueprintActive && Input.GetKeyDown(KeyCode.Mouse1))
        {
            BlockDescriptionBlock.SetActive(!BlockDescriptionBlock.activeSelf);
        }

        if (Input.mouseScrollDelta.y != 0 && BlockDescriptionBlock.activeSelf)
        {
            CurrentBlock += Convert.ToInt16(Input.mouseScrollDelta.y);
            CurrentBlock = clampBlockPointer(CurrentBlock);
            Destroy(PreviewBlock);
            MakePreview(BlockList[CurrentBlock].Prefab); 
        }
    }

    private void MakePreview(GameObject PreviewOrigin)
    {
        PreviewBlock = Instantiate(BlockList[CurrentBlock].Prefab, Hit.point, Quaternion.identity) as GameObject;
        PreviewBlock.layer = 2;
        Destroy(PreviewBlock.GetComponent<Rigidbody>());

        BoxCollider[] BC = PreviewBlock.GetComponentsInChildren<BoxCollider>();

        BC[0].gameObject.AddComponent<CollisionChecker>();
        BC[0].isTrigger = true;

        for (int i = 0; i < BC.Length; ++i)
        {
            if (BC[i].tag == "SocketMarker")
                Destroy(BC[i].gameObject);
            else
            {
                BC[i].gameObject.layer = 2;

                Renderer[] _R = BC[i].GetComponentsInChildren<Renderer>();

                foreach (Renderer R in _R)
                {
                    R.material = null;
                    R.material = BuildAllowedMaterial;
                }

                _R = null;
            }
        }

        BC = null;
    }

    private void SetBlock (GameObject Block)
    {
        isSet = true;
        Destroy(PreviewBlock);
        GameObject Block_Inst = Instantiate(BlockList[CurrentBlock].Prefab, Hit.point + new Vector3(0, foundationsHeight, 0), Quaternion.identity) as GameObject;
        Block_Inst.GetComponent<Rigidbody>().isKinematic = true;
        isSet = false;
    }

    private GameObject Take(RaycastHit hitInfo)
    {
        GameObject ActiveItem;

        ActiveItem = hitInfo.collider.gameObject;

        return ActiveItem;
    }

    private void Set(GameObject ActiveItem, RaycastHit hitInfo)
    {
        if (!hitInfo.collider.gameObject.GetComponent<SocketMarker>().isSocketOccured)
        {
            Vector3 itemGlobalTransform = Vector3.zero;

            if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Top")
                itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform + new Vector3(0, ActiveItem.transform.localScale.y / 2, 0);
            else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Bottom")
                itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform - new Vector3(0, ActiveItem.transform.localScale.y / 2, 0);
            else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Right")
                itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform + new Vector3(ActiveItem.transform.localScale.y / 2, 0, 0);
            else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Left")
                itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform - new Vector3(ActiveItem.transform.localScale.y / 2, 0, 0);
            else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Front")
                itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform + new Vector3(0, 0, ActiveItem.transform.localScale.y / 2);
            else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Back")
                itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform - new Vector3(0, 0, ActiveItem.transform.localScale.y / 2);

            GameObject B_Found = Instantiate(ActiveItem, itemGlobalTransform, Quaternion.identity) as GameObject;
            B_Found.GetComponent<Rigidbody>().isKinematic = true;
            ActiveItem = null;

            hitInfo.collider.gameObject.GetComponent<SocketMarker>().isSocketOccured = true;
        }
        else
        {
            Debug.LogError("Target socket is occured: " + hitInfo.collider.name);
        }
    }

    private Vector3 GetSetTransform(GameObject ActiveItem, RaycastHit hitInfo)
    {
        Vector3 itemGlobalTransform = Vector3.zero;

        if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Top")
            itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform + new Vector3(0, ActiveItem.transform.localScale.y / 2, 0);
        else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Bottom")
            itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform - new Vector3(0, ActiveItem.transform.localScale.y / 2, 0);
        else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Right")
            itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform + new Vector3(ActiveItem.transform.localScale.y / 2, 0, 0);
        else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Left")
            itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform - new Vector3(ActiveItem.transform.localScale.y / 2, 0, 0);
        else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Front")
            itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform + new Vector3(0, 0, ActiveItem.transform.localScale.y / 2);
        else if (hitInfo.collider.gameObject.GetComponent<SocketMarker>().Direction == "Back")
            itemGlobalTransform = hitInfo.collider.gameObject.GetComponent<SocketMarker>().setItemTransform - new Vector3(0, 0, ActiveItem.transform.localScale.y / 2);

        return itemGlobalTransform;
    }

    private float clampFoundationsHeight(float height)
    {
        if (height <= 0.1f)
            height = -0.09f;
        if (height >= 2f)
            height = 1.99f;

        return height;
    }

    private int clampBlockPointer(int CurrentBlockPointer)
    {
        if (CurrentBlockPointer <= 0)
            CurrentBlockPointer = 0;
        if (CurrentBlockPointer >= BlockList.Count - 1)
            CurrentBlockPointer = BlockList.Count - 1;

        return CurrentBlockPointer;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;


public class DragRotate : MonoBehaviour
{
    Vector3 mPrevPos = Vector3.zero;
    Vector3 mPosDelta = Vector3.zero;

    [SerializeField]
    InputActionAsset controllerInteraction;
    InputAction select;
    bool selected = false;

    [SerializeField]
    Transform leftController;

    private void Start()
    {
        var actionMap = controllerInteraction.FindActionMap("XRI LeftHand Interaction"); 
        select = actionMap.FindAction("Select");
        select.performed += context => selected = true;
        select.canceled += context => selected = false;
        select.Enable();

    }

    private void Update()
    {
        Debug.Log("Selected: "+ selected);
        if(selected)//Input.GetMouseButton(0))
        {
            Debug.Log("Previous Postion: "+mPrevPos);
            mPosDelta = leftController.position - mPrevPos; //Input.mousePosition - mPrevPos;
            //transform.RotateAround(transform.up,0,0)
            Debug.Log("New Angle: "+ -Vector3.Dot(mPosDelta, Camera.main.transform.right));
            transform.Rotate(transform.up, -Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
        }

        mPrevPos = leftController.position; //Input.mousePosition;
    }

}

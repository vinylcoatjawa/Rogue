using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
/* MAY BE OBSOLETE */

public class Movement : MonoBehaviour
{
    Transform _entityTransform;
    Vector3 _mouseWorldPos;
    Vector3 _mousePos;
    Vector3 _worldPos;
    
    void Awake()
    {
        _entityTransform = gameObject.transform;
        InputActions inputActions = new InputActions();
        inputActions.InputForTesting.Enable();

    }


    public void Move(){
        _entityTransform.position += new Vector3(10, 0, 0);
    }

    public void MousePos(){
        _mousePos = Input.mousePosition;
        _worldPos = Camera.main.ScreenToWorldPoint(_mousePos);
        Debug.Log($"mousepos is: {_mousePos} and worldPos is: {_worldPos}");
    }

    void ClickOnPlayer(InputAction.CallbackContext context){
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit raycastHit);
        if(raycastHit.transform.gameObject.tag == "Player"){
            Debug.Log("hit!");
        }
        
        //Debug.Log($"clicked and {context.phase}");
        //if(inputAction.)
    }


}

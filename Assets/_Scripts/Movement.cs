using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    Transform _entityTransform;
    Vector3 _mouseWorldPos;
    Vector3 _mousePos;
    Vector3 _worldPos;
    void Awake()
    {
        _entityTransform = gameObject.transform;
    }

    public void Move(){
        _entityTransform.position += new Vector3(10, 0, 0);
    }

    public void MousePos(){
        _mousePos = Input.mousePosition;
        _worldPos = Camera.main.ScreenToWorldPoint(_mousePos);
        Debug.Log($"mousepos is: {_mousePos} and worldPos is: {_worldPos}");
    }


}

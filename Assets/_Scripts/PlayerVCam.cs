using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerVCam : MonoBehaviour
{
    GameObject _player;
    CinemachineVirtualCamera _vCam;
    CinemachineTransposer _transposer;
    GameObject _floorMesh;
    CinemachineBrain _brain;
    void Awake()
    {
        _floorMesh = GameObject.FindGameObjectWithTag("FloorMesh");
    }
    void Start()
    {      
        _player = GameObject.FindGameObjectWithTag("Player");
        _vCam = GetComponent<CinemachineVirtualCamera>();
        _vCam.Follow = _player.transform;
        _vCam.LookAt = _player.transform;
        _transposer = _vCam.AddCinemachineComponent<CinemachineTransposer>();

        _brain = GetComponent<CinemachineBrain>();
        
        _transposer.m_FollowOffset = new Vector3(35, 75, -35);
    }

    // public void MousePos(){
    //     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

    //     if (Physics.Raycast(ray, out RaycastHit raycastHit)){
    //         _floorMesh.get
            
    //         Debug.Log(raycastHit.transform.position);

    //     }
        //Vector3 mousePos = Input.mousePosition;
        //Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //Vector3 worldPos = _brain.OutputCamera.ScreenToWorldPoint(mousePos);
        //Debug.Log($"mousepos is: {mousePos} and worldPos is: {worldPos}");
        //Debug.Log(mousePos + "  " + worldPos);
        //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x,mousePos.y, Camera.main.nearClipPlane)));
    

   
}

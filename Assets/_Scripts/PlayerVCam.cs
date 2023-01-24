using UnityEngine;
using Cinemachine;
/// <summary>
/// Virtual camera attached to the player
/// </summary>
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
        _transposer.m_XDamping = 0;
        _transposer.m_YDamping = 0;
        _transposer.m_ZDamping = 0;
    }
   
}

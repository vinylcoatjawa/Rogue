//using System.Numerics;
using UnityEngine.Events;
using UnityEngine;
/* this is a custom unity event which is raised when an scriptable object event is raised,
this will essentially be the unity event response whenever we raise a Vector3 event */
[System.Serializable] public class UnityVector3Event : UnityEvent<UnityEngine.Vector3> { }

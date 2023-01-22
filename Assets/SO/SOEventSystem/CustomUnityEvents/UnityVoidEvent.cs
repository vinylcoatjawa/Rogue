using UnityEngine.Events;

/* this is a custom unity event which is raised when an scriptable object event is raised, this will essentially be the unity event response whenever we raise a void event */
[System.Serializable] public class UnityVoidEvent : UnityEvent<Void> { }
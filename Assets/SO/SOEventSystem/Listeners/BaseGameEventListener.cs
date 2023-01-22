using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Base class from which all the custom scriptable object event system listeners will inherit from. EVery listener will need to know about the type it has
/// the custom event we created and the matching unity event, in a sense this is a mapping between the custom and the unity event
/// </summary>
/// <typeparam name="T">The type of the custom event</typeparam>
/// <typeparam name="E">The custom game event</typeparam>
/// <typeparam name="UER">The response unity event</typeparam>
public abstract class BaseGameEventListener<T, E, UER> : MonoBehaviour, IGameEventListener<T> where E : BaseGameEvent<T> where UER : UnityEvent<T>
{
    public E GameEvent { get { return gameEvent; } set { gameEvent = value; } }

    [SerializeField] private E gameEvent;
    [SerializeField] private UER unityEvemtResponse;
    

    /// <summary>
    /// We register the this instance of listener on enable
    /// </summary>
    private void OnEnable()
    {
        if(gameEvent == null) { return; }

        GameEvent.RegisterListener(this);
    }
    /// <summary>
    /// We unregister this instance the listener on disable
    /// </summary>
    private void OnDisable()
    {
        if(gameEvent == null) { return; }

        GameEvent.UnregisterListener(this);
    }
    /// <summary>
    /// We invoke the actual unity response here
    /// </summary>
    /// <param name="item">The type of the event</param>
    public void OnEventRaised(T item)
    {
        if(unityEvemtResponse != null)
        {
            unityEvemtResponse.Invoke(item);
        }
    }
}

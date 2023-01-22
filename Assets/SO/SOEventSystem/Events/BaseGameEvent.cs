using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// All scriptable object custom events are inheriting from this
/// </summary>
/// <typeparam name="T">Type of the game event, can be ant class</typeparam>
public abstract class BaseGameEvent<T> : ScriptableObject
{
    /* we keep the listeners in a private list so we know who is listening to this event */
    readonly List<IGameEventListener<T>> _eventListeners = new List<IGameEventListener<T>>();
    /// <summary>
    /// Raise iterates through the listeners and raises the event, it does so in a reversed for loop so in case a gameobject destroys itself due to this event being raised 
    /// so there will be no issues with the indexing
    /// </summary>
    /// <param name="item"></param>
    public void Raise(T item)
    {
        for (int i = _eventListeners.Count - 1; i >= 0; i--)
        {
            _eventListeners[i].OnEventRaised(item);
        }
    }
    /// <summary>
    /// Registers the listener by adding it to the private list of listeners
    /// </summary>
    /// <param name="listener">The listener to be registered</param>
    public void RegisterListener(IGameEventListener<T> listener)
    {
        if (!_eventListeners.Contains(listener))
        {
            _eventListeners.Add(listener);
        }
    }
    /// <summary>
    /// Unregisters the listener by removing it from the private list of listeners 
    /// </summary>
    /// <param name="listener">The listener to be unregistered</param>
    public void UnregisterListener(IGameEventListener<T> listener)
    {
        if (_eventListeners.Contains(listener))
        {
            _eventListeners.Remove(listener);
        }
    }
}

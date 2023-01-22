/// <summary>
/// Interface all listeners will inherit from which adds OnEventRaised
/// </summary>
/// <typeparam name="T">The type of the event</typeparam>
public interface IGameEventListener<T>
{
    void OnEventRaised(T item);
}

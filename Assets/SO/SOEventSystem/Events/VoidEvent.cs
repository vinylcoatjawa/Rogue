using UnityEngine;

[CreateAssetMenu(fileName = "New Void Event", menuName = "SO/Game Events/Void Event")]
/// <summary>
/// Custom event for Void type
/// </summary>
public class VoidEvent : BaseGameEvent<Void>
{
    public void Raise() => Raise(new Void());
}
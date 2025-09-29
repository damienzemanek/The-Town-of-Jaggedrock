using UnityEngine;

public abstract class Destination : MonoBehaviour
{
    public abstract bool preventContact { get; set; }
    public abstract void MakeContact();

    public abstract void ResetContact();
}

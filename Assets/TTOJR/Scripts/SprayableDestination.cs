using UnityEngine;

public class SprayableDestination : MonoBehaviour, IDestination
{
    public bool preventContact { get => isClean; set => isClean = value; }
    public bool isClean;
    public GameObject dirtyObject;
    public void MakeContact() => Clean();
    public void ResetContact() => Dirty();

    private void Start()
    {
        Dirty();
    }

    public void Clean()
    {
        isClean = true;
        dirtyObject.SetActive(false);
    }

    public void Dirty()
    {
        isClean = false;
        dirtyObject.SetActive(value: true);
    }
}

using System;
using DependencyInjection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(400)]
public class Interactor : MonoBehaviour
{
    [Inject] EntityControls controls;

    public bool canInteract;
    public GameObject interactDisplay;

    public UnityEvent InteractEvent;

    private void OnEnable()
    {
        controls.interact += Interact;
    }

    private void OnDisable()
    {
        controls.interact -= Interact;
    }

    public void ToggleCanInteract(bool canInteract)
    {
        this.canInteract = canInteract;
        interactDisplay.SetActive(canInteract);
    }

    public void SetInteractEvent(UnityEvent callback)
    {
        InteractEvent = callback;
    }

    public void Interact()
    {
        if(canInteract) InteractEvent?.Invoke();
    }



}

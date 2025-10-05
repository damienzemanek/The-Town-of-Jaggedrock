using System;
using System.Collections;
using DependencyInjection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(400)]
public class Interactor : MonoBehaviour
{
    [Inject] MainCamera mainCamera;
    [Inject] EntityControls controls;
    public LayerMask interactionMask;

    public bool canInteract;
    public GameObject interactDisplay;

    public UnityEvent InteractEvent;

    public Action<Ray, RaycastHit> RaycasterEvent;
    public Action FailedRaycast;

    private void OnEnable()
    {
        controls.interact += Interact;
        RaycasterEvent += InteractorRaycast;
    }

    private void OnDisable()
    {
        controls.interact -= Interact;
        RaycasterEvent -= InteractorRaycast;
    }
    private void Update()
    {
        CastInteractorRacyast();
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
        if (canInteract)
        {
            InteractEvent?.Invoke();
            print("Interactor: Callback invoked");
        }
    }


    public void CastInteractorRacyast()
    {
        Ray ray = new Ray(mainCamera.cam.transform.position, mainCamera.cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, mainCamera.castDist, interactionMask))
        {
            RaycasterEvent?.Invoke(ray, hit);
            hit.transform.gameObject.GetComponent<Detector>().OnRaycastedEnter(gameObject);
        }
        else
            FailedRaycast?.Invoke();

    }

    void InteractorRaycast(Ray ray, RaycastHit hit)
    {
        Debug.DrawLine(ray.origin, hit.point, Color.red);
    }



}
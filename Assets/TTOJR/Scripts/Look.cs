using UnityEngine;

[RequireComponent(typeof(EntityControls))]
public class Look : MonoBehaviour
{
    public EntityControls Controls { get => EntityControls.Instance; }

    [SerializeField] float xSens, ySens;
    [SerializeField] float lookX, lookY, xRot, yRot;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        MouseLook();
    }

    void MouseLook()
    {
        Vector2 mouseInput = Controls.look.Invoke();


        lookX = mouseInput.x * (xSens / 5);
        lookY = mouseInput.y * (ySens / 5) * -1;

        xRot += lookX;
        yRot += lookY;

        yRot = Mathf.Clamp(yRot, -90f, 90f);


        transform.rotation = Quaternion.Euler(0, xRot, 0);
        //print(Controls.head);
        Controls.head.transform.rotation = Quaternion.Euler(yRot, xRot, 0);

    }
}

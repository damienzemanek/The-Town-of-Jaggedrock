using UnityEngine;

[RequireComponent(typeof(EntityControls))]
public class EntityMove : MonoBehaviour
{
    public EntityControls Controls { get => EntityControls.Instance; }
    Rigidbody rb;
    [SerializeField] float speedMultiplier; float origSpeed;
    [SerializeField] float maxVel;


    private void Awake()
    {
        if(rb == null) rb = GetComponent<Rigidbody>();
        if (Controls == null) Debug.LogError("No Controls found");
        origSpeed = speedMultiplier;
        rb.maxLinearVelocity = maxVel;
        rb.maxAngularVelocity = maxVel;
    }

    private void FixedUpdate()
    {
        MoveEntity();
    }

    void MoveEntity()
    {
        Vector2 moveInput = (Controls != null) ? Controls.move.Invoke() : Vector2.zero ;
        if (moveInput == Vector2.zero) return;

        if (Mathf.Abs(moveInput.x) > 0.5f && Mathf.Abs(moveInput.y) > 0.5f)
            speedMultiplier = origSpeed * 0.7f;
        else
            speedMultiplier = origSpeed;

       // print(moveInput + " mlt " + speedMultiplier);

        if (moveInput.x != 0)
        {
            if (moveInput.x > 0.5)
                rb.AddForce(Controls.head.transform.right * speedMultiplier);
            if (moveInput.x < 0.5)
                rb.AddForce(-Controls.head.transform.right * speedMultiplier);
        }
        if (moveInput.y != 0)
        {
            if (moveInput.y > 0.5)
                rb.AddForce(Controls.head.transform.forward * speedMultiplier);
            if (moveInput.y < 0.5)
                rb.AddForce(-Controls.head.transform.forward * speedMultiplier);
        }

        print(rb.linearVelocity);

    }





}

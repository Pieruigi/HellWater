using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour
{
    Vector3 desiredVelocity;
    Vector3 currentVelocity;

    float acceleration = 15f;
    float maxSpeed = 5;

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal_1"), 0, Input.GetAxisRaw("Vertical_1")).normalized;

        desiredVelocity = maxSpeed * input;

        currentVelocity = Vector3.MoveTowards(currentVelocity, desiredVelocity, acceleration * Time.deltaTime);

        // Update rotation
        if (!Vector3.zero.Equals(input))
            transform.forward = input;

        Debug.Log("CurrentVelocity:" + currentVelocity);
    }

    private void FixedUpdate()
    {

        rb.MovePosition(rb.position + transform.forward * currentVelocity.magnitude * Time.fixedDeltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    WheelCollider[] wheels;

    WheelCollider[] ForwardWheel;

    public InputActionReference input;

    public InputActionReference Brake;

    Vector2 InputVector;

    float BrakeValue;

    Rigidbody CarBody;

    public float EngineStrength;

    public float MaxturningAngle;

    public float BrakeForce;

    // Start is called before the first frame update
    void Start()
    {
        //geting refrences to the wheels for driving
        wheels = GetComponentsInChildren<WheelCollider>();
        
        int i = 0;

        ForwardWheel = new WheelCollider[2];

        foreach (WheelCollider wheel in wheels)
        {//Getting refrences to the forward wheels for steering
            if (wheel.name == "Wheel_Forward_Right" || wheel.name == "Wheel_Forward_Left")
            {
                ForwardWheel[i] = wheel;

                i++;
            }
        }

        CarBody = GetComponent<Rigidbody>();
        // Lowering the center of mass is important for stability
        CarBody.centerOfMass = Vector3.down;
    }

    // Update is called once per frame
    void Update()
    { //Readign the input values
       InputVector  = input.action.ReadValue<Vector2>();
        BrakeValue = Brake.action.ReadValue<float>();
    }

    private void FixedUpdate()
    {
        //using the input values on the wheels to add or remove torque
        foreach (WheelCollider wheel in wheels)
        {
            wheel.motorTorque = EngineStrength*InputVector.y;
            wheel.brakeTorque = BrakeForce * BrakeValue;
        }
         
        //using our refrences to the forward wheels in order to turn
        foreach (WheelCollider TurningWheels in ForwardWheel)
        {
            TurningWheels.steerAngle = MaxturningAngle * InputVector.x;
        }

    }

    private void OnCollisionEnter(Collision collision)
    { 
        // Stopping the wheels rotation upon a collision prevents the car from getting stuck on walls. 
        foreach(WheelCollider wheel in wheels)
        {
            wheel.rotationSpeed = 0;
        }
    }
}

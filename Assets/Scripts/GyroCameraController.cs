using UnityEngine;

public class GyroCameraController : MonoBehaviour
{
    private Gyroscope gyro;
    
    float rotateX=0f;
    float rotateY=0f;

    public float rotationSpeed = 5f;

    private float yaw = 0f;
    private float pitch = 0f;


    private void Start()
    {
        // Check if the device supports gyroscope
        if (!SystemInfo.supportsGyroscope)
        {
            Debug.LogError("Gyroscope is not supported on this device.");
            return;
        }

        // Enable the gyroscope
        gyro = Input.gyro;
        gyro.enabled = true;
    }

    private void Update()
    {
        /*
        // Rotate the camera based on gyroscope input
        transform.Rotate(-gyro.rotationRateUnbiased.x, -gyro.rotationRateUnbiased.y, gyro.rotationRateUnbiased.z);

        //rotating camera with mouse input
        rotateX+=Input.GetAxis("Mouse X");
        rotateY+=Input.GetAxis("Mouse Y");
        transform.localEulerAngles=new Vector3(rotateX,rotateY,0);
        */
        // Retrieve mouse movement
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Retrieve gyro rotation
        Quaternion gyroRotation = Quaternion.identity;
        if (gyro.enabled)
        {
            gyroRotation = Input.gyro.attitude;
            gyroRotation.x *= -1; // Adjust for different coordinate systems
            gyroRotation.y *= -1;
        }

        // Apply rotation
        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        // Combine mouse and gyro rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f) * gyroRotation;

        // Rotate the camera
        transform.rotation = rotation;
    }
}

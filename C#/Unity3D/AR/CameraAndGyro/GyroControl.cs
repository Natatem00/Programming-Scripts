using UnityEngine;

namespace Controllers.AR
{
    public class NewGyroControl : MonoBehaviour
    {
        private bool gyroEnabled;
        private Gyroscope gyro;

        private GameObject cameraContainer;
        private Quaternion rot;

        private void Start()
        {
            cameraContainer = new GameObject("Camera Container");
            cameraContainer.transform.position = transform.position;
            cameraContainer.transform.SetParent(transform.parent);
            transform.SetParent(cameraContainer.transform);
            //cameraContainer.transform.SetParent(transform);
            gyroEnabled = EnableGyro();
        }

        private bool EnableGyro()
        {
            if (SystemInfo.supportsGyroscope)
            {
                gyro = Input.gyro;
                gyro.enabled = true;
                
                cameraContainer.transform.rotation = Quaternion.Euler(90f, 90f, 90f);
                rot = new Quaternion(0, 0, 1, 0);
                
                return true;
            }

            return false;
        }

        private void Update()
        {
            if (gyroEnabled)
            {
                transform.localRotation = gyro.attitude * rot;
            }
        }
    }
}
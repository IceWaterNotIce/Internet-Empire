    using NUnit.Framework;
    using UnityEngine;

namespace InternetEmpire
{


    public class CameraController : MonoBehaviour
    {
        private Camera mainCamera;
        private Vector3 targetCameraPosition;
        private float targetOrthographicSize;
        private Vector3 lastMousePosition;

    
        public float cameraSpeed;

        public int minCameraSize;

        public int maxCameraSize;


        void Start()
        {
            mainCamera = Camera.main;
            targetCameraPosition = mainCamera.transform.position;
            targetOrthographicSize = mainCamera.orthographicSize;
        }

        void Update()
        {
            HandleCameraMovement();
            SmoothCameraMovement();
            HandleCameraZoom();
            SmoothCameraZoom();
        }

        void HandleCameraZoom()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                targetOrthographicSize = Mathf.Max(targetOrthographicSize - 1, minCameraSize);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                targetOrthographicSize = Mathf.Min(targetOrthographicSize + 1, maxCameraSize);
            }
        }

        void SmoothCameraZoom()
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, Time.unscaledDeltaTime * 5f);
        }

        void HandleCameraMovement()
        {
            if (Input.GetMouseButtonDown(2)) // 按下滑鼠滾輪
            {
                lastMousePosition = Input.mousePosition;
            }

            if (Input.GetMouseButton(2)) // 拖動滑鼠滾輪
            {
                Vector3 delta = Input.mousePosition - lastMousePosition;
                Vector3 move = new Vector3(-delta.x, -delta.y, 0) * cameraSpeed * mainCamera.orthographicSize / 10;
                targetCameraPosition += move * Time.unscaledDeltaTime;
                lastMousePosition = Input.mousePosition;
            }
        }

        void SmoothCameraMovement()
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.unscaledDeltaTime * 5f);
        }
    }
}
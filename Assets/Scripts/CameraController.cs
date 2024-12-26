using UnityEngine;
using UnityEngine.InputSystem;

namespace InternetEmpire
{


    public class CameraController : MonoBehaviour
    {
        private Camera mainCamera;
        private Vector3 targetCameraPosition;
        private float targetOrthographicSize;
        private Vector3 lastMousePosition;

        [Range(5, 20)]
        public float cameraSpeed;

        public int minCameraSize;

        public int maxCameraSize;


        void Start()
        {
            mainCamera = Camera.main;
            targetCameraPosition = mainCamera.transform.position;
            targetOrthographicSize = mainCamera.orthographicSize;
            InputManager.Instance.OnMultiFingerTouch += HandleMultiFingerTouch;
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
            var scrollValue = Mouse.current.scroll.ReadValue().y;
            if (scrollValue > 0)
            {
            targetOrthographicSize = Mathf.Max(targetOrthographicSize - 1, minCameraSize);
            }
            else if (scrollValue < 0)
            {
            targetOrthographicSize = Mathf.Min(targetOrthographicSize + 1, maxCameraSize);
            }
        }


        private float TouchDistance;
        private void HandleMultiFingerTouch(System.Collections.Generic.List<Vector2> touches, float time)
    {
        if (touches.Count == 2)
        {
            Vector2 touch0 = touches[0];
            Vector2 touch1 = touches[1];

            float currentTouchDistance = Vector2.Distance(touch0, touch1);
            
            if (TouchDistance == 0)
            {
                TouchDistance = currentTouchDistance;
            }

            float delta = currentTouchDistance - TouchDistance;
            targetOrthographicSize = Mathf.Clamp(targetOrthographicSize - delta, minCameraSize, maxCameraSize);
            TouchDistance = currentTouchDistance;

            


        }
    }

        void SmoothCameraZoom()
        {
            mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, targetOrthographicSize, Time.unscaledDeltaTime * 5f);
        }

        void HandleCameraMovement()
        {
            if (Mouse.current.middleButton.wasPressedThisFrame)
            {
                lastMousePosition = Mouse.current.position.ReadValue();
            }

            if (Mouse.current.middleButton.isPressed)
            {
                Vector3 delta = (Vector3)Mouse.current.position.ReadValue() - lastMousePosition;
                Vector3 move = new Vector3(-delta.x, -delta.y, 0) * cameraSpeed * mainCamera.orthographicSize / 10;
                targetCameraPosition += move * Time.unscaledDeltaTime;
                lastMousePosition = Mouse.current.position.ReadValue();
            }
        }

        void SmoothCameraMovement()
        {
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetCameraPosition, Time.unscaledDeltaTime * 5f);
        }
    }
}
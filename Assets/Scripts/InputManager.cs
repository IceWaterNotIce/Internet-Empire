using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void MoveTouchEvent(Vector2 position, float time);
    public event MoveTouchEvent OnMoveTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;

    private InputSystem_Actions m_touchController;

    protected override void Awake()
    {
        m_touchController = new InputSystem_Actions();
        EnhancedTouchSupport.Enable();
    }

    private void OnEnable()
    {
        m_touchController.Enable();
    }

    private void OnDisable()
    {
        m_touchController.Disable();
    }

    private void Start()
    {
        m_touchController.Touch.TouchPress.started += ctx => StartTouch(ctx);
        m_touchController.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }

    private void StartTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch started");
        if (OnStartTouch != null) OnStartTouch(m_touchController.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
    }

    private void MoveTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch position");
        if (OnMoveTouch != null) OnMoveTouch(m_touchController.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
    }

    private void EndTouch(InputAction.CallbackContext ctx)
    {
        Debug.Log("Touch ended");
        if (OnEndTouch != null) OnEndTouch(m_touchController.Touch.TouchPosition.ReadValue<Vector2>(), (float)ctx.time);
    }


    private void FingerDown(Finger finger)
    {
        Debug.Log("Finger down");
        if (OnStartTouch != null) OnStartTouch(finger.screenPosition, Time.time);
    }

    private void Update()
    {
        Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches);
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            Debug.Log(touch.phase == UnityEngine.InputSystem.TouchPhase.Began);
        }

         // Check for active touches
        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            // If the touch is ongoing, call MoveTouch
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Moved || touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary)
            {
                Debug.Log("Touch position");
                if (OnMoveTouch != null) 
                    OnMoveTouch(touch.screenPosition, Time.time);
            }
        }
    }
}
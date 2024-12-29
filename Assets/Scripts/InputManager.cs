using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using System.Linq;
using System.Collections;


[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{

    public delegate void StartTouchEvent(Vector2 position, float time);
    public event StartTouchEvent OnStartTouch;

    public delegate void MoveTouchEvent(Vector2 position, float time);
    public event MoveTouchEvent OnMoveTouch;

    public delegate void EndTouchEvent(Vector2 position, float time);
    public event EndTouchEvent OnEndTouch;
    public delegate void MultiFingerTouchEvent(List<Vector2> positions, float time);
    public event MultiFingerTouchEvent OnMultiFingerTouch;

    public delegate void ZoomEvent(float delta, float time);
    public event ZoomEvent OnZoom;
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
        m_touchController.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        m_touchController.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();

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


    private Coroutine zoomCoroutine;
    private void ZoomStart()
    {
        Debug.Log("Zoom started");
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd()
    {
        Debug.Log("Zoom ended");
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
            zoomCoroutine = null;
        }
    }

    IEnumerator ZoomDetection()
    {
        float previousDistance = 0f, distance = 0f;
        while (true)
        {
            distance = Vector2.Distance(m_touchController.Touch.FirstTouchPosition.ReadValue<Vector2>(), m_touchController.Touch.SecondaryTouchPosition.ReadValue<Vector2>());
            // Zoom out
            if(distance != previousDistance)
            {
                if (OnZoom != null) OnZoom(distance - previousDistance, Time.time);
            }
            previousDistance = distance;
            
        }

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

        // Handle multi-finger touch
        if (UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Count > 1)
        {
            if (OnMultiFingerTouch != null)
                OnMultiFingerTouch(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches.Select(t => t.screenPosition).ToList(), Time.time);
        }
    }
}
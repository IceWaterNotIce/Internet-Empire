using UnityEngine;

public class DeviceController : MonoBehaviour
{
    private Device deviceData;

    public Device DeviceData
    {
        get { return deviceData; }
        set { deviceData = value; }
    }

    private Color originalColor;



    void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
    }

    void OnMouseOver()
    {
        GetComponent<Renderer>().material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = originalColor;
    }

    public void Connect(DeviceController otherDevice)
    {
        deviceData.Connect(otherDevice.DeviceData);
    }
}
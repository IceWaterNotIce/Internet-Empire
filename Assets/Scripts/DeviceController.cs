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

    [SerializeField] private GameObject ConnectionStateField;
    [SerializeField] private GameObject ConnectionState;

    void Start()
    {
        originalColor = GetComponent<Renderer>().material.color;
        GetComponent<SpriteRenderer>().sprite = deviceData.sprite;

        int maxConnections = deviceData.maxConnections;
        for (int i = 1; i < maxConnections; i++)
        {
            Instantiate(ConnectionState, ConnectionStateField.transform);
        }
    }

    void OnMouseOver()
    {
        GetComponent<SpriteRenderer>().material.color = Color.yellow;
    }

    void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().material.color = originalColor;
    }

    public void Connect(DeviceController otherDevice)
    {
        deviceData.Connect(otherDevice.DeviceData);
    }


}
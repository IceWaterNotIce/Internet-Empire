using UnityEngine;
using UnityEngine.UI;
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

    public int connectionsCount = 0;

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

    public void ConnectionAdded()
    {
        connectionsCount++;
        ConnectionStateField.transform.GetChild(connectionsCount - 1).GetComponent<RawImage>().color = Color.green;
    }

    public void ConnectionRemoved()
    {
        ConnectionStateField.transform.GetChild(connectionsCount - 1).GetComponent<RawImage>().color = Color.white;
        connectionsCount--;
    }
}
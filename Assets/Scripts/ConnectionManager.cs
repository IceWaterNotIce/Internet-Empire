using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public enum ConnectionMethod { Wire, Wireless }

    public List<GameObject> connectionPrefabs = new List<GameObject>();
    public ConnectionMethod currentMethod;
    public List<Connection> connections = new List<Connection>();

    private DeviceController firstDevice; // First selected device
    public MessageManager messageManager; // Reference to the MessageManager

    public Dictionary<ConnectionMethod, float> connectionCostPerMeter = new Dictionary<ConnectionMethod, float>
    {
        { ConnectionMethod.Wire, 10f }, // 每米10元
        { ConnectionMethod.Wireless, 5f } // 每米5元
    };

    public CityStreetSceneManager cityStreetSceneManager;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SetConnectionMethod(ConnectionMethod.Wire);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            SetConnectionMethod(ConnectionMethod.Wireless);
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                DeviceController device = hit.collider.GetComponent<DeviceController>();
                if (device != null)
                {
                    SelectDevice(device);
                }
            }
        }

    }

    void SetConnectionMethod(ConnectionMethod method)
    {
        currentMethod = method;
        Debug.Log($"Connection method set to: {currentMethod}");
    }

    void ConnectDevices(DeviceController device1, DeviceController device2)
    {
        if (device1 == null || device2 == null)
        {
            Debug.LogError("One or both devices are null. Connection failed.");
            return;
        }
        if (device1.gameObject == device2.gameObject)
        {
            messageManager.ShowMessage("Cannot connect a device to itself. Connection failed.");
            return;
        }

        float distance = Vector2.Distance(device1.transform.position, device2.transform.position);
        float cost = distance * connectionCostPerMeter[currentMethod];

        if (cityStreetSceneManager.money >= cost)
        {
            cityStreetSceneManager.money -= cost;

            device1.Connect(device2);
            device2.Connect(device1);

            GameObject connectionPrefab = connectionPrefabs[(int)currentMethod];
            GameObject connection = Instantiate(connectionPrefab, device1.transform.position, Quaternion.identity);
            connection.GetComponent<Connection>().Device1 = device1;
            connection.GetComponent<Connection>().Device2 = device2;
            connections.Add(connection.GetComponent<Connection>());
        }
        else
        {
            messageManager.ShowMessage("Not enough money to make the connection.");
            Debug.LogWarning("Not enough money to make the connection.");
        }
    }

    void SelectDevice(DeviceController device)
    {
        if (firstDevice == null)
        {
            firstDevice = device;
        }
        else
        {
            ConnectDevices(firstDevice, device);
            firstDevice = null;
        }
    }
}
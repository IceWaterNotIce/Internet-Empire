using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public List<Connection> connections = new List<Connection>();

    private DeviceController firstDevice; // First selected device
    public MessageManager messageManager; // Reference to the MessageManager

    public ConnectionList connectionList;

    public Connection currentMethod;

    public CityStreetSceneManager cityStreetSceneManager;

    public GameObject connectionPrefab;

    void Update()
    {
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

    public void SetConnectionMethod(Connection connection)
    {
        currentMethod = connection;
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
        float cost = distance * currentMethod.pricePerMeter;

        if (cityStreetSceneManager.money >= cost)
        {
            cityStreetSceneManager.money -= cost;

            device1.Connect(device2);
            device2.Connect(device1);

            GameObject connection = Instantiate(connectionPrefab, device1.transform.position, Quaternion.identity);
            ConnectionController connectionController = connection.GetComponent<ConnectionController>();
            connectionController.ConnectionData = currentMethod;
            connectionController.Device1 = device1;
            connectionController.Device2 = device2;
            
            
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
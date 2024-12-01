using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class ConnectionManager : MonoBehaviour
{
    public List<ConnectionController> connections = new List<ConnectionController>(); // List of connections

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
            firstDevice = null;
            return;
        }
        if (device1.gameObject == device2.gameObject)
        {
            messageManager.ShowMessage("Cannot connect a device to itself. Connection failed.");
            firstDevice = null;
            return;
        }
        if (device1.connectionsCount >= device1.DeviceData.maxConnections)
        {
            messageManager.ShowMessage("First device has reached maximum connections. Connection failed.");
            firstDevice = null;
            return;
        }
        if (device2.connectionsCount >= device2.DeviceData.maxConnections)
        {
            messageManager.ShowMessage("Second device has reached maximum connections. Connection failed.");
            firstDevice = null;
            return;
        }
        // use connections to check if the devices are already connected
        foreach (ConnectionController connection in connections)
        {
            if (connection.Device1 == device1 && connection.Device2 == device2 ||
                connection.Device1 == device2 && connection.Device2 == device1)
            {
                messageManager.ShowMessage("Devices are already connected. Connection failed.");
                firstDevice = null;
                return;
            }
        }
        float distance = Vector2.Distance(device1.transform.position, device2.transform.position);
        float cost = distance * currentMethod.pricePerMeter;

        if (cityStreetSceneManager.money >= cost)
        {
            cityStreetSceneManager.money -= cost;

            device1.ConnectionAdded();
            device2.ConnectionAdded();

            GameObject connection = Instantiate(connectionPrefab);
            ConnectionController connectionController = connection.GetComponent<ConnectionController>();
            connectionController.ConnectionData = currentMethod;
            connectionController.Device1 = device1;
            connectionController.Device2 = device2;
        }
        else
        {
            messageManager.ShowMessage("Not enough money to make the connection.");
            Debug.LogWarning("Not enough money to make the connection.");
            firstDevice = null;
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

    public void RemoveConnection(ConnectionController connection)
    {
        DeviceController device1 = connection.Device1;
        DeviceController device2 = connection.Device2;

        device1.ConnectionRemoved();
        device2.ConnectionRemoved();

        connections.Remove(connection);
    }

    public bool CanConnect(List<ConnectionController> connections, DeviceController start, DeviceController final)
    {
        HashSet<DeviceController> visited = new HashSet<DeviceController>();
        return DFS(connections, start, final, visited);
    }

    private bool DFS(List<ConnectionController> connections, DeviceController current, DeviceController final, HashSet<DeviceController> visited)
    {
        if (current == final)
        {
            return true;
        }

        visited.Add(current);

        foreach (ConnectionController connection in connections)
        {
            DeviceController next = null;
            if (connection.Device1 == current && !visited.Contains(connection.Device2))
            {
                next = connection.Device2;
            }
            else if (connection.Device2 == current && !visited.Contains(connection.Device1))
            {
                next = connection.Device1;
            }

            if (next != null && DFS(connections, next, final, visited))
            {
                return true;
            }
        }

        return false;
    }
}
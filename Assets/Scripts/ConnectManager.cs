using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class ConnectionManager : MonoBehaviour
{
    public enum ConnectionMethod { Wire, Wireless }

    public List<GameObject> connectionPrefabs = new List<GameObject>();
    public ConnectionMethod currentMethod;
    public List<Connection> connections = new List<Connection>();

    private Device firstDevice; // First selected device

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Device device = hit.collider.GetComponent<Device>();
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

    void ConnectDevices(Device device1, Device device2)
    {
        if (device1 != null && device2 != null)
        {
            Debug.Log($"Connecting {device1.deviceName} to {device2.deviceName} using {currentMethod}.");
            device1.Connect(device2);
            device2.Connect(device1);
            Debug.Log($"Connection successful.");
            Debug.Log($"Device 1 location: {device1.transform.position}, Device 2 location: {device2.transform.position}");
            GameObject connectionPrefab = connectionPrefabs[(int)currentMethod];
            GameObject connection = Instantiate(connectionPrefab, device1.transform.position, Quaternion.identity);
            connection.GetComponent<Connection>().Device1 = device1;
            connection.GetComponent<Connection>().Device2 = device2;
            connections.Add(connection.GetComponent<Connection>());
        }
        else
        {
            Debug.LogError("One or both devices are null. Connection failed.");
        }
    }

    void SelectDevice(Device device)
    {
        if (firstDevice == null)
        {
            firstDevice = device; // Set the first device
            Debug.Log($"First device selected: {device.deviceName}");
        }
        else
        {
            // Attempt to connect the selected devices
            ConnectDevices(firstDevice, device);
            firstDevice = null; // Reset the first device
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

namespace InternetEmpire
{

    public class ConnectionManager : MonoBehaviour
    {
        public List<ConnectionController> connections = new List<ConnectionController>(); // List of connections

        private DeviceController firstDevice; // First selected device
        public InternetEmpire.MessageManager messageManager; // Reference to the MessageManager

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

        [Test]
        public void CanConnectTestFalse()
        {
            List<ConnectionController> connections = new List<ConnectionController>();
            DeviceController startDevice = new GameObject().AddComponent<DeviceController>();
            startDevice.connectionsCount = 1;
            DeviceController finalDevice = new GameObject().AddComponent<DeviceController>();
            finalDevice.connectionsCount = 2;
            DeviceController device = new GameObject().AddComponent<DeviceController>();
            device.connectionsCount = 2;
            ConnectionController connection1 = new GameObject().AddComponent<ConnectionController>();
            connection1.Device1 = startDevice;
            connection1.Device2 = device;
            connections.Add(connection1);
            Assert.IsFalse(CanConnect(connections, startDevice, finalDevice));
        }

        [Test]
        public void CanConnectTestTrue()
        {
            List<ConnectionController> connections = new List<ConnectionController>();
            DeviceController startDevice = new GameObject().AddComponent<DeviceController>();
            startDevice.connectionsCount = 1;
            DeviceController finalDevice = new GameObject().AddComponent<DeviceController>();
            finalDevice.connectionsCount = 2;
            ConnectionController connection1 = new GameObject().AddComponent<ConnectionController>();
            connection1.Device1 = startDevice;
            connection1.Device2 = finalDevice;
            connections.Add(connection1);
            Assert.IsTrue(CanConnect(connections, startDevice, finalDevice));
        }
        // using A* algorithm to check if two devices can be connected

        public bool CanConnect(List<ConnectionController> connections, DeviceController startDevice, DeviceController finalDevice)
        {
            List<DeviceController> openList = new List<DeviceController>();
            List<DeviceController> closedList = new List<DeviceController>();

            openList.Add(startDevice);

            while (openList.Count > 0)
            {
                DeviceController currentDevice = openList[0];
                openList.Remove(currentDevice);
                closedList.Add(currentDevice);

                if (currentDevice == finalDevice)
                {
                    return true;
                }

                foreach (ConnectionController connection in connections)
                {
                    if (connection.Device1 == currentDevice)
                    {
                        if (!closedList.Contains(connection.Device2) && !openList.Contains(connection.Device2))
                        {
                            openList.Add(connection.Device2);
                        }
                    }
                    else if (connection.Device2 == currentDevice)
                    {
                        if (!closedList.Contains(connection.Device1) && !openList.Contains(connection.Device1))
                        {
                            openList.Add(connection.Device1);
                        }
                    }
                }
            }

            return false;
        }
    }
}
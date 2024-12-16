using System.Collections.Generic;
using UnityEngine;


namespace InternetEmpire
{

    public class ConnectionManager : MonoBehaviour
    {
        public List<Connection> connections = new List<Connection>(); // List of connections

        private Device firstDevice; // First selected device
        public MessageManager messageManager; // Reference to the MessageManager

        public ConnectionList connectionList;

        public ConnectionMethod currentMethod;

        public CityStreetSceneManager cityStreetSceneManager;

        public GameObject connectionPrefab;

        public Connection linkingConnection;

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null)
                {
                    Device device = hit.collider.GetComponent<Device>();
                    if (device != null && currentMethod != null)
                    {
                        SelectDevice(device);
                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                firstDevice = null;
                Destroy(linkingConnection.gameObject);
            }

            if (linkingConnection != null && firstDevice != null)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                linkingConnection.Device1 = firstDevice;
                linkingConnection.Device2 = null;
                linkingConnection.transform.position = (firstDevice.transform.position + (Vector3)mousePosition) / 2;
                linkingConnection.transform.right = mousePosition - (Vector2)firstDevice.transform.position;
                linkingConnection.transform.localScale = new Vector3(Vector2.Distance(firstDevice.transform.position, mousePosition) / 3, linkingConnection.transform.localScale.y, linkingConnection.transform.localScale.z);
            }

        }

        public void SetConnectionMethod(ConnectionMethod connection)
        {
            currentMethod = connection;
        }


        void ConnectDevices(Device device1, Device device2)
        {
            if (currentMethod == null)
            {
                messageManager.ShowMessage("No connection method selected. Connection failed.");
                firstDevice = null;
                return;
            }
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
            if (device1.ConnectionsCount >= device1.Model.MaxConnections)
            {
                messageManager.ShowMessage("First device has reached maximum connections. Connection failed.");
                firstDevice = null;
                return;
            }
            if (device2.ConnectionsCount >= device2.Model.MaxConnections)
            {
                messageManager.ShowMessage("Second device has reached maximum connections. Connection failed.");
                firstDevice = null;
                return;
            }
            // use connections to check if the devices are already connected
            foreach (Connection connection in connections)
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

                device1.ConnectionsCount++;
                device2.ConnectionsCount++;

                GameObject connection = Instantiate(connectionPrefab);
                Connection connectionController = connection.GetComponent<Connection>();
                connectionController.ConnectionData = currentMethod;
                connectionController.Device1 = device1;
                connectionController.Device2 = device2;

                connections.Add(connectionController);
            }
            else
            {
                messageManager.ShowMessage("Not enough money to make the connection.");
                Debug.LogWarning("Not enough money to make the connection.");
                firstDevice = null;
            }
        }

        void SelectDevice(Device device)
        {
            if (firstDevice == null && linkingConnection == null)
            {
                firstDevice = device;
                linkingConnection = Instantiate(connectionPrefab).GetComponent<Connection>();
            }
            else
            {
                Destroy(linkingConnection.gameObject);
                ConnectDevices(firstDevice, device);
                firstDevice = null;
            }
        }

        public void RemoveConnection(Connection connection)
        {
            Device device1 = connection.Device1;
            Device device2 = connection.Device2;

            device1.ConnectionsCount--;
            device2.ConnectionsCount--;

            connections.Remove(connection);
            Destroy(connection.gameObject);
        }

        public void RemoveConnection(Device device)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            foreach (Connection connection in connections)
            {
                if (connection.Device1 == device || connection.Device2 == device)
                {
                    connectionsToRemove.Add(connection);
                }
            }

            foreach (Connection connection in connectionsToRemove)
            {
                RemoveConnection(connection);
            }
        }


        // using A* algorithm to check if two devices can be connected
        public bool CanConnect(Device startDevice, Device finalDevice)
        {
            List<Device> openList = new List<Device>();
            List<Device> closedList = new List<Device>();

            openList.Add(startDevice);

            while (openList.Count > 0)
            {
                Device currentDevice = openList[0];
                openList.Remove(currentDevice);
                closedList.Add(currentDevice);

                if (currentDevice == finalDevice)
                {
                    return true;
                }

                foreach (Connection connection in connections)
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
        public List<Device> Route(Device startDevice, Device finalDevice)
        {
            List<Device> openList = new List<Device>();
            List<Device> closedList = new List<Device>();

            openList.Add(startDevice);

            while (openList.Count > 0)
            {
                Device currentDevice = openList[0];
                openList.Remove(currentDevice);
                closedList.Add(currentDevice);

                if (currentDevice == finalDevice)
                {
                    return closedList;
                }

                foreach (Connection connection in connections)
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

            return null;
        }

    }
}
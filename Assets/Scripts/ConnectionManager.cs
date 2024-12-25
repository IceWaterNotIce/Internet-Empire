using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


namespace InternetEmpire
{
    public class ConnectionManager : MonoBehaviour
    {
        public List<Connection> connections = new List<Connection>(); // List of connections

        /*
        a list of connection : [ the index of device1 in device list, the index of device2 in device list, the connection method index in ConnectionList*/
        public List<int[]> connectionsList = new List<int[]>();
        public string SaveConnectionList()
        {
            DeviceManager deviceManager = FindFirstObjectByType<DeviceManager>();

            connectionsList.Clear();
            foreach (Connection connection in connections)
            {
                int device1Index = deviceManager.devices.IndexOf(connection.Device1);
                int device2Index = deviceManager.devices.IndexOf(connection.Device2);
                int connectionMethodIndex = System.Array.IndexOf(connectionList.connections, connection.ConnectionData);
                connectionsList.Add(new int[] { device1Index, device2Index, connectionMethodIndex });
            }

            return JsonUtility.ToJson(connectionsList);
        }

        public void LoadConnectionList(string json)
        {
            DeviceManager deviceManager = FindFirstObjectByType<DeviceManager>();

            connectionsList = JsonUtility.FromJson<List<int[]>>(json);
            foreach (int[] connectionData in connectionsList)
            {
                Device device1 = deviceManager.devices[connectionData[0]];
                Device device2 = deviceManager.devices[connectionData[1]];
                ConnectionMethod connectionMethod = connectionList.GetConnection(connectionData[2]);
                createConnection(device1, device2, connectionMethod);
            }
        }

        private Device firstDevice; // First selected device

        public ConnectionList connectionList;

        public ConnectionMethod currentMethod;
        public CityStreetSceneManager cityStreetSceneManager;

        public GameObject connectionPrefab;

        public Connection linkingConnection;

        private InputManager inputManager;

        private void Awake()
        {
            inputManager = InputManager.Instance;
        }

        private void OnEnable()
        {
            inputManager.OnStartTouch += TouchSelectDevice;
        }

        private void OnDisable()
        {
            inputManager.OnStartTouch -= TouchSelectDevice;
        }

        private void TouchSelectDevice(Vector2 position, float time)
        {
            Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(position);
            Debug.Log("Touch position: " + position);
            RaycastHit2D hit = Physics2D.Raycast(TouchPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Device device = hit.collider.GetComponent<Device>();
                if (device != null && currentMethod != null)
                {
                    SelectDevice(device);
                }
            }

            inputManager.OnMoveTouch += TouchMoveConnection;
            inputManager.OnEndTouch += TouchEndConnection;
        }

        private void TouchEndConnection(Vector2 position, float time)
        {
            Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(position);
            Debug.Log("Touch position: " + position);
            Debug.Log("Touch position: " + TouchPosition);
            RaycastHit2D hit = Physics2D.Raycast(TouchPosition, Vector2.zero);
            if (hit.collider != null)
            {
                Device device = hit.collider.GetComponent<Device>();
                if (device != null && currentMethod != null)
                {
                    SelectDevice(device);
                }
            }
            else
            {
                cancelConnection();
            }

            inputManager.OnMoveTouch -= TouchMoveConnection;
            inputManager.OnEndTouch -= TouchEndConnection;
        }

        private void TouchMoveConnection(Vector2 position, float time)
        {
            Vector2 TouchPosition = Camera.main.ScreenToWorldPoint(position);
            Debug.Log("Touch position: " + position);
            Debug.Log("Touch position: " + TouchPosition);
            if (linkingConnection != null && firstDevice != null)
            {
                linkingConnection.Device1 = firstDevice;
                linkingConnection.Device2 = null;
                linkingConnection.transform.position = (firstDevice.transform.position + (Vector3)TouchPosition) / 2;
                linkingConnection.transform.right = TouchPosition - (Vector2)firstDevice.transform.position;
                linkingConnection.transform.localScale = new Vector3(Vector2.Distance(firstDevice.transform.position, TouchPosition) / 3, linkingConnection.transform.localScale.y, linkingConnection.transform.localScale.z);
            }
        }

        void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
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



            //#endregion


            // only works on PC

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                firstDevice = null;
                Destroy(linkingConnection.gameObject);
            }



            if (linkingConnection != null && firstDevice != null && Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                linkingConnection.Device1 = firstDevice;
                linkingConnection.Device2 = null;
                linkingConnection.transform.position = (firstDevice.transform.position + (Vector3)mousePosition) / 2;
                linkingConnection.transform.right = mousePosition - (Vector2)firstDevice.transform.position;
                linkingConnection.transform.localScale = new Vector3(Vector2.Distance(firstDevice.transform.position, mousePosition) / 3, linkingConnection.transform.localScale.y, linkingConnection.transform.localScale.z);
            }

        }

        

        public void cancelConnection()
        {
            firstDevice = null;
            Destroy(linkingConnection.gameObject);
        }

        public void SetConnectionMethod(ConnectionMethod connection)
        {
            currentMethod = connection;
        }


        void ConnectDevices(Device device1, Device device2)
        {
            if (currentMethod == null)
            {
                MessageManager.Instance.ToastMessage("Please select a connection method.");
                firstDevice = null;
                return;
            }
            if (device1 == null || device2 == null)
            {
                MessageManager.Instance.ToastMessage("Please select two devices to connect.");
                firstDevice = null;
                return;
            }
            if (device1.gameObject == device2.gameObject)
            {
                MessageManager.Instance.ToastMessage("Cannot connect a device to itself.");
                firstDevice = null;
                return;
            }
            if (device1.ConnectionsCount >= device1.Model.MaxConnections)
            {
                MessageManager.Instance.ToastMessage("The first device has reached the maximum number of connections.");
                firstDevice = null;
                return;
            }
            if (device2.ConnectionsCount >= device2.Model.MaxConnections)
            {
                MessageManager.Instance.ToastMessage("The second device has reached the maximum number of connections.");
                firstDevice = null;
                return;
            }
            // use connections to check if the devices are already connected
            foreach (Connection connection in connections)
            {
                if (connection.Device1 == device1 && connection.Device2 == device2 ||
                    connection.Device1 == device2 && connection.Device2 == device1)
                {
                    MessageManager.Instance.ToastMessage("The devices are already connected.");
                    firstDevice = null;
                    return;
                }
            }
            float distance = Vector2.Distance(device1.transform.position, device2.transform.position);
            float cost = distance * currentMethod.pricePerMeter;

            if (cityStreetSceneManager.money >= cost)
            {
                cityStreetSceneManager.money -= cost;

                createConnection(device1, device2, currentMethod);
            }
            else
            {
                MessageManager.Instance.ToastMessage("Not enough money to make the connection.");
                Debug.LogWarning("Not enough money to make the connection.");
                firstDevice = null;
            }
        }

        public void createConnection(Device device1, Device device2, ConnectionMethod connectionMethod)
        {
            device1.ConnectionsCount++;
            device2.ConnectionsCount++;

            GameObject connection = Instantiate(connectionPrefab);
            Connection connectionController = connection.GetComponent<Connection>();
            connectionController.ConnectionData = connectionMethod;
            connectionController.Device1 = device1;
            connectionController.Device2 = device2;

            connections.Add(connectionController);
        }

        void SelectDevice(Device device)
        {
            if (firstDevice == null && linkingConnection == null)
            {
                firstDevice = device;
                linkingConnection = Instantiate(connectionPrefab).GetComponent<Connection>();
                ConnectionPanelController connectionPanelController = FindFirstObjectByType<ConnectionPanelController>();
                connectionPanelController.toggleConnectionCancelButton(true);
                Debug.Log("First device selected.");
            }
            else
            {
                Destroy(linkingConnection.gameObject);
                ConnectDevices(firstDevice, device);
                firstDevice = null;
                ConnectionPanelController connectionPanelController = FindFirstObjectByType<ConnectionPanelController>();
                connectionPanelController.toggleConnectionCancelButton(false);
                Debug.Log("Second device selected.");
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
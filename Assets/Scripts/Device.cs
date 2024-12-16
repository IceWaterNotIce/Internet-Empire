namespace InternetEmpire
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class Device : MonoBehaviour
    {
        private DeviceModel m_model;
        public DeviceModel Model
        {
            get { return m_model; }
            set { m_model = value; }
        }

        private Color originalColor;

        [SerializeField] private GameObject ConnectionStateField;
        [SerializeField] private GameObject ConnectionState;

        [SerializeField] private Slider m_CapacityRemainSlider;

        private float m_capacityUsed = 0;
        public float CapacityUsed
        {
            get { return m_capacityUsed; }
            set
            {
                if (value < 0)
                {
                    Debug.LogError("Capacity used cannot be less than 0.");
                    m_capacityUsed = 0;
                }
                else if (value > m_model.Capacity)
                {
                    Debug.LogError("Capacity used cannot be greater than capacity.");
                    m_capacityUsed = m_model.Capacity;
                }
                m_capacityUsed = value;
                UpdateUI();
            }
        }

        private float m_handledSize = 0;

        public List<NetworkPacket> packets = new List<NetworkPacket>();

        public void AddPacket(NetworkPacket packet)
        {
            if (m_capacityUsed < m_model.Capacity)
            {
                packets.Add(packet);
                m_capacityUsed += packet.size;
                UpdateUI();
            }
            else
            {
                Debug.LogError("Cannot add packet. Capacity reached.");
            }
        }


        private int m_connectionsCount = 0;
        public int ConnectionsCount
        {
            get { return m_connectionsCount; }
            set
            {
                if (value < 0)
                {
                    Debug.LogError("Connections count cannot be less than 0.");
                    m_connectionsCount = 0;
                }
                else if (value > m_model.MaxConnections)
                {
                    Debug.LogError("Connections count cannot be greater than max connections.");
                    m_connectionsCount = m_model.MaxConnections;
                }
                m_connectionsCount = value;
                UpdateUI();
            }
        }
        public List<Connection> connections = new List<Connection>();

        public void AddConnection(Connection connection)
        {
            if (m_connectionsCount < m_model.MaxConnections)
            {
                connections.Add(connection);
                m_connectionsCount++;
                UpdateUI();

            }
            else
            {
                Debug.LogError("Cannot add connection. Max connections reached.");
            }
        }

        public void RemoveConnection(Connection connection)
        {
            if (connections.Contains(connection))
            {
                connections.Remove(connection);
                m_connectionsCount--;
                UpdateUI();
            }
            else
            {
                Debug.LogError("Connection not found.");
            }
        }

        void Start()
        {
            originalColor = GetComponent<Renderer>().material.color;
            
            UpdateUI();
        }

        void OnMouseOver()
        {
            GetComponent<SpriteRenderer>().material.color = Color.yellow;
        }

        void OnMouseExit()
        {
            GetComponent<SpriteRenderer>().material.color = originalColor;
        }

        void UpdateUI()
        {
            GetComponent<SpriteRenderer>().sprite = m_model.Sprite;
            for (int i = ConnectionStateField.transform.childCount; i < m_model.MaxConnections; i++)
            {
                Instantiate(ConnectionState, ConnectionStateField.transform);
            }
            for (int i = 0; i < m_model.MaxConnections; i++)
            {
                ConnectionStateField.transform.GetChild(i).GetComponent<RawImage>().color = i < m_connectionsCount ? Color.green : Color.white;
            }

            m_CapacityRemainSlider.maxValue = m_model.Capacity;
            m_CapacityRemainSlider.value = m_model.Capacity - m_capacityUsed;
        }



        void sendPacket()
        {
            if (packets[0] == null)
            {
                Debug.Log("Packet is null.");
                return;
            }

            // get next target device
            Device nextDevice = packets[0].route[0];
            packets[0].route.RemoveAt(0);

            // get the connection controller between this device and the next device
            float connectSpeed = 1;
            foreach (Connection connection in connections)
            {
                if (connection.Device1 == nextDevice || connection.Device2 == nextDevice)
                {
                    connectSpeed = connection.ConnectionData.maxSpeed;
                    break;
                }
            }
            if (packets[0].route.Count == 0)
            {
                Destroy(packets[0].gameObject);
                packets.RemoveAt(0);
            }
            else
            {
                // moving the packet to the next device
                packets[0].transform.position = Vector3.MoveTowards(packets[0].transform.position, nextDevice.transform.position, connectSpeed * Time.deltaTime);
            }

            m_capacityUsed -= packets[0].size;
            UpdateUI();
            
        }

        void Update()
        {
            Debug.Log("m_handledSize: " + m_handledSize);
            m_handledSize += m_model.HandlingSpeed * Time.deltaTime;
            if (packets.Count > 0 && m_handledSize >= packets[0].size)
            {
                m_handledSize -= packets[0].size;
                sendPacket();
            }
        }
    }
}
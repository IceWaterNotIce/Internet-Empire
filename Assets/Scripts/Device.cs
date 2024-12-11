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

        private int m_capacityUsed = 0;
        public int CapacityUsed
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

        public List<NetworkPacket> packets = new List<NetworkPacket>();


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
    }
}
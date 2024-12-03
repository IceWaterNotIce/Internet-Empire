using UnityEngine;
using System.Collections.Generic;

namespace InternetEmpire
{

    public class Client : MonoBehaviour
    {
        private ClientType m_clientType;
        public ClientType Type
        {
            get { return m_clientType; }
            set { m_clientType = value; }
        }

        private List<ClientDevice> m_ClientDevices = new List<ClientDevice>();
        public List<ClientDevice> Devices
        {
            get { return m_ClientDevices; }
            set { m_ClientDevices = value; }
        }

        private int m_satisfaction;
        public int Satisfaction
        {
            get { return m_satisfaction; }
            set
            {
                if (value <= 0)
                {
                    Destroy(gameObject);
                    return;
                }
                else if (value > 100)
                {
                    Debug.LogError("Satisfaction cannot be greater than 100.");
                    m_satisfaction = 100;
                }
                m_satisfaction = value;
                UpdateUI();
            }
        }

        //Todo:
        void OnDestroy()
        {
            ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();

            ClientManager clientManager = FindFirstObjectByType<ClientManager>();
            clientManager.clients.Remove(this);
            DeviceManager deviceManager = FindFirstObjectByType<DeviceManager>();
            foreach (ClientDevice device in m_ClientDevices)
            {
                connectionManager.RemoveConnection(device.Device);
                deviceManager.RemoveDevice(device.Device);
            }
        }

        void Start()
        {
            m_satisfaction = m_clientType.DefaultSatisfaction;
            UpdateUI();
        }

        void UpdateUI()
        {
            foreach (ClientDevice device in m_ClientDevices)
            {
                device.UpdateUI();
            }
        }
    }
}
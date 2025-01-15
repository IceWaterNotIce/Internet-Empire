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
                    Debug.LogWarning("Satisfaction cannot be greater than 100.");
                    m_satisfaction = 100;
                }
                m_satisfaction = value;
                UpdateUI();
            }
        }

        //Todo:
        void OnDestroy()
        {

            // remove connetions then remove devices , then remove client
            ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();

            DeviceManager deviceManager = FindFirstObjectByType<DeviceManager>();
            foreach (ClientDevice device in m_ClientDevices)
            {
                connectionManager.RemoveConnection(device.Device);
            }

            foreach (ClientDevice device in m_ClientDevices)
            {
                deviceManager.devices.Remove(device.Device);
            }

            ClientManager clientManager = FindFirstObjectByType<ClientManager>();
             if (clientManager != null)
            {
                clientManager.clients.Remove(this);
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
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Collections.Generic;

namespace InternetEmpire
{
    public class ClientDevice : MonoBehaviour
    {
        private Device m_device;
        public Device Device
        {
            get { return m_device; }
            set { m_device = value; }
        }

        private Client m_client;
        public Client Client
        {
            get { return m_client; }
            set { m_client = value; }
        }

        [SerializeField] private Slider m_satisfactionSlider;

        void Start()
        {
            if (Client.Type.MinDemandGenTime > 0)
            { StartCoroutine(GenerateDemand()); }
            UpdateUI();
        }

        void Update()
        {

        }

        IEnumerator GenerateDemand()
        {
            while (true)
            {
                yield return new WaitForSeconds(Client.Type.MinDemandGenTime);
                float demand = Random.Range(Client.Type.MinDemand, Client.Type.MaxDemand);
                Debug.Log("Demand: " + demand);
                CityStreetSceneManager cityStreetSceneManager = FindFirstObjectByType<CityStreetSceneManager>();
                ClientManager clientManager = FindFirstObjectByType<ClientManager>();
                // randomly select a client but not the current client
                Client client = clientManager.clients[Random.Range(0, clientManager.clients.Count)];
                ClientDevice clientDevice = client.Devices[Random.Range(0, client.Devices.Count)];

                if (clientDevice == this)
                {
                    Debug.Log("Selected client is the current client.");
                    continue;
                }
                // check the current client can connect to the selected client
                ConnectionManager connectionManager = FindFirstObjectByType<ConnectionManager>();
                if (connectionManager.CanConnect(m_device, clientDevice.Device))
                {
                    Debug.Log("Client can connect to the selected client.");
                    NetworkPacketManager networkPacketManager = FindFirstObjectByType<NetworkPacketManager>();
                    List<Device> route = connectionManager.Route(m_device, clientDevice.Device);
                    GameObject packetObject = Instantiate(networkPacketManager.packetPrefab, m_device.transform.position, Quaternion.identity);
                    NetworkPacket packet = packetObject.GetComponent<NetworkPacket>();
                    packet.route = route;
                    packet.source = m_device;
                    packet.destination = clientDevice.Device;
                    packet.size = demand;
                    networkPacketManager.AddPacket(packet);
                    m_device.AddPacket(packet);


                    cityStreetSceneManager.money += demand;
                    m_client.Satisfaction += 2;
                }
                else
                {
                    Debug.Log("Client cannot connect to the selected client.");
                    m_client.Satisfaction -= 2;
                }
            }
        }
        

        public void UpdateUI()
        {
            m_satisfactionSlider.value = m_client.Satisfaction;
        }
    }
}
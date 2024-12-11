using UnityEngine;
using System.Collections.Generic;

namespace InternetEmpire
{
    public class NetworkPacketManager : MonoBehaviour
    {
        public GameObject packetPrefab;
        public List<NetworkPacket> packets = new List<NetworkPacket>();

        public void AddPacket(NetworkPacket packet)
        {
            packets.Add(packet);
        }

        public void RemovePacket(NetworkPacket packet)
        {
            packets.Remove(packet);
        }

        public void SendPacket(NetworkPacket packet)
        {
            foreach (Device device in packet.route)
            {
                //
            }
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace InternetEmpire{
    public class NetworkPacket : MonoBehaviour{

        public float size;
        public Device source;
        public Device destination;

        public float lifetime = 10;

        public List<Device> route;
        public NetworkPacket(float size, Device source, Device destination, List<Device> route){
            this.size = size;
            this.source = source;
            this.destination = destination;
            this.route = route;
        }

        void Start(){
            // Start is called once before the first execution of Update after the MonoBehaviour is created
        }

        void Update(){
            // Update is called once per frame
            lifetime -= Time.deltaTime;
            if(lifetime <= 0){
                FailedDelivery();
            }
        }

        void SucessfulDelivery(){
            // Called when the packet is successfully delivered
            Destroy(gameObject);
        }

        void FailedDelivery(){
            // Called when the packet fails to be delivered
            Destroy(gameObject);
        }

        void OnDestroy(){
            // OnDestroy is called when the MonoBehaviour will be destroyed
            NetworkPacketManager networkPacketManager = FindFirstObjectByType<NetworkPacketManager>();
            networkPacketManager.RemovePacket(this);

            foreach(Device device in route){
                device.packets.Remove(this);
            }
        }
    }


}
using System.Collections.Generic;
using UnityEngine;

namespace InternetEmpire{
    public class NetworkPacket : MonoBehaviour{
        public string data;
        public Device source;
        public Device destination;

        public List<Device> route;
        public NetworkPacket(string data, Device source, Device destination, List<Device> route){
            this.data = data;
            this.source = source;
            this.destination = destination;
            this.route = route;

        }

        void Start(){
            // Start is called once before the first execution of Update after the MonoBehaviour is created
        }

        void Update(){
            // Update is called once per frame
        }
    }


}
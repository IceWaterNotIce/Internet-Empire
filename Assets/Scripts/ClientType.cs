namespace InternetEmpire
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewClient", menuName = "Client")]
    public class ClientType : ScriptableObject
    {
        private global::DeviceType device;

        public global::DeviceType Device
        {
            get { return device; }
            set { device = value; }
        }

        public int satisfaction;

        public float demandGenerationTime;
        public float minDemand;
        public float maxDemand;
    }
}


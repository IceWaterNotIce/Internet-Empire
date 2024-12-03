namespace InternetEmpire
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewClient", menuName = "Client")]
    public class ClientType : ScriptableObject
    {
        public string clientName;

        private Device device;

        public Device Device
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


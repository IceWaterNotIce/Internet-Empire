namespace InternetEmpire
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewConnection", menuName = "Connection")]
    public class ConnectionMethod : ScriptableObject
    {
        public float pricePerMeter;
        public float maxSpeed;
        public float latency;
        public float reliability;
        public float maxDistance;

        public Sprite sprite;
    }
}
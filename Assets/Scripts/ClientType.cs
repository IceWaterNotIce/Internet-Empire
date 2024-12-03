namespace InternetEmpire
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewClient", menuName = "Client")]
    public class ClientType : ScriptableObject
    {
        [SerializeField]
        private DeviceTypeList.DeviceType[] m_lstDeviceTypesCanUse;
        public DeviceTypeList.DeviceType[] DeviceTypesCanUse
        {
            get { return m_lstDeviceTypesCanUse; }
            set { m_lstDeviceTypesCanUse = value; }
        }

        [SerializeField]
        [Range(10, 100)]
        private int m_defaultSatisfaction = 50;
        public int DefaultSatisfaction
        {
            get { return m_defaultSatisfaction; }
            set { m_defaultSatisfaction = value; }
        }

        [SerializeField]
        [Range(1, 100)]
        private float m_minDemandGenerationTime = 2;
        public float MinDemandGenTime
        {
            get { return m_minDemandGenerationTime; }
            set { m_minDemandGenerationTime = value; }
        }

        [SerializeField]
        [Range(1, 100)]
        private float m_maxDemandGenerationTime = 5;
        public float MaxDemandGenTime
        {
            get { return m_maxDemandGenerationTime; }
            set { m_maxDemandGenerationTime = value; }
        }

        [SerializeField]
        [Range(1, 100)]
        private float m_minDemand = 1;
        public float MinDemand
        {
            get { return m_minDemand; }
            set { m_minDemand = value; }
        }

        [SerializeField]
        [Range(1, 100)]
        private float m_maxDemand = 5;
        public float MaxDemand
        {
            get { return m_maxDemand; }
            set { m_maxDemand = value; }
        }
    }
}


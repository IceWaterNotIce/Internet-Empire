namespace InternetEmpire
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "ClientList", menuName = "ClientList")]
    public class ClientList : ScriptableObject
    {
        public enum ClientType { Home, Business, Government, School }
        public Client[] clients;

        public Client GetClient(int index)
        {
            if (clients == null || index < 0 || index >= clients.Length)
            {
                Debug.LogError($"Index out of range. Index: {index}, Clients Length: {clients?.Length ?? 0}");
                return null;
            }
            return clients[index];
        }

        public int GetCount()
        {
            return clients?.Length ?? 0;
        }
    }
}
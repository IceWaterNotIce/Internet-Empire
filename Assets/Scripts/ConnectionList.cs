namespace InternetEmpire
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "NewConnectionList", menuName = "ConnectionList")]
    public class ConnectionList : ScriptableObject
    {
        public ConnectionMethod[] connections;

        public ConnectionMethod GetConnection(int index)
        {
            if (index < 0 || index >= this.connections.Length)
            {
                Debug.LogError("Index out of range.");
                return null;
            }
            return this.connections[index];
        }

        public int GetCount()
        {
            return this.connections.Length;
        }
    }
}
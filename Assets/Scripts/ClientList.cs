using UnityEngine;

public class ClientList : ScriptableObject
{
    public enum ClientType { Normal, VIP, Hacker, Thief }
    public Client[] clients;

    public Client GetClient(int index)
    {
        if (index < 0 || index >= this.clients.Length)
        {
            Debug.LogError("Index out of range.");
            return null;
        }
        return this.clients[index];
    }

    public int GetCount()
    {
        return this.clients.Length;
    }
}
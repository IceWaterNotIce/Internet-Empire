using UnityEngine;
using UnityEditor;
using InternetEmpire;

[CustomEditor(typeof(ClientType))]
public class ClientTypeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //existed field
        base.OnInspectorGUI();
        
        ClientType clientType = (ClientType)target;
        if (clientType.MinDemandGenTime > clientType.MaxDemandGenTime )
        {
            clientType.MinDemandGenTime = clientType.MaxDemandGenTime;
        }
        if (clientType.MinDemand > clientType.MaxDemand)
        {
            clientType.MinDemand = clientType.MaxDemand;
        }

        //clienttype.DeviceTypesCanUse child can not be same show error message if exit
        if (clientType.DeviceTypesCanUse.Length > 0)
        {
            for (int i = 0; i < clientType.DeviceTypesCanUse.Length; i++)
            {
                for (int j = i + 1; j < clientType.DeviceTypesCanUse.Length; j++)
                {
                    if (clientType.DeviceTypesCanUse[i] == clientType.DeviceTypesCanUse[j])
                    {
                        EditorGUILayout.HelpBox("DeviceTypesCanUse child can not be same", MessageType.Error);
                        return;
                    }
                }
            }
        }
       

    }
}
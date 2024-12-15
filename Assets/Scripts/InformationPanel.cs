using UnityEngine;
using System;
using System.Reflection;
using System.Diagnostics;
using UnityEngine.UI;
using TMPro;

namespace InternetEmpire
{
    public class InformationPanel : MonoBehaviour
    {
        [SerializeField] private string[] tags = { "Device", "ClientDevice", "Client", "ConnectionManager" };

        [SerializeField] private GameObject Panel;
        [SerializeField] private GameObject ContentLayout;
        [SerializeField] private GameObject InformationPrefab;
        public void ListObjectFields(object obj)
        {
            //show the panel
            Panel.SetActive(true);
            //clear the content layout
            foreach (Transform child in ContentLayout.transform)
            {
                Destroy(child.gameObject);
            }
            Type classType = obj.GetType();
            FieldInfo[] fields = classType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                GameObject go = Instantiate(InformationPrefab, ContentLayout.transform);
                //set parent
                go.transform.SetParent(ContentLayout.transform);
                go.GetComponent<TextMeshProUGUI>().text = $"{field.Name}: {field.GetValue(obj)}";
            }
        }

        void Update()
        {
             if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null)
                {
                   UnityEngine.Debug.Log($"Hit object: {hit.collider.gameObject.name}");
                   switch (hit.collider.gameObject.tag)
                    {
                        case "Device":
                            Device device = hit.collider.gameObject.GetComponent<Device>();
                            ListObjectFields(device);
                            break;
                        case "ClientDevice":
                            ClientDevice clientDevice = hit.collider.gameObject.GetComponent<ClientDevice>();
                            ListObjectFields(clientDevice);
                            break;
                        case "Client":
                            Client client = hit.collider.gameObject.GetComponent<Client>();
                            ListObjectFields(client);
                            break;
                        case "ConnectionManager":
                            ConnectionManager connectionManager = hit.collider.gameObject.GetComponent<ConnectionManager>();
                            ListObjectFields(connectionManager);
                            break;
                        default:
                            Console.WriteLine("No object found.");
                            break;
                    }
                }
            }
        }

        public void ClosePanel()
        {
            //hide the panel
            Panel.SetActive(false);
        }
    }
}
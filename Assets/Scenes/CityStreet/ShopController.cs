using UnityEngine;
using UnityEngine.UI;

namespace InternetEmpire
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] 
        private GameObject[] m_panels;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void TogglePanel(GameObject panel)
        {
            // if m_panels not include panel, return
            if (System.Array.IndexOf(m_panels, panel) == -1)
            {
                return;
            }
            panel.SetActive(!panel.activeSelf);
            foreach (GameObject p in m_panels)
            {
                if (p != panel)
                {
                    p.SetActive(false);
                }
            }
        }
    }
}
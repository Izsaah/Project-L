using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.UI;
namespace ProjectL.Local.MainMenu.Scripts.MenuUI
{
    public class MenuWindowSwitcher : MonoBehaviour
    {
        [Header("All your 3d Stations")]
        public GameObject[] allPanels;
        public void SwitchToPanel(GameObject panelToOpen)
        {
            foreach (GameObject panel in allPanels)
            {
                if (panel != null)
                {
                    panel.SetActive(panel == panelToOpen);
                }
            }
        }
    }
}

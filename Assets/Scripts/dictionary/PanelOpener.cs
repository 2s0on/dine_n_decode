using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panelToOpen; // assign the panel GameObject in the inspector

    public void OpenPanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true); // activates the panel, making it visible
        }
    }

    public void ClosePanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(false); // deactivates the panel, hiding it
        }
    }

    public void TogglePanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(!panelToOpen.activeSelf); // toggles visibility
        }
    }
}
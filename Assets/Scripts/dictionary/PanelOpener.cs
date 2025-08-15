using UnityEngine;

public class PanelOpener : MonoBehaviour
{
    public GameObject panelToOpen; // Assign your UI Panel here in the Inspector

    public void OpenPanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(true); // Activates the panel, making it visible
        }
    }

    public void ClosePanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(false); // Deactivates the panel, hiding it
        }
    }

    public void TogglePanel()
    {
        if (panelToOpen != null)
        {
            panelToOpen.SetActive(!panelToOpen.activeSelf); // Toggles visibility
        }
    }
}
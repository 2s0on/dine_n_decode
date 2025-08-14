using UnityEngine;
using UnityEngine.EventSystems;

public class TrashArea : MonoBehaviour
{
    public void DeleteItem(GameObject item)
    {
    Destroy(item);
    Debug.Log("Item trashed!");
    }
}
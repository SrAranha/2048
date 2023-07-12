using System.Collections.Generic;
using UnityEngine;

public class MenuSorter : MonoBehaviour
{
    [SerializeField] private List<GameObject> listOfMenus;
    [SerializeField] private int defaultMenuIndex;
    private int currentMenuIndex;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < listOfMenus.Count; i++)
        {
            DisableMenu(i);
        }
        currentMenuIndex = defaultMenuIndex;
        EnableMenu(currentMenuIndex);
    }

    private void DisableMenu(int menuIndex)
    {
        listOfMenus[menuIndex].SetActive(false);
    }
    private void EnableMenu(int menuIndex)
    {
        listOfMenus[menuIndex].SetActive(true);
    }
    public void ChangeMenu(int menuIndex)
    {
        DisableMenu(currentMenuIndex);
        EnableMenu(menuIndex);
        currentMenuIndex = menuIndex;
    }
}

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuSorter : MonoBehaviour
{
    [SerializeField] private bool backWithESC;
    [SerializeField] private List<GameObject> listOfMenus;
    [Min(0)]
    [SerializeField] private int defaultMenuIndex;
    private int currentMenuIndex;
    // Start is called before the first frame update
    void Start()
    {
        if (listOfMenus.Count == 0)
        {
            Debug.LogWarning("MenuSorter hasn't any menus to sort.", gameObject);
        }
        for (int i = 0; i < listOfMenus.Count; i++)
        {
            DisableMenu(i);
        }
        if (defaultMenuIndex >= listOfMenus.Count)
        {
            defaultMenuIndex = listOfMenus.Count - 1;
        }
        currentMenuIndex = defaultMenuIndex;
        EnableMenu(currentMenuIndex);
    }
    void Update()
    {
        if (backWithESC && Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("<color=purple>ESC PRESSED</color>");
            ChangeMenu(currentMenuIndex - 1);
        }
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
        if (menuIndex < 0 || menuIndex >= listOfMenus.Count) { return; }
        else
        {
            DisableMenu(currentMenuIndex);
            EnableMenu(menuIndex);
            currentMenuIndex = menuIndex;
        }
    }
}

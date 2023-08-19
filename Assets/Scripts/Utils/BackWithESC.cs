using UnityEngine;
using UnityEngine.SceneManagement;

public class BackWithESC : MonoBehaviour
{
    [SerializeField] private int sceneIndex;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}

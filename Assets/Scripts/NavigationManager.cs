using UnityEngine;

public class NavigationManager : MonoBehaviour
{
    public Material[] panoramas;
    private int currentIndex = 0;

    void Start()
    {
        UpdateSkybox();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            GoForward();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            GoBack();
        }
    }

    public void GoForward()
    {
        if (currentIndex < panoramas.Length - 1)
        {
            currentIndex++;
            UpdateSkybox();
        }
    }

    public void GoBack()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateSkybox();
        }
    }

    void UpdateSkybox()
    {
        RenderSettings.skybox = panoramas[currentIndex];
    }
}
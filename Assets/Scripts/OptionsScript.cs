using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class OptionsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] optionsStuff;

    [SerializeField]
    private TMP_Dropdown screenResDropdown;

    [SerializeField]
    private TMP_Dropdown screenSizeDropdown;

    private bool fullscreen = true;
    private int currRes = 0;
    public void ShowOptions()
    {
        gameObject.SetActive(true);
        StartCoroutine("PopIn");
    }

    IEnumerator PopIn()
    {
        for(int i = 0; i < optionsStuff.Length; i++)
        {
            int selected = Random.Range(0, optionsStuff.Length);
            if (!optionsStuff[selected].activeSelf)
            {
                optionsStuff[selected].SetActive(true);
                yield return new WaitForSeconds(.05f);
            }
            else
                i--;
        }
        yield return null;
    }

    public void hideOptions()
    {
        foreach(GameObject o in optionsStuff)
            o.SetActive(false);
        gameObject.SetActive(false);
    }

    public void changeRes()
    {
        if (currRes == screenResDropdown.value)
            return;
        switch(screenResDropdown.value)
        {
            case 0:
                Screen.SetResolution(1920, 1080, fullscreen);
                break;
            case 1:
                Screen.SetResolution(1440, 900, fullscreen);
                break;
            case 2:
                Screen.SetResolution(1366, 768, fullscreen);
                break;
        }
    }

    public void changeSize()
    {
        switch(screenSizeDropdown.value)
        {
            case 0:
                Screen.fullScreen = true;
                fullscreen = true;
                break;
            case 1:
                Screen.fullScreen = false;
                fullscreen = false;
                break;
        }
    }
}

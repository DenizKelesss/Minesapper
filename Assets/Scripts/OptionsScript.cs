using UnityEngine;
using System.Collections;

public class OptionsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject[] optionsStuff;
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
}

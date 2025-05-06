using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using NUnit.Framework;
using Unity.VisualScripting;

public class WireCutterMinigame : MonoBehaviour, IMinigame
{
    private Action onSuccess;
    private Action onFailure;
    private FirstPersonPlayer player;
    private List<Image> wires = new List<Image>();
    private List<GameObject> activeWireObjs = new List<GameObject>();
    private Color cut;
    private float timeRemaining;


    [SerializeField]
    private GameObject minigameUI;
    [SerializeField] 
    private float minigameTime = 10f;
    [SerializeField]
    private TextMeshProUGUI timerText;
    [SerializeField]
    private GameObject wirePrefab;
    [SerializeField]
    private RectTransform wireArea;
    [SerializeField]
    private int wireAmount = 4;
    [SerializeField]
    private TextMeshProUGUI instruction;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartMinigame(Action successCallback, Action failCallback, float customTime = -1f)
    {
        player = FindFirstObjectByType<FirstPersonPlayer>();
        if (player != null) player.canMove = false;

        onSuccess = successCallback;
        onFailure = failCallback;

        minigameUI.SetActive(true);

        timeRemaining = minigameTime;
        SpawnWires();
    }

    // Update is called once per frame
    void Update()
    {
        if (!minigameUI.activeSelf) return;
        timeRemaining -= Time.deltaTime;
        if (timerText != null) timerText.text = "Time: " + Mathf.Ceil(timeRemaining);
        if (timeRemaining <= 0) { MinigameFailed(); return; }

        foreach(var wire in wires)
        {
            if (Input.GetMouseButtonDown(0) &&
               RectTransformUtility.RectangleContainsScreenPoint(wire.rectTransform, Input.mousePosition))
            {
                if (wire.color == cut)
                {
                    wire.color = Color.black;
                    if(checkWiresCut()) { MinigameSuccess(); return; }

                }
                else { MinigameFailed(); return; }
            }
        }
    }

    private void SpawnWires()
    {
        List<Color> colors = new List<Color>();
        for(int i = 0; i < wireAmount; i++)
        {
            GameObject wire = Instantiate(wirePrefab, wireArea);
            wires.Add(wire.GetComponent<Image>());

            wire.GetComponent<Image>().rectTransform.anchoredPosition = new Vector2(
                0,
                (-wireArea.rect.height)/2 + (i * (wireArea.rect.height / wireAmount)+100)
                );

            var rand = Random.Range(1, 4);
            switch(rand)
            {
                case 1:
                    wire.GetComponent<Image>().color = Color.red;
                    if(!colors.Contains(Color.red))
                        colors.Add(Color.red);
                    break;
                case 2:
                    wire.GetComponent<Image>().color = Color.blue;
                    if (!colors.Contains(Color.blue))
                        colors.Add(Color.blue);
                    break;
                case 3:
                    wire.GetComponent<Image>().color = Color.green;
                    if (!colors.Contains(Color.green))
                        colors.Add(Color.green);
                    break;
            }
            activeWireObjs.Add(wire);
        }
        cut = colors[Random.Range(0, colors.Count)];
        if (cut == Color.red) instruction.text = "Cut the Red wires!";
        else if (cut == Color.blue) instruction.text = "Cut the Blue wires!";
        else if (cut == Color.green) instruction.text = "Cut the Green wires!";
    }

    private bool checkWiresCut()
    {
        foreach(Image img in wires)
        {
            if (img.color == cut)
                return false;
        }
        return true;
    }

    private void MinigameFailed()
    {
        instruction.text = string.Empty;
        EndMinigame();
        onFailure?.Invoke();
        var um = FindFirstObjectByType<UpgradeManager>();
        int dmg = (um != null) ? um.GetFailDamage() : 3;
        var ph = FindFirstObjectByType<PlayerHealth>();
        if (ph != null) ph.DecreaseHealth(dmg);
    }

    private void MinigameSuccess()
    {
        instruction.text = string.Empty;
        var um = FindFirstObjectByType<UpgradeManager>();
        if (um != null) um.GainXP(UnityEngine.Random.Range(5, 11));
        onSuccess?.Invoke();
        EndMinigame();
    }

    private void EndMinigame()
    {
        minigameUI.SetActive(false);
        if (player != null) player.canMove = true;
        ClearWires();
    }

    private void ClearWires()
    {
        foreach(var w in activeWireObjs) Destroy(w);
        activeWireObjs.Clear();
        foreach(var w in wires) Destroy(w);
        wires.Clear();
    }
}

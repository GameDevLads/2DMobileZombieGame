using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorMenu : MonoBehaviour
{
    public GameObject doorMenuUI;
    public Button purchaseButton;
    public Button adButton;

    public GameObject door;

    // Use ispaused variable to check if pause menu is active
    public BoolVariableSO isPaused;
    public BoolVariableSO keyPressedESO;
    public FloatVariableSO coinAmountSO;

    private void Start()
    {
        doorMenuUI.SetActive(false);
    }

    private void Update()
    {
        //Disable purchase button if not enough coins
        if(coinAmountSO.Value >= 1)
        {
            purchaseButton.interactable = true;
        }
        else
        {
            purchaseButton.interactable = false;
        }
    }

    public void Resume()
    {
        doorMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused.Value = false;
    }

    public void Pause()
    {
        doorMenuUI.SetActive(true);
        // Slow down time when in pause menu
        Time.timeScale = 0.3f;
        isPaused.Value = true;
    }

    public void Purchase()
    {
        if(coinAmountSO.Value >= 1)
        {
            Destroy(door);
            coinAmountSO.Value--;
        }
    }

    public void WatchAD()
    {
        // Placeholder
        /// Gimmie gimmie
    }
}

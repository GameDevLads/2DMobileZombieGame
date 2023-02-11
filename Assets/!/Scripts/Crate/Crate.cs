using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    private bool showButton = false;
    private bool showInterface = false;
    private Animator animator;
    public ItemModel currentItem;
    public ItemModel[] items;
    public bool isRunning = false;
    public bool shouldRun = true;
    private Texture sprite;
    [SerializeField]
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showButton = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            showButton = false;
        }
    }

    private void OnGUI()
    {
        if (showButton)
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Interact"))
            {
                animator.enabled = true;
                showInterface = true;
            }
        }
        if (showInterface)
        {
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "Interface");
            float spriteWidth = 256;
            float spriteHeight = 256;
            float x = Screen.width / 2 - spriteWidth / 2;
            float y = Screen.height / 2 - spriteHeight / 2;
            if(sprite != null)
                GUI.DrawTexture(new Rect(x, y, spriteWidth, spriteHeight), sprite);
            if (GUI.Button(new Rect(Screen.width / 2 - 150, Screen.height - 200, 150, 50), "Roll"))
            {
                if(!isRunning){
                    StartCoroutine(ShowItems());
                    isRunning = true;
                }
            }
            if (GUI.Button(new Rect(Screen.width / 2 , Screen.height - 200, 150, 50), "Watch Ad (X2)"))
            {
                // Watch Ad
            }
            if (GUI.Button(new Rect(Screen.width / 2 - 75, Screen.height - 100, 150, 50), "Close"))
            {
                showInterface = false;
            }
        }
    }

    void Roll()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].model.SetActive(false);
        }
    }
    public IEnumerator ShowItems()
    {
        StartCoroutine(SelectRandomItem());
        float timer = Time.time;
        while(shouldRun){
            float speed = 0.2f;
            sprite = items[currentIndex].model.GetComponent<SpriteRenderer>().sprite.texture;
            currentIndex++;
            //loop back to 0
            if(currentIndex >= items.Length)
            {
                currentIndex = 0;
            }
            yield return new WaitForSeconds(speed);

        }
    }
    IEnumerator SelectRandomItem()
    {
        // wait for 3 seconds
        yield return new WaitForSeconds(3);
        shouldRun = false;
        currentItem = items[Random.Range(0,items.Length)];
        // displayText.text = currentItem.item.itemName;
        // itemImage.sprite = currentItem.sprite;
    }
}

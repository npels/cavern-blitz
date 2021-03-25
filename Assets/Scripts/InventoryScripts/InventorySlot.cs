using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image image;
    public TMPro.TextMeshProUGUI quantityText;

    Item item;

    private void Awake()
    {
        item = GameObject.Find("Inventory").GetComponent<Item>();
    }

    private void Update()
    {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 click2D = new Vector2(clickpos.x, clickpos.y);

                RaycastHit2D hit = Physics2D.Raycast(click2D, Vector2.zero);
                if (hit.collider != null)
                {
                    Debug.Log(hit.collider.gameObject.name);
                }
            }
        
    }
    public void AddItem(Item.Items newItem, int i)
    {
        quantityText.text = i.ToString();
        image.sprite = item.GetItemObject(newItem);
        image.enabled = true;
        quantityText.enabled = true;
    }

    public void RemoveItem()
    {
        image = null;
        quantityText = null;
        image.sprite = null;
        image.enabled = false;
        quantityText.enabled = true;
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }
}

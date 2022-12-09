using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public int PanelNumber;

    [HideInInspector]
    public ShopController ShopController;

    [HideInInspector]
    public Color[] RarityColors;

    [HideInInspector]
    public GameObject TooltipPanel;
    private bool hover;

    [HideInInspector]
    public GameObject ThisItem;

    private void Start()
    {
        hover = false;

        TooltipPanel = GameObject.FindGameObjectWithTag("Tooltip");
        ShopController = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShopController>();
        RarityColors = ShopController.RarityColors;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hover)
        {
            TooltipPanel.transform.position = transform.position;

            hover = true;

            TooltipUpdate(TooltipPanel.GetComponent<ItemPanel>(), ThisItem);

            ShopController.CurrentSelectedItem = ThisItem;
            ShopController.SelectedItemNumber = PanelNumber;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hover)
        {
            TooltipPanel.transform.position = new Vector2(50, 50);

            hover = false;

            ShopController.CurrentSelectedItem = null;
            ShopController.SelectedItemNumber = 0;
        }
    }

    public void TooltipUpdate(ItemPanel ItemPanel, GameObject Item)
    {
        if (Item.tag == "Weapon")
        {
            ParticleCollision Weapon = Item.GetComponent<ParticleCollision>();

            ItemPanel.Name.text = Weapon.Name;
            ItemPanel.Name.color = RarityColors[Weapon.Rarity - 1];

            ItemPanel.Type.text = "Tier " + Weapon.Rarity + " Weapon";

            ItemPanel.Details.text =
                "Damage: " + Weapon.Damage + "\n" +
                "Cooldown: " + Weapon.UseTime + "\n" +
                "Range: " + Weapon.Range + "\n";

            ItemPanel.Tooltip.text = Weapon.TooltipText;
            ItemPanel.Tooltip.color = RarityColors[Weapon.Rarity - 1];

            ItemPanel.Price.text = (Weapon.SellPrice / 1.5).ToString();

            ItemPanel.Icon.GetComponent<Image>().sprite = Weapon.Sprite.GetComponent<SpriteRenderer>().sprite;
        }

        else if (Item.tag == "Accessory") {
            Accessory Accessory = Item.GetComponent<Accessory>();

            ItemPanel.Name.text = Accessory.Name;
            ItemPanel.Name.color = RarityColors[Accessory.Rarity - 1];

            ItemPanel.Type.text = "Tier " + Accessory.Rarity + " Accessory";

            string DetailText = "";
            if (Accessory.Health != 0) {
                DetailText += "Health: +" + Accessory.Health+ "\n";
            }
            if (Accessory.Damage != 0)
            {
                DetailText += "Damage: +" + Accessory.Damage + "%" + "\n";
            }
            if (Accessory.MoveSpeed != 0)
            {
                DetailText += "MoveSpeed: +" + Accessory.MoveSpeed;
            }

            ItemPanel.Details.text = DetailText;

            ItemPanel.Tooltip.text = Accessory.TooltipText;
            ItemPanel.Tooltip.color = RarityColors[Accessory.Rarity - 1];

            ItemPanel.Price.text = (Accessory.SellPrice / 1.5).ToString();

            ItemPanel.Icon.GetComponent<Image>().sprite = Accessory.GetComponent<SpriteRenderer>().sprite;
        }
    }
}

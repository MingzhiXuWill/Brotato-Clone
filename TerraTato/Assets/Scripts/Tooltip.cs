using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
        RarityColors = GameObject.FindGameObjectWithTag("GameController").GetComponent<ShopController>().RarityColors;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!hover)
        {
            TooltipPanel.transform.position = transform.position;
            TooltipPanel.SetActive(true);

            hover = true;

            TooltipUpdate(TooltipPanel.GetComponent<ItemPanel>(), ThisItem);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (hover)
        {
            TooltipPanel.transform.position = new Vector3(0, 0, 0);
            TooltipPanel.SetActive(false);

            hover = false;
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

            ItemPanel.Price.text = (Weapon.SellPrice / 5).ToString();

            ItemPanel.Icon.GetComponent<Image>().sprite = Weapon.Sprite.GetComponent<SpriteRenderer>().sprite;
        }
    }
}

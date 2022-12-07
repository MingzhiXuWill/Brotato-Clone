using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public int CurrentTier;

    public Color[] RarityColors;

    public GameObject[] ItemPanels;

    #region this one is actually hard to make. A double array in inspector
    [System.Serializable]
    public class SubClass
    {
        public GameObject[] ItemPoolTier;
    }
    [SerializeField] private SubClass[] ItemPool;
    #endregion

    void Start()
    {
        Reroll();
    }

    public void Reroll() {
        for (int i1 = 0; i1 < ItemPanels.Length; i1++)
        {
            ShopItemUpdate(i1, CreateRandomWeapon(CurrentTier));
        }
    }

    public void ShopItemUpdate(int ItemPanelNubmer, GameObject Item) 
    {
        ItemPanel ItemPanel = ItemPanels[ItemPanelNubmer].GetComponent<ItemPanel>();

        if (Item.tag == "Weapon") {
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

            ItemPanel.Price.text = Weapon.SellPrice.ToString();

            ItemPanel.Icon.GetComponent<Image>().sprite = Weapon.Sprite.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public GameObject CreateRandomWeapon(int TierNumber) 
    {
        List<GameObject> ItemPoolTemp = new List<GameObject>();

        // Merge Arrays, could also get the length then go get the object, save CPU power i guess

        for (int i1 = 0; i1 < TierNumber; i1++)
        {
            for (int i2 = 0; i2 < ItemPool[i1].ItemPoolTier.Length; i2++) 
            {
                ItemPoolTemp.Add(ItemPool[i1].ItemPoolTier[i2]);
            }
        }

        int RandomPos = Random.Range(0, ItemPoolTemp.Count);

        return ItemPoolTemp[RandomPos];
    }
}

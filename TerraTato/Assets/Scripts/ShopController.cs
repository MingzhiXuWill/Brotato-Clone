using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    [HideInInspector]
    public PlayerController Player;

    public int CurrentTier;

    public Color[] RarityColors;

    public GameObject[] ShopPanels;

    public GameObject[] WeaponPanels;

    public GameObject[] ItemPanels;

    [HideInInspector]
    public GameObject[] ShopItems;

    public TextMeshProUGUI WeaponText;
    public TextMeshProUGUI ItemText;
    public TextMeshProUGUI CoinText;

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
        // Set up shop
        ShopItems = new GameObject[4];
        // Find the player object
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        Player.SortWeapons();
        Player.SortAccessories();

        Reroll();

        WeaponUpdate();
        ShopItemUpdate();
        ItemUpdate();
    }

    public void Reroll() {
        for (int i1 = 0; i1 < ShopPanels.Length; i1++)
        {
            ShopItems[i1] = CreateRandomWeapon(CurrentTier);
        }

        ShopItemUpdate();
    }

    public void WeaponUpdate()
    {
        int count = 0;
        for (int i1 = 0; i1 < Player.Weapons.Length; i1++)
        {
            if (Player.Weapons[i1] != null)
            {
                count++;
                Image Icon = WeaponPanels[i1].transform.GetChild(0).GetComponent<Image>();

                WeaponPanels[i1].GetComponent<Tooltip>().ThisItem = Player.Weapons[i1];

                Icon.GetComponent<Image>().sprite = Player.Weapons[i1].GetComponent<ParticleCollision>().Sprite.GetComponent<SpriteRenderer>().sprite;
            }
            else 
            {
                WeaponPanels[i1].SetActive(false);
            }
        }

        WeaponText.text = "Weapons(" + count + "/4)";
    }

    public void ItemUpdate()
    {
        int count = 0;
        for (int i1 = 0; i1 < Player.Accessories.Length; i1++)
        {
            if (Player.Accessories[i1] != null)
            {
                count++;
                Image Icon = ItemPanels[i1].transform.GetChild(0).GetComponent<Image>();

                ItemPanels[i1].GetComponent<Tooltip>().ThisItem = Player.Accessories[i1];

                Icon.GetComponent<Image>().sprite = Player.Accessories[i1].GetComponent<SpriteRenderer>().sprite;
            }
            else
            {
                ItemPanels[i1].SetActive(false);
            }
        }

        ItemText.text = "Accessories(" + count + "/12)";
    }

    public void ShopItemUpdate() 
    {
        for (int i1 = 0; i1 < ShopPanels.Length; i1++)
        {
            GameObject Item = ShopItems[i1];
            ItemPanel ItemPanel = ShopPanels[i1].GetComponent<ItemPanel>();
            if (Item == null) 
            {
                ItemPanel.gameObject.SetActive(false);
            }
            else if (Item.tag == "Weapon")
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

                ItemPanel.Price.text = Weapon.SellPrice.ToString();

                ItemPanel.Icon.GetComponent<Image>().sprite = Weapon.Sprite.GetComponent<SpriteRenderer>().sprite;
            }
            else if (Item.tag == "Accessory")
            {
                Accessory Accessory = Item.GetComponent<Accessory>();

                ItemPanel.Name.text = Accessory.Name;
                ItemPanel.Name.color = RarityColors[Accessory.Rarity - 1];

                ItemPanel.Type.text = "Tier " + Accessory.Rarity + " Accessory";

                string DetailText = "";
                if (Accessory.Health != 0)
                {
                    DetailText += "Health: +" + Accessory.Health + "\n";
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

                ItemPanel.Price.text = (Accessory.SellPrice / 5).ToString();

                ItemPanel.Icon.GetComponent<Image>().sprite = Accessory.GetComponent<SpriteRenderer>().sprite;
            }
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

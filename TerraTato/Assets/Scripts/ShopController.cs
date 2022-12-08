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

    public int TierMulti;

    public Color[] RarityColors;

    public GameObject[] ShopPanels;

    public GameObject[] WeaponPanels;

    public GameObject[] ItemPanels;

    [HideInInspector]
    public GameObject[] ShopItems;

    public TextMeshProUGUI WeaponText;
    public TextMeshProUGUI ItemText;
    public TextMeshProUGUI CoinText;

    public TextMeshProUGUI HealthText;
    public TextMeshProUGUI DamageText;
    public TextMeshProUGUI MoveSpeedText;

    public TextMeshProUGUI UpgradeText;
    public TextMeshProUGUI ShopLevelText;

    [HideInInspector]
    public GameObject TooltipPanel;
    [HideInInspector]
    public GameObject CurrentSelectedItem = null;
    [HideInInspector]
    public int SelectedItemNumber = 0;

    // Sound
    public AudioClip RerollSound;
    public AudioClip CoinSound;

    bool FirstTimePlay = true;

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
        TooltipPanel = GameObject.FindGameObjectWithTag("Tooltip");

        // Set up shop
        ShopItems = new GameObject[4];
        // Find the player object
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        ChangeCoin(10);
        Reroll();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            SellItem(CurrentSelectedItem, SelectedItemNumber);

            TooltipPanel.transform.position = new Vector2(50, 50);
        }     
    }

    public void SellItem(GameObject Item, int ItemNumber) {
        if (Item.tag == "Weapon")
        {
            ChangeCoin((int)(Player.Weapons[ItemNumber - 1].GetComponent<ParticleCollision>().SellPrice / 1.5f));

            Player.SortWeapons();
            Player.Weapons[ItemNumber - 1] = null;
        }
        else
        {
            ChangeCoin((int)(Player.Accessories[ItemNumber - 1].GetComponent<Accessory>().SellPrice / 1.5f));

            Player.Accessories[ItemNumber - 1] = null;

            Player.SortAccessories();
            Player.StatsUpdate();
        }
        SoundManager.sndman.PlaySound(CoinSound, 2f);
        ShopUpdate();
    }

    public void BuyItem(int ItemNumber) 
    {
        GameObject Item = ShopItems[ItemNumber];

        if (Item.tag == "Weapon")
        {
            ParticleCollision Weapon = Item.GetComponent<ParticleCollision>();
            if (CoinCheck(Weapon.SellPrice) && EquipableCheck(Item)) 
            {
                SoundManager.sndman.PlaySound(CoinSound, 2f);
                ShopItems[ItemNumber] = null;
                EquipItem(Item);
                ChangeCoin(-Weapon.SellPrice);
            }
        }
        else
        {
            Accessory Accessory = Item.GetComponent<Accessory>();
            if (CoinCheck(Accessory.SellPrice) && EquipableCheck(Item))
            {
                SoundManager.sndman.PlaySound(CoinSound, 2f);
                ShopItems[ItemNumber] = null;
                EquipItem(Item);
                ChangeCoin(-Accessory.SellPrice);
            }
        }

        ShopUpdate();
    }

    public void ChangeCoin(int Price) {
        Player.TotalCoins += Price;
    }

    public void EquipItem(GameObject Item)
    {
        if (Item.tag == "Weapon")
        {
            Player.SortWeapons();
            Player.Weapons[Player.WeaponNumber] = Item;
        }
        else
        {
            Player.Accessories[Player.AccessoryNumber] = Item;
            Player.SortAccessories();
            Player.StatsUpdate();
        }
    }

    public bool CoinCheck(int Price) 
    {
        return (Player.TotalCoins >= Price);
    }

    public bool EquipableCheck(GameObject Item)
    {
        if (Item.tag == "Weapon")
        {
            if (Player.WeaponNumber < 4)
            {
                return true;
            }
            else 
            {
                return false;
            }
        }
        else 
        {
            if (Player.AccessoryNumber < 12)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public void Reroll()
    {
        if (CoinCheck(10)) {
            for (int i1 = 0; i1 < ShopPanels.Length; i1++)
            {
                ShopItems[i1] = CreateRandomItem(CurrentTier);
            }

            ChangeCoin(-10);

            if (FirstTimePlay) 
            {
                FirstTimePlay = false;
            }
            else 
            {
                SoundManager.sndman.PlaySound(RerollSound, 1f);    
            }

            ShopItemUpdate();
            CoinTextUpdate();
        }
    }

    public void ShopUpdate() {
        Player.SortWeapons();
        Player.SortAccessories();

        UpgradeUpdate();
        WeaponUpdate();
        ShopItemUpdate();
        ItemUpdate();
        CoinTextUpdate();
        StatsUpdate();
    }

    public void WeaponUpdate()
    {
        int count = 0;
        for (int i1 = 0; i1 < Player.Weapons.Length; i1++)
        {
            if (Player.Weapons[i1] != null)
            {
                WeaponPanels[i1].SetActive(true);
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
                ItemPanels[i1].SetActive(true);
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

    public void StatsUpdate() {
        HealthText.text = "Health: " + Player.MaxHealth;
        DamageText.text = "Damage: " + Player.DamageMulti + "%";
        MoveSpeedText.text = "Move Speed: " + Player.MoveSpeed;
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
                ItemPanel.gameObject.SetActive(true);
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
                ItemPanel.gameObject.SetActive(true); 
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

                ItemPanel.Price.text = Accessory.SellPrice.ToString();

                ItemPanel.Icon.GetComponent<Image>().sprite = Accessory.GetComponent<SpriteRenderer>().sprite;
            }
        }
    }

    public void CoinTextUpdate() {
        CoinText.text = Player.TotalCoins.ToString();
    }

    public void UpgradeUpdate() 
    {
        UpgradeText.text = "Upgrade Shop (Cost " + (CurrentTier * TierMulti) + ")";
    }

    public void ShopLevelUpdate()
    {
        ShopLevelText.text = "Shop(Level " + CurrentTier + ")";
    }

    public void Upgrade() 
    {
        if (CoinCheck(CurrentTier * TierMulti)) {
            SoundManager.sndman.PlaySound(CoinSound, 2f);

            ChangeCoin(-CurrentTier * TierMulti);

            CurrentTier ++;

            CoinTextUpdate();
            UpgradeUpdate();
            ShopLevelUpdate();
        } 
    }

    public GameObject CreateRandomItem(int TierNumber) 
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

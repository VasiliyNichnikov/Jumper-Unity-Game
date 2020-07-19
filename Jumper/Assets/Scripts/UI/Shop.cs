using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
  public Text selectedItemPrice;
  public Text selectedItemDonatePrice;
  public Text selectedItemRarity;
  public Transform selectedItemModelField;
  public Button buttonBuyForCoins;
  public Button buttonBuyForDonateCoins;
  public Text playerCoins;
  public Text playerDonateCoins;

  public ShopItem selectedItem
  {
    get
    {
      return _selectedItem;
    }
    set
    {
      if (value != _selectedItem)
      {
        _selectedItem = value;
        UpdateSelectedItemData();
      }
    }
  }

  private ShopItem _selectedItem;

  private void Awake()
  {
    if (PlayerData.instance == null)
    {
      SaveSystem.LoadPlayerData();
    }
    UpdatePlayerCoins();
  }

  private void Update()
  {
    selectedItemModelField.Rotate(0, .5f, 0);
  }

  private void UpdateSelectedItemModel()
  {
    foreach (Transform child in selectedItemModelField)
    {
      GameObject.Destroy(child.gameObject);
    }
    GameObject model = Instantiate(_selectedItem.model, Vector3.zero, Quaternion.identity, selectedItemModelField);
    model.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
    model.transform.localPosition = Vector3.zero;
  }

  private bool IsItemBought(string itemId)
  {
    return PlayerData.instance.jumpers.Contains(itemId);
  }

  private void UpdateSelectedItemData()
  {
    bool isItemBought = IsItemBought(_selectedItem.id);
    if (!isItemBought)
    {
      selectedItemPrice.text = _selectedItem.price.ToString();
      selectedItemDonatePrice.text = _selectedItem.donatePrice.ToString();
      buttonBuyForCoins.interactable = PlayerData.instance.coins > _selectedItem.price;
      buttonBuyForDonateCoins.interactable = PlayerData.instance.donateCoins > _selectedItem.donatePrice;
    }
    selectedItemRarity.text = _selectedItem.rarity.ToString();
    buttonBuyForCoins.gameObject.SetActive(!isItemBought);
    buttonBuyForDonateCoins.gameObject.SetActive(!isItemBought);
    UpdateSelectedItemModel();
  }

  private void UpdatePlayerCoins()
  {
    playerCoins.text = PlayerData.instance.coins.ToString();
    playerDonateCoins.text = PlayerData.instance.donateCoins.ToString();
  }

  public void BuyItem(bool isDonateCoins)
  {
    if (isDonateCoins)
    {
      PlayerData.instance.donateCoins -= selectedItem.donatePrice;
    }
    else
    {
      PlayerData.instance.coins -= selectedItem.price;
    }
    PlayerData.instance.jumpers.Add(selectedItem.id);
    SaveSystem.SavePlayerData();
    UpdateSelectedItemData();
    UpdatePlayerCoins();
  }
}

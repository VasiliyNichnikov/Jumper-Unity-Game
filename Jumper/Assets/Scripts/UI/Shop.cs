using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
  public GameObject itemsField;
  public ShopItem[] shopItems;
  public Text selectedItemPrice;
  public Text selectedItemDonatePrice;

  public ShopItem selectedItem
  {
    get
    {
      return _selectedItem;
    }
    set
    {
      _selectedItem = value;
      selectedItemPrice.text = _selectedItem.price.ToString();
      selectedItemDonatePrice.text = _selectedItem.donatePrice.ToString();
    }
  }

  public ShopItem _selectedItem;

  void Start()
  {
    if (itemsField != null)
    {
      foreach (ShopItem shopItem in shopItems)
      {
        ShopItem newShopItem = Instantiate(shopItem, Vector3.zero, Quaternion.identity, itemsField.transform);
        if (selectedItem == null)
        {
          selectedItem = shopItem;
          newShopItem.GetComponent<Button>().Select();
        }
      }
    }
  }
}

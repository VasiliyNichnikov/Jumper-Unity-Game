using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
  public Text selectedItemPrice;
  public Text selectedItemDonatePrice;
  public Text selectedItemRarity;

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
      selectedItemRarity.text = _selectedItem.rarity.ToString();
    }
  }

  private ShopItem _selectedItem;

  void Start()
  {
  }
}

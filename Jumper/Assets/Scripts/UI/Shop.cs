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
        selectedItemPrice.text = _selectedItem.price.ToString();
        selectedItemDonatePrice.text = _selectedItem.donatePrice.ToString();
        selectedItemRarity.text = _selectedItem.rarity.ToString();
        UpdateSelectedItemModel();
      }
    }
  }

  private ShopItem _selectedItem;

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
}

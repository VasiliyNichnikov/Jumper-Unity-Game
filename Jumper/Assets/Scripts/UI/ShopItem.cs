﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ShopItem : MonoBehaviour
{
  public GameObject model;
  public int price;
  public int donatePrice;
  public Rarity rarity;
  public string id;

  private Shop shopManager;
  private UnityAction _action;

  private void Start()
  {
    id = model.name;
    shopManager = FindObjectOfType<Shop>();
    Button button = gameObject.GetComponent<Button>();
    if (shopManager.selectedItem == null)
    {
      shopManager.selectedItem = this;
      button.Select();
    }
    _action += OnButtonClick;
    button.onClick.AddListener(_action);
  }

  private void OnButtonClick()
  {
    shopManager.selectedItem = this;
  }
}

public enum Rarity
{
  Casual,
  Rare,
  Epic,
}
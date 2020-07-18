using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
  public static PlayerData instance;
  public int coins = 2500;
  public int donateCoins = 250;
  public HashSet<string> jumpers = new HashSet<string>();

  public PlayerData(PlayerData playerData = null)
  {
    if (playerData == null)
    {
      instance = this;
    }
    else
    {
      instance = playerData;
    }
  }
}

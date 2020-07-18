using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
  private static string DATA_PATH = "/player.data";

  public static void SavePlayerData()
  {
    BinaryFormatter formatter = new BinaryFormatter();
    string path = $"{Application.persistentDataPath}{DATA_PATH}";
    FileStream stream = new FileStream(path, FileMode.Create);

    formatter.Serialize(stream, PlayerData.instance);
    stream.Close();
  }

  public static void LoadPlayerData()
  {
    string path = $"{Application.persistentDataPath}{DATA_PATH}";
    if (File.Exists(path))
    {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);
      PlayerData savedData = formatter.Deserialize(stream) as PlayerData;

      stream.Close();
      new PlayerData(savedData);
    }
    else
    {
      new PlayerData();
    }
  }
}

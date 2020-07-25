using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
  public KeyCode MoveRight;
  public float moveSpeed = 0.5f;

  private bool flagOn = false;
  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (flagOn)
      MoveOBJ();
  }
  void Control()
  {
    if (Input.GetKey(MoveRight))
    {
      MoveOBJ();
    }
  }
  public void MoveOBJ()
  {
    transform.position += new Vector3(moveSpeed * Time.deltaTime, 0, 0);
  }
  public void SwitchMove()
  {
    flagOn = !flagOn;
  }
}

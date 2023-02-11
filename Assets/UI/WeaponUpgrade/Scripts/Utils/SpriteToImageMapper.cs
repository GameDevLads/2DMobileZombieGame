using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteToImageMapper : MonoBehaviour
{
  public Image ImageRef;
  public SpriteVariableSO SpriteVariable;
  // Start is called before the first frame update
  void Start()
  {
    Change();
  }

  public void Change()
  {
    ImageRef.sprite = SpriteVariable.Value;
    ImageRef.SetNativeSize();
  }
}

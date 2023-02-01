using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloatToTxtMapper : MonoBehaviour
{
  public TMP_Text Text;
  public FloatVariableSO FloatVariable;
  public StringVariableSO IfNull;
  // Start is called before the first frame update
  void Start()
  {
    Change();
  }

  public void Change()
  {
    Text.text = FloatVariable.Value.ToString();
    if (FloatVariable.IsNull)
      Text.text = IfNull.Value;
  }
}

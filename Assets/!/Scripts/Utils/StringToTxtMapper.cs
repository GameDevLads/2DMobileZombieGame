using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StringToTxtMapper : MonoBehaviour
{
  public TMP_Text Text;
  public StringVariableSO StringVariable;
  public StringVariableSO IfNull;
  // Start is called before the first frame update
  void Start()
  {
    Change();
  }

  public void Change()
  {
    Text.text = StringVariable.Value;
    if (StringVariable.IsNull)
      Text.text = IfNull.Value;
  }
}

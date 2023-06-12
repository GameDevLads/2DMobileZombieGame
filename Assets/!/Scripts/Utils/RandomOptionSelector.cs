using System;
using System.Collections.Generic;

public class RandomOptionSelector
{
  private List<int> options;
  private List<int> selectedOptions;

  public RandomOptionSelector(int numOfOptions)
  {
    options = new List<int>(numOfOptions);
    selectedOptions = new List<int>(numOfOptions);

    for (int i = 0; i < numOfOptions; i++)
    {
      options.Add(i);
    }
  }

  public int GetRandomOption()
  {
    int index = UnityEngine.Random.Range(0, options.Count);
    int option = options[index];

    // Remove the selected option from the available options
    options.RemoveAt(index);

    // Add the selected option to the list of selected options
    selectedOptions.Add(option);

    // If all options have been selected, reset the available options
    if (options.Count == 0)
    {
      options.AddRange(selectedOptions);
      selectedOptions.Clear();
    }

    return option;
  }
}

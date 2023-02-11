using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon Type")]
public class WeaponTypeSO : ScriptableObject
{
  public string Title;
  public Sprite Image;
  public int ImageSize;
  public WeaponUpgradeTemplate[] WeaponUpgrades;

  public void CopyOf(WeaponTypeSO other)
  {
    Title = other.Title;
    Image = other.Image;
    ImageSize = other.ImageSize;
    WeaponUpgrades = other.WeaponUpgrades;
  }
}

[System.Serializable]
public class WeaponUpgradeTemplate 
{
  public int Level;
  public int Price;
  public int Damage;
  public int Accuracy;
  public int Rate;
  public int Ammo;
}
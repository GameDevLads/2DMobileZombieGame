using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradeControllerv2 : MonoBehaviour
{
  public FloatVariableSO Coins;
  public WeaponTypeStatusSO[] WeaponTypeStatusSOs;
  public ZombieSpecialtySO[] ZombieSpecialties;
  public GameEvent EventWeaponTypeChanged;
  public WeaponTypeTemplate WeaponTypeTemplate;
  public StatsTemplate StatsTemplate;
  public StatsTemplate StatsNextTemplate;
  public ZombieSpecialtyTemplate ZombieSpecialtyTemplate;
  private int CurrentIndex = 0;
  private int CurrentZombieIndex = 0;
  // Start is called before the first frame update
  void Start()
  {
    updateZombieIndex(WeaponTypeStatusSOs[CurrentIndex]);
    OnChange();
  }

  public void Upgrade()
  {
    Debug.Log("upgrade");
    if (StatsNextTemplate.Price.IsNull)
    {
      Debug.Log("No upgrade available");
      return;
    }
    if (Coins.Value < StatsNextTemplate.Price.Value)
    {
      Debug.Log("Not enough coin");
      return;
    }
    Coins.Value -= StatsNextTemplate.Price.Value;
    WeaponTypeStatusSOs[CurrentIndex].Level.Value++;
    updateStats(WeaponTypeStatusSOs[CurrentIndex]);
    EventWeaponTypeChanged.Raise();
  }

  public void Change(int i)
  {
    CurrentIndex = i;
    OnChange();
  }

  public void ChangeUp()
  {
    if (CurrentIndex + 1 >= WeaponTypeStatusSOs.Length)
      return;
    CurrentIndex++;
    updateZombieIndex(WeaponTypeStatusSOs[CurrentIndex]);
    OnChange();
  }

  public void ChangeDown()
  {
    if (CurrentIndex <= 0)
      return;
    CurrentIndex--;
    updateZombieIndex(WeaponTypeStatusSOs[CurrentIndex]);
    OnChange();
  }

  public void ChangeLeft()
  {
    if (CurrentZombieIndex <= 0)
      return;
    CurrentZombieIndex--;
    OnChange();
  }

  public void ChangeRight()
  {
    if (CurrentZombieIndex >= ZombieSpecialties.Length - 1)
      return;
    CurrentZombieIndex++;
    OnChange();
  }

  public void OnChange()
  {
    var CurrentWeaponTypeStatus = WeaponTypeStatusSOs[CurrentIndex];
    updateWeaponType(CurrentWeaponTypeStatus);
    updateZombieType(CurrentWeaponTypeStatus);
    updateStats(CurrentWeaponTypeStatus);
    EventWeaponTypeChanged.Raise();
  }

  private void updateZombieType(WeaponTypeStatusSO CurrentWeaponTypeStatus)
  {
    CurrentWeaponTypeStatus.ZombieSpecialty = ZombieSpecialties[CurrentZombieIndex];
    ZombieSpecialtyTemplate.Title.Value = CurrentWeaponTypeStatus.ZombieSpecialty.Name;
    ZombieSpecialtyTemplate.Sprite.Value = CurrentWeaponTypeStatus.ZombieSpecialty.Sprite;
  }

  private void updateZombieIndex(WeaponTypeStatusSO CurrentWeaponTypeStatus)
  {
    for (int i = 0; i < ZombieSpecialties.Length; i++)
    {
      if (CurrentWeaponTypeStatus.ZombieSpecialty.Name.Equals(ZombieSpecialties[i].Name))
      {
        CurrentZombieIndex = i;
        break;
      }
    }
  }

  private void updateWeaponType(WeaponTypeStatusSO CurrentWeaponTypeStatus)
  {
    WeaponTypeTemplate.StringVariable.Value = CurrentWeaponTypeStatus.WeaponType.Title;
    WeaponTypeTemplate.SpriteVariable.Value = CurrentWeaponTypeStatus.WeaponType.Image;
  }

  private void updateStats(WeaponTypeStatusSO currentWeaponTypeStatus)
  {
    var currentLevel = currentWeaponTypeStatus.Level.Value;
    var upgrade = currentWeaponTypeStatus.WeaponType.WeaponUpgrades[currentLevel];
    updateStatsTemplate(StatsTemplate, upgrade);
    updateNextStats(currentLevel, currentWeaponTypeStatus);
  }

  private void updateStatsTemplate(StatsTemplate statsTemplate, WeaponUpgradeTemplate upgradeTemplate)
  {
    statsTemplate.Damage.Value = upgradeTemplate.Damage;
    statsTemplate.Accuracy.Value = upgradeTemplate.Accuracy;
    statsTemplate.Rate.Value = upgradeTemplate.Rate;
    statsTemplate.Ammo.Value = upgradeTemplate.Ammo;
    statsTemplate.Level.Value = upgradeTemplate.Level;
    statsTemplate.Price.Value = upgradeTemplate.Price;
    unNullifyUpgrade(statsTemplate);
  }

  private void updateNextStats(int currentLevel, WeaponTypeStatusSO currentWeaponTypeStatus)
  {
    if (currentLevel + 1 < currentWeaponTypeStatus.WeaponType.WeaponUpgrades.Length)
    {
      var nextUpgrade = currentWeaponTypeStatus.WeaponType.WeaponUpgrades[currentLevel + 1];
      updateStatsTemplate(StatsNextTemplate, nextUpgrade);
    }
    else
    {
      nullifyUpgrade(StatsNextTemplate);
    }
  }

  private void unNullifyUpgrade(StatsTemplate statsTemplate)
  {
    statsTemplate.Damage.IsNull = false;
    statsTemplate.Accuracy.IsNull = false;
    statsTemplate.Rate.IsNull = false;
    statsTemplate.Ammo.IsNull = false;
    statsTemplate.Level.IsNull = false;
    statsTemplate.Price.IsNull = false;
  }

  private void nullifyUpgrade(StatsTemplate statsTemplate)
  {
    statsTemplate.Damage.IsNull = true;
    statsTemplate.Accuracy.IsNull = true;
    statsTemplate.Rate.IsNull = true;
    statsTemplate.Ammo.IsNull = true;
    statsTemplate.Level.IsNull = true;
    statsTemplate.Price.IsNull = true;
  }
}

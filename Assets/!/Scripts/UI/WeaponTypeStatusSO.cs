using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Weapon Type Status")]
public class WeaponTypeStatusSO : ScriptableObject
{
  public WeaponTypeSO WeaponType;
  public IntVariableSO Level;
  public ZombieSpecialtySO ZombieSpecialty;
}

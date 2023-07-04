using Assets.__.Scripts.SO.Player;
using UnityEngine;
using UnityEngine.UI;

public class HUDWeaponSelect : MonoBehaviour
{
    public GameEvent WeaponSwitchEvent;

    [Tooltip("The players weapon slot to show on the HUD.")]
    public PlayerWeaponSO PlayerWeaponSlot;

    [Tooltip("The players selected weapon.")]
    public PlayerWeaponSO PlayerSelectedWeapon;

    void Start()
    {
        var button = this.GetComponent<Button>();
        button.onClick.AddListener(OnWeaponSelect);
        InitWeaponSlotSpriteRender();
    }

    private void OnWeaponSelect()
    {
        PlayerSelectedWeapon.Weapon = PlayerWeaponSlot.Weapon;
        WeaponSwitchEvent.Raise();
    }

    private void InitWeaponSlotSpriteRender()
    {
        if (PlayerWeaponSlot == null || PlayerWeaponSlot.Weapon == null)
            return;
        
        var weaponSpriteRenderer = PlayerWeaponSlot.Weapon.GetComponent<SpriteRenderer>();
        var currentImage = this.gameObject.GetComponent<Image>();
        currentImage.sprite = weaponSpriteRenderer.sprite;
    }
}
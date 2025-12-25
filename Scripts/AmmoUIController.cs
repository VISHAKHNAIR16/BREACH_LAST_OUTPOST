using UnityEngine;
using TMPro;

public class AmmoUIController : MonoBehaviour
{
    public TextMeshProUGUI currentAmmoText;
    public TextMeshProUGUI reserveAmmoText;
    public WeaponShoot weapon;   // reference to your gun script

    void Update()
    {
        if (weapon == null) return;

        currentAmmoText.text = weapon.currentAmmo.ToString();
        reserveAmmoText.text = weapon.reserveAmmo.ToString();
    }
}

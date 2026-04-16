using UnityEngine;
using InfimaGames.LowPolyShooterPack;

public class WeaponWithAmmoManager : Weapon
{
    private AmmoManager ammoManager;

    protected override void Awake()
    {
        base.Awake();
        
        IGameModeService gameModeService = ServiceLocator.Current.Get<IGameModeService>();
        CharacterBehaviour character = gameModeService.GetPlayerCharacter();
        ammoManager = character.GetComponent<AmmoManager>();
    }

    public override void Fire(float spreadMultiplier = 1.0f)
    {
        bool hasInfiniteAmmo = InfiniteAmmoButton.HasInfiniteAmmo();
        
        if (hasInfiniteAmmo)
        {
            base.Fire(spreadMultiplier);
            int magazineSize = GetAmmunitionTotal();
            base.FillAmmunition(magazineSize);
        }
        else
        {
            base.Fire(spreadMultiplier);
        }
    }

    public override bool HasAmmunition()
    {
        if (InfiniteAmmoButton.HasInfiniteAmmo())
            return true;
        
        return base.HasAmmunition();
    }

    public override void FillAmmunition(int amount)
    {
        if (ammoManager == null)
        {
            base.FillAmmunition(amount);
            return;
        }

        int currentAmmo = GetAmmunitionCurrent();
        int magazineSize = GetAmmunitionTotal();

        if (amount == 0)
        {
            int ammoToAdd = ammoManager.CalculateReloadAmount(currentAmmo, magazineSize);
            
            if (ammoToAdd > 0)
            {
                base.FillAmmunition(ammoToAdd);
            }
        }
        else
        {
            base.FillAmmunition(amount);
        }
    }
}

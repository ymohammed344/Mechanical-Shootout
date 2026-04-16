using UnityEngine;

public class GrenadeManager : MonoBehaviour
{
    private int currentGrenades = 0;
    private const int MAX_GRENADES = 2;

    void Start()
    {
        CheckAndApplyGrenades();
    }

    void CheckAndApplyGrenades()
    {
        if (PlayerPrefs.GetInt("GrenadesPurchased", 0) == 1)
        {
            currentGrenades = MAX_GRENADES;
            
            PlayerPrefs.SetInt("GrenadesPurchased", 0);
            PlayerPrefs.Save();

            Debug.Log($"Grenades loaded! Starting with {currentGrenades} grenades.");
        }
    }

    public bool HasGrenades()
    {
        return currentGrenades > 0;
    }

    public void UseGrenade()
    {
        if (currentGrenades > 0)
        {
            currentGrenades--;
            Debug.Log($"Grenade used! Remaining: {currentGrenades}");
        }
    }

    public int GetGrenadeCount()
    {
        return currentGrenades;
    }

    public int GetMaxGrenades()
    {
        return MAX_GRENADES;
    }
}

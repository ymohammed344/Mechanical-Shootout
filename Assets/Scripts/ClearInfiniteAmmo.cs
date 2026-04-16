using UnityEngine;

public class ClearInfiniteAmmo : MonoBehaviour
{
    void Start()
    {
        PlayerPrefs.SetInt("InfiniteAmmoActive", 0);
        PlayerPrefs.Save();
        Debug.Log("Infinite Ammo flag cleared!");
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Bringupsettings : MonoBehaviour
{
    public GameObject setting;
    public bool issettingactive;

    void Update()
    {
      if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(issettingactive == false)
            {
                Pause();
            }

            else
            {
                Resume();
            }
        }   
    }

    public void Pause()
    {
        setting.SetActive(true);
        issettingactive = true;
        this.GetComponent<MouseLook>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume()
    {
        setting.SetActive(false);
        issettingactive = false;
        this.GetComponent<MouseLook>().enabled = true;
        Cursor.lockState = CursorLockMode.None;
    }
}

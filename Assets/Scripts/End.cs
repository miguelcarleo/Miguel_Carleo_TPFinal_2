using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public GameObject victoryScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Time.timeScale = 0;
            GameManager.isActive = false;
            GameManager.UnLockCursor();
            victoryScene.SetActive(true);
        }
    }
}

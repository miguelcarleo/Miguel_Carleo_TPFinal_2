using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject startPanel;
    [HideInInspector] public static bool isActive = false;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }
    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 0;
        startPanel.SetActive(true);
    }
    void Update()
    {
        if (isActive)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (!pausePanel.activeInHierarchy)
                {
                    PauseGame();
                }
                else if (pausePanel.activeInHierarchy)
                {
                    ContinueGame();
                }
            }
        }
    }
    private void PauseGame()
    {
        isActive = false;
        Time.timeScale = 0;
        pausePanel.SetActive(true);
        UnLockCursor();
    }
    private void ContinueGame()
    {
        isActive = true;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
        LockCursor();
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public static void UnLockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        isActive = true;
        Time.timeScale = 1;
        LockCursor();
    }

    public void Restart()
    {
        isActive = false;
        SceneManager.LoadScene("SampleScene");
    }
}

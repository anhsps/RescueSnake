using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public bool soundEnabled { get; private set; }

    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private AudioSource[] audioSource;
    [SerializeField] private GameObject musicButton, muteButton;

    private CanvasScaler canvasScaler;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null && instance != this)
            Destroy(this);
        else
            instance = this;

        levelText.text = "Level " + SceneManager.GetActiveScene().name;

        soundEnabled = PlayerPrefs.GetInt("SoundEnabled", 1) == 1;
        UpdateSound();

        canvasScaler = GetComponentInParent<CanvasScaler>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Delete data");
    }

    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void NextLV() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    public void GameWin()
    {
        winMenu.SetActive(true);
        if (soundEnabled)
            audioSource[audioSource.Length - 1].Play();

        int currentLevel = int.Parse(SceneManager.GetActiveScene().name);
        UnlockNextLevel(currentLevel);
    }

    public void UnlockNextLevel(int currentLevel)
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        if (currentLevel >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    public void ToggleSound()
    {
        soundEnabled = !soundEnabled;
        PlayerPrefs.SetInt("SoundEnabled", soundEnabled ? 1 : 0);
        UpdateSound();
    }

    public void UpdateSound()
    {
        foreach (var item in audioSource)
            item.volume = soundEnabled ? 1f : 0f;

        musicButton.SetActive(soundEnabled);
        muteButton.SetActive(!soundEnabled);
    }

    public float AdjustSize(float size)
    {
        if (canvasScaler)
        {
            // Lấy tỷ lệ giữa độ phân giải thực tế và độ phân giải tham chiếu
            float scaleFactor = Mathf.Min(
                Screen.width / canvasScaler.referenceResolution.x,
                Screen.height / canvasScaler.referenceResolution.y
            );
            return size * scaleFactor * 8 / 3f;
        }
        else return size;
    }
}

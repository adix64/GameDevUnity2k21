using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMPro.TMP_Text playerName;
    public TMPro.TMP_InputField inputField;
    // Start is called before the first frame update
    void Start()
    {//obtinere date in memoria non-volatila (e.g. pe disc)
        playerName.text = PlayerPrefs.GetString("playerName", "nobody");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayBasicsGame()
    {
        SceneManager.LoadScene("Basics");
    }

    public void QuitBasicsGame()
    {
        Application.Quit();
    }
    public void SaveName()
    {//salvare date in memoria non-volatila (e.g. pe disc)
        PlayerPrefs.SetString("playerName", inputField.text);
        playerName.text = inputField.text;
    }
}

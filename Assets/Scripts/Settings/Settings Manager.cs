using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeReference]
    PlayerDataSO PDSO;

    bool Settings, Confirming;

    string location;

    [SerializeReference]
    GameObject Menu, Confirmer;

	[SerializeField]
	private AudioMixer mix = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Menu = transform.Find("BG").gameObject;
        Confirmer = transform.Find("Confirmer").gameObject;

        Confirm(false);
    }

    public void ToggleSettings()
    {
        Debug.Log("Settings");
        bool Settings = !Menu.activeSelf;
        Menu.SetActive(Settings);
    }

    public void MainMenu()
    {
        if (Confirming)
        {
            SceneManager.LoadScene("Main Menu");
        }
        else
        {
            location = "MainMenu";
            Confirm(true);
        }
    }

    public void Save()
    {
        if (Confirming)
        {
            PDSO.SaveGame();
        }
        else
        {
            location = "Save";
            Confirm(true);
        }
    }

    void Confirm(bool set)
    {
        Confirmer.SetActive(set);
    }

    public void ConfirmAns(bool ans)
    {
        Confirming = ans;

        if (ans)
        {
            SendMessage(location);
        }

        Confirm(false);
    }

	private void ChangeVolume(string which, float sliderValue)
	{
		float progress = Mathf.Log10(sliderValue) + 1;
		mix.SetFloat(which, Mathf.Lerp(-80, 0, progress));
	}

	public void OnMusicSliderValueChanged(float value)
    {
        ChangeVolume("Music Volume", value);
    }

    public void OnSFXSliderValueChanged(float value)
    {
        ChangeVolume("SFX Volume", value);
    }
}

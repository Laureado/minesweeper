using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomController : MonoBehaviour {

    public InputField heightInput;
    public InputField widhtInput;
    public InputField mineInput;
    public Text warning;

    void Start()
    {
        heightInput.text = PlayerPrefs.GetInt("Height", 10).ToString();
        widhtInput.text = PlayerPrefs.GetInt("Width", 10).ToString();
        mineInput.text = PlayerPrefs.GetInt("Mines", 5).ToString();
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void SelectedGameEasy()
    {
        PlayerPrefs.SetInt("Height", 8);
        PlayerPrefs.SetInt("Width", 8);
        PlayerPrefs.SetInt("Mines", 10);
        SceneManager.LoadScene(2);
    }

    public void SelectedGameNormal()
    {
        PlayerPrefs.SetInt("Height", 16);
        PlayerPrefs.SetInt("Width", 16);
        PlayerPrefs.SetInt("Mines", 40);
        SceneManager.LoadScene(2);
    }

    public void SelectedGameHard()
    {
        PlayerPrefs.SetInt("Height", 16);
        PlayerPrefs.SetInt("Width", 30);
        PlayerPrefs.SetInt("Mines", 99);
        SceneManager.LoadScene(2);
    }

    public void SelectedGameCustom()
    {
        if (heightInput.text == "" || widhtInput.text == "" || mineInput.text == "")
        {
            warning.text = "No puedes dejar las casillas vacías";
            return;
        }
            

        int height = int.Parse(heightInput.text);
        int widht = int.Parse(widhtInput.text);
        int mine = int.Parse(mineInput.text);

        if (height < 3 || height > 20)
        {
            warning.text = "El altura solo puede ser de 3-20";
            return;
        }
            
        if (widht < 3 || widht > 30)
        {
            warning.text = "El ancho solo puede ser de 3-30";
            return;
        }
            
        if (mine >= height*widht || mine > 599 || mine < 1)
        {
            warning.text = "No puedes poner menos de una mina o más del debido.";
            return;
        }
           

        PlayerPrefs.SetInt("Height", height);
        PlayerPrefs.SetInt("Width", widht);
        PlayerPrefs.SetInt("Mines", mine);
        SceneManager.LoadScene(2);
    }
}

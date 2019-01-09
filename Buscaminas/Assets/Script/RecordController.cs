using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RecordController : MonoBehaviour {

    string filePath;
    string jsonString;
    ListaRecord listaRecords;
    byte lastDifficultyButton;

    public string dificultad;
    public Transform targetTransform;
    public RecordDisplayController recordDisplayPrefab;
    public GameObject confirmPanel;
    public GameObject eraseButton;
    public GameObject[] difficultyButton;

    void Awake()
    {
        dificultad = "facil";
        lastDifficultyButton = 0;
        filePath = Application.dataPath + "/records.json";
        jsonString = File.ReadAllText(filePath);
        listaRecords = JsonUtility.FromJson<ListaRecord>(jsonString);
        
        

        try
        {
            fillRecords(dificultad);
        }
        catch (System.NullReferenceException e)
        {
            print("Records no encontrados: " + e.Message);
        }
        //record.tiempo = 1;
        //jsonString = JsonUtility.ToJson(record);
        //File.WriteAllText(filePath, jsonString);
    }

    public void BackButton()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangePanelState()
    {
        if (!confirmPanel.activeSelf)
            confirmPanel.SetActive(true);
        else
            confirmPanel.SetActive(false);
    }

    public void EraseButton()
    {
        ListaRecord newList = new ListaRecord();
        File.WriteAllText(filePath, "");

        jsonString = JsonUtility.ToJson(newList);
        File.WriteAllText(filePath, jsonString);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void facilSelect()
    {
        changeButtonColor(0);
        eraseAllRecords("facil");
    }

    public void normalSelect()
    {
        changeButtonColor(1);
        eraseAllRecords("normal");
    }

    public void dificilSelect()
    {
        changeButtonColor(2);
        eraseAllRecords("dificil");
    }

    void changeButtonColor(byte dificultadButton)
    {
        Image img = difficultyButton[lastDifficultyButton].GetComponent<Image>();
        img.color = Color.white;

        img = difficultyButton[dificultadButton].GetComponent<Image>();
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#A5A7D0FF", out color);
        img.color = color;
      
        lastDifficultyButton = dificultadButton;
    }

    public void eraseAllRecords(string dificultad)
    {
        object[] AllRecords = GameObject.FindGameObjectsWithTag("RecordPanel");
        foreach (GameObject recordPanel in AllRecords)
        {
            Destroy(recordPanel);
        }
        fillRecords(dificultad);
    }

    public void fillRecords(string dificultad)
    {
        int elNumeroXD = 1;
        List<Record> lr = listaRecords.records.FindAll(a => a.dificultad == dificultad);
        foreach (Record record in lr)
        {
            eraseButton.SetActive(true);
            RecordDisplayController displayer = (RecordDisplayController)Instantiate(recordDisplayPrefab);
            displayer.transform.SetParent(targetTransform, false);
            displayer.Prime(record, elNumeroXD);
            elNumeroXD++;
        }
    }
}

[System.Serializable]
public class Record
{
    public string dificultad;
    public int tiempo;

    public override string ToString()
    {
        return string.Format("En dificultad {0}: {1}",dificultad, tiempo);
    }
}

[System.Serializable]
public class ListaRecord
{
    public List<Record> records;

}
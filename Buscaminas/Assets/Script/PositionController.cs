using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PositionController : MonoBehaviour {

    public int numberMines = 5;
    public static byte heightMines;
    public static byte widthMines;
    public static bool[,] matrizMines;
    public static int avaliableFlags;
    public bool firsClickMine;
    public Sprite mineSprite;

    public GameObject BoxPrefab;
    public Button restartButton;
    public Text countText;
    public Text secondText;

    int discoveredCount;
    string filePath;
    string jsonString;
    float seconds;
    GameObject go;
    ListaRecord listaRecords;
    BoxController[,] allBoxes;

    enum BoxState { Hidden, Revealed, Flag, Mined };

    void Awake()
    {
        filePath = Application.dataPath + "/records.json";
        jsonString = File.ReadAllText(filePath);
        listaRecords = JsonUtility.FromJson<ListaRecord>(jsonString);

        try
        {
        }
        catch (System.NullReferenceException e)
        {
            print("Records no encontrados: " + e.Message);
        }
    }

    void Start() {

        //Asignando numeros de altura, ancho, y cantidad de minas
        heightMines = (byte) PlayerPrefs.GetInt("Height", 2);
        widthMines = (byte) PlayerPrefs.GetInt("Width", 2);
        numberMines = (byte)PlayerPrefs.GetInt("Mines", 1);
        countText.text = numberMines.ToString();
        avaliableFlags =  numberMines;
        firsClickMine = true;

       //Asignando posicion de las minas
        matrizMines = new bool[heightMines,widthMines];
         allBoxes = new BoxController[heightMines,widthMines];

        int placedMines = 0;
        bool firstTime = true;

        while(placedMines < numberMines)
        {
            for (int y = 0; y <= heightMines - 1; y++)
            {
                for (int x = 0; x <= widthMines - 1; x++)
                {

                    //Se crea el cuadro y aparece en pantalla
                    if (firstTime)
                    {
                        go = Instantiate(BoxPrefab, new Vector3((-1f * (widthMines / 2)) + x, (1f * (heightMines / 2)) - y, 0f), Quaternion.identity);
                        allBoxes[y, x] = go.GetComponent<BoxController>();
                        allBoxes[y, x].boxPositionHeight = y;
                        allBoxes[y, x].boxPositionWidth = x;
                    }                

                    if (placedMines < numberMines && allBoxes[y, x].mined == false)
                    {
                        //Se asigna si está minado o no
                        if (Random.Range(0, 10) == 1)
                        {
                            matrizMines[y, x] = true;
                            placedMines++;
                            allBoxes[y, x].mined = true;
                        }

                    }
                }
            }
            firstTime = false;
        }        

    }

    private void Update()
    {
        if(restartButton.isActiveAndEnabled == false)
          UpdateTimer();
    }

    void UpdateTimer()
    {
        seconds += Time.deltaTime;
        secondText.text = seconds.ToString("000");
    }

    public void changeMine(BoxController bc)
    {

        if (firsClickMine )
        {
            while (firsClickMine && bc.mined)
            {
                for (int y = 0; y <= heightMines - 1; y++)
                {
                    for (int x = 0; x <= widthMines - 1; x++)
                    {
                        if (allBoxes[y, x].mined == false && firsClickMine && Random.Range(0, 10) == 1)
                        {
                            firsClickMine = false;
                            matrizMines[y, x] = true;
                            allBoxes[y, x].mined = true;
                        }
                    }
                }
            }
                
            bc.mined = false;
            matrizMines[bc.boxPositionHeight, bc.boxPositionWidth] = false;
            firsClickMine = false;
        }
        
    }

    public void SumarDescubiertos()
    {
        discoveredCount++;
        if (discoveredCount == (heightMines * widthMines) - numberMines)
            WonGame();
    }

    public void GameOver()
    {
        Collider2D col2;
        BoxController boxScript;
        SpriteRenderer boxSprite;
        Object[] allBox = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject boxGO in allBox)
        {
            col2 = boxGO.GetComponent<BoxCollider2D>();
            boxScript = boxGO.GetComponent<BoxController>();
            boxSprite = boxGO.GetComponent<SpriteRenderer>();

            col2.enabled = false;
            
            if (boxScript.GetFlagged() && boxScript.mined == false)
            {
                boxSprite.color = Color.red;
            }
            else if (boxScript.mined && !boxScript.GetFlagged())
            {
                boxSprite.sprite = mineSprite;
            }
                

        }

        restartButton.gameObject.SetActive(true);
    }

    void WonGame ()
    {
        string dificultad = "";
        restartButton.GetComponentInChildren<Text>().text = "¡Ganaste!";
        ListaRecord newList = new ListaRecord();
        newList.records = new List<Record>();

        if (widthMines == 8 && heightMines == 8 && numberMines == 10)
        {
            dificultad = "facil";
        } 
        else if (widthMines == 16 && heightMines == 16 && numberMines == 40)
        {
            dificultad = "normal";
        }
        else if (widthMines == 30 && heightMines == 16 && numberMines == 99)
        {
            dificultad = "dificil";
        }

        if(dificultad != "")
        {
            if(listaRecords.records.Count != 0)
            {
                foreach (Record record in listaRecords.records)
                {
                    newList.records.Add(record);
                }
            }
            newList.records.Add(new Record() { dificultad = dificultad, tiempo = (int)seconds });

            //newList.records.Sort((a, b) => (a.tiempo.ToString().CompareTo(b.tiempo.ToString())));
            newList.records.Sort(delegate(Record a, Record b){
                return a.tiempo.CompareTo(b.tiempo);
            });

            List<Record> lr = newList.records.FindAll(a => a.dificultad == dificultad);
            if(lr.Count > 12)
            {
                newList.records.Remove(newList.records.Find(a => a.dificultad == lr[12].dificultad && a.tiempo == lr[12].tiempo));
            }

            jsonString = JsonUtility.ToJson(newList);
            File.WriteAllText(filePath, jsonString);

        }
        GameOver();
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("lvl0");
    }

    public void GoBack()
    {
        SceneManager.LoadScene(1);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxController : MonoBehaviour {

    public int boxPositionHeight;
    public int boxPositionWidth;
    public bool mined;
    public Sprite[] mineSprites;
    PositionController position;
    byte minesAround;

    enum BoxState { Hidden, Revealed, Flag, Mined};

     BoxState boxState = BoxState.Hidden;

    SpriteRenderer sprite;
	
	void Start () {
        sprite = GetComponent<SpriteRenderer>();
        GameObject go = GameObject.FindWithTag("Controller");
        position = go.GetComponent<PositionController>();
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
            revelarBox();

        if (Input.GetMouseButtonDown(1))
            FlagBox();

    }

    public bool GetFlagged()
    {
        bool isFlagged = false;
        if (boxState == BoxState.Flag)
            isFlagged = true;

        return isFlagged;
    }

    void revelarBox()
    {
        position.changeMine(this);

        if (boxState == BoxState.Hidden)
        {
            if (mined)
            {
                sprite.color = Color.red;
                position.GameOver();
            }
            else
            {
                position.SumarDescubiertos();
                int minasRodeando = conocerCantidad();
                if (minasRodeando != 0)
                {
                    minesAround = (byte) minasRodeando;
                    sprite.sprite = mineSprites[minasRodeando + 2];
                    boxState = BoxState.Revealed;
                }
                else
                {
                    sprite.sprite = mineSprites[0];
                }
            }
        }
        else if(boxState == BoxState.Revealed)
        {
            byte flagCount = 0;
            BoxController boxScript;
            Object[] allBox = GameObject.FindGameObjectsWithTag("Box");
            foreach (GameObject boxGO in allBox)
            {
                boxScript = boxGO.GetComponent<BoxController>();
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (!(boxPositionHeight + y < 0 || boxPositionWidth + x < 0 || boxPositionHeight + y >= PositionController.heightMines || boxPositionWidth + x >= PositionController.widthMines) && (boxScript.boxPositionHeight == boxPositionHeight + y && boxScript.boxPositionWidth == boxPositionWidth + x && boxScript.boxState == BoxState.Flag))
                        {
                            flagCount++;
                        }
                    }
                }
            }

            if (flagCount == minesAround)
                revealAroundBoxes();
        }
    }

  

    void FlagBox()
    {
        if (boxState == BoxState.Flag)
        {
            boxState = BoxState.Hidden;
            sprite.sprite = mineSprites[11];
            PositionController.avaliableFlags++;
        }
        else if (boxState == BoxState.Hidden && PositionController.avaliableFlags > 0)
        {
            boxState = BoxState.Flag;
            sprite.sprite = mineSprites[1];
            PositionController.avaliableFlags--;
        }
        position.countText.text = PositionController.avaliableFlags.ToString();
    }

    void revealAroundBoxes()
    {
        boxState = BoxState.Revealed;
        BoxController boxScript;
        Object[] allBox = GameObject.FindGameObjectsWithTag("Box");
        foreach (GameObject boxGO in allBox)
        {
            boxScript = boxGO.GetComponent<BoxController>();
            for (int y = -1; y <= 1; y++)
            {
                for (int x = -1; x <= 1; x++)
                {
                    if (!(boxPositionHeight + y < 0 || boxPositionWidth + x < 0 || boxPositionHeight + y >= PositionController.heightMines || boxPositionWidth + x >= PositionController.widthMines) && (boxScript.boxPositionHeight == boxPositionHeight + y && boxScript.boxPositionWidth == boxPositionWidth + x && boxScript.boxState == BoxState.Hidden))
                    {
                        boxScript.revelarBox();
                    }
                }
            }
        }
    }

    int conocerCantidad()
    {
        int cantidadMinas = 0;

        for(int y = -1; y <= 1; y++)
        {
            for(int x = -1; x <= 1; x++)
            {
                if(!(boxPositionHeight + y < 0 || boxPositionWidth + x < 0 || boxPositionHeight + y >= PositionController.heightMines  || boxPositionWidth + x  >= PositionController.widthMines))
                {
                    if (PositionController.matrizMines[boxPositionHeight + y, boxPositionWidth + x])
                    {        
                        cantidadMinas++;
                    }
                }              
            }
        }
        
        if(cantidadMinas == 0)
        {
            revealAroundBoxes();
        }

        return cantidadMinas;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RecordDisplayController : MonoBehaviour {

    public Text difficulty;
    public Text time;
    public Text number;

    public Record record;
    public int numero;

	// Use this for initialization
	void Start () {
        if (record != null) Prime(record, numero);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Prime(Record record, int numero)
    {
        this.record = record;
        this.numero = numero;

        if (difficulty != null) difficulty.text = record.dificultad;
        if (difficulty != null) time.text = record.tiempo.ToString();
        if (difficulty != null) number.text = numero.ToString();
    }
}

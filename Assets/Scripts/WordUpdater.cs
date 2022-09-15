using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WordUpdater : MonoBehaviour
{
    public TextMeshProUGUI Kanji;
    public TextMeshProUGUI Furigana;
    public TextMeshProUGUI Meaning;
    
    List<Word> WordList = new List<Word>();
    int index = 0;
    // Start is called before the first frame update
    void Awake() {
        WordList.Add(new Word("急ぐ","いそぐ","서두르다"));
        WordList.Add(new Word("糸","いと","실"));
        WordList.Add(new Word("都会","とかい","도시"));
        WordList.Add(new Word("都合","つごう","사정, 형편"));
    }

    void UpdateWord()
    {
        Word word = WordList[index];
        Kanji.text = word.Kanji;
        Furigana.text = word.Furigana;
        Meaning.text = word.Meaning;
    }

    private void Start() {
        UpdateWord();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreviousWord()
    {
        if(index > 0)
        {
            --index;
            UpdateWord();
        }
    }

    public void NextWord()
    {
        if(index < WordList.Count - 1)
        {
            ++index;
            UpdateWord();
        }
    }
}

class Word
{
    public string Kanji { get; set; }
    public string Furigana { get; set; }
    public string Meaning { get; set; }
    public Word(string Kanji, string Furigana, string Meaning)
    {
        this.Kanji = Kanji;
        this.Furigana = Furigana;
        this.Meaning = Meaning;
    }
}
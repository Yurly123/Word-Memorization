using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;
using System.IO;
using System.Linq;

public class WordUpdater : MonoBehaviour
{
    public TextMeshProUGUI KanjiText;
    public TextMeshProUGUI FuriganaText;
    public TextMeshProUGUI MeaningText;
    
    List<Word> WordList = new List<Word>();
    int index = 0;
    // Start is called before the first frame update
    void Awake() {
        WordList.Add(new Word("青い", "あおい", "파랗다"));
        WordList.Add(new Word("味", "あじ", "맛"));
        RandomizeWordList();
    }

    void UpdateWord(int index)
    {
        Word word = WordList[index];
        KanjiText.text = word.Kanji;
        FuriganaText.text = word.Furigana;
        MeaningText.text = word.Meaning;
    }

    private void Start() {
        UpdateWord(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreviousWord()
    {
        if(index > 0)
        {
            UpdateWord(--index);
        }
    }

    public void NextWord()
    {
        if(index < WordList.Count - 1)
        {
            UpdateWord(++index);
        }
    }

    public void RandomizeWordList()
    {
        LoadWordList();
        for (int i = 0; i < WordList.Count; ++i)
        {
            int RandIndex = Random.Range(0, WordList.Count);
            Word temp = WordList[i];
            WordList[i] = WordList[RandIndex];
            WordList[RandIndex] = temp;
        }
        index = 0;
        UpdateWord(index);
    }

    void LoadWordList()
    {
        XmlDocument Document = new XmlDocument();
        List<Word> XmlWordList = new List<Word>();
        try
        {
            Document.Load(Application.persistentDataPath + @"\Words.xml");
            XmlElement Words = Document["Words"];

            foreach (XmlElement Word in Words.ChildNodes)
            {
                Word word = new Word();
                word.Kanji = Word.GetAttribute("한자");
                word.Furigana = Word.GetAttribute("후리가나");
                word.Meaning = Word.GetAttribute("의미");
                XmlWordList.Add(word);
            }
        }
        catch { }

        try
        {
            Document.Load(Application.dataPath + @"\Words.xml");
            XmlElement Words = Document["Words"];

            foreach (XmlElement Word in Words.ChildNodes)
            {
                Word word = new Word();
                word.Kanji = Word.GetAttribute("한자");
                word.Furigana = Word.GetAttribute("후리가나");
                word.Meaning = Word.GetAttribute("의미");
                XmlWordList.Add(word);
            }
        }
        catch { }

        foreach (Word XmlWord in XmlWordList)
        {
            var OverlappingWords = from Word in WordList
                                   where Word.Kanji == XmlWord.Kanji
                                   where Word.Furigana == XmlWord.Furigana
                                   where Word.Meaning == Word.Meaning
                                   select Word;
            if (OverlappingWords.Count() == 0)
            {
                WordList.Add(XmlWord);
            }
        }
        SaveWordList();
    }
    void SaveWordList()
    {
        XmlDocument Document = new XmlDocument();
        XmlElement Words = Document.CreateElement("Words");
        Document.AppendChild(Words);
 
        foreach (var Word in WordList)
        {
            XmlElement WordElement = Document.CreateElement("Word");
            WordElement.SetAttribute("한자", Word.Kanji);
            WordElement.SetAttribute("후리가나", Word.Furigana);
            WordElement.SetAttribute("의미", Word.Meaning);
            Words.AppendChild(WordElement);
        }

        Document.Save(Application.persistentDataPath + @"\Words.xml");
        Document.Save(Application.dataPath + @"\Words.xml");
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
    public Word() : this(string.Empty, string.Empty, string.Empty) { }
}
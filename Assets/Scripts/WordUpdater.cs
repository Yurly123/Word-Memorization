using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class WordUpdater : MonoBehaviour
{
    public TextMeshProUGUI KanjiText;
    public TextMeshProUGUI FuriganaText;
    public TextMeshProUGUI MeaningText;
    public TextMeshProUGUI WordIndexText;
    public GameObject CheckMark;
    public TextMeshProUGUI CheckFilterText;
    
    List<Word> WordList = new List<Word>();
    int index = 0;
    bool CheckFilter = false;
    // Start is called before the first frame update
    void Awake() {
        WordList = WordXmlIO.LoadWordList();
        RandomizeWordList();
    }

    void UpdateWord(int index)
    {
        if (CheckFilter)
        {
            while (!WordList[index].Checked)
            {
                ++index;
                if (index >= WordList.Count())
                {
                    index = 0;
                }
                if (index == this.index)
                {
                    CheckedFilter();
                    break;
                }
            }
            this.index = index;
        }
        Word word = WordList[index];
        KanjiText.text = word.Kanji;
        FuriganaText.text = word.Furigana;
        MeaningText.text = word.Meaning;
        WordIndexText.text = (index + 1).ToString();
        UpdateCheckMark(index);
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
        if(index <= 0)
            index = WordList.Count;
        if (CheckFilter)
        {
            --index;
            int OriginalIndex = index;
            while (!WordList[index].Checked)
            {
                --index;
                if (index < 0)
                {
                    index = WordList.Count() - 1;
                }
                if (index == OriginalIndex)
                {
                    CheckedFilter();
                    break;
                }
            }
            ++index;
        }
        UpdateWord(--index);
    }

    public void NextWord()
    {
        if(index >= WordList.Count() - 1)
            index = -1;
        UpdateWord(++index);
    }

    public void RandomizeWordList()
    {
        WordXmlIO.SaveWordList(WordList);
        WordList = WordXmlIO.LoadWordList();
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

    void UpdateCheckMark(int index)
    {
        if (WordList[index].Checked)
        {
            CheckMark.SetActive(true);
        }
        else
        {
            CheckMark.SetActive(false);
        }
    }
    public void CheckCurrentWord()
    {
        if (WordList[index].Checked)
        {
            WordList[index].Checked = false;
        }
        else
        {
            WordList[index].Checked = true;
        }
        UpdateCheckMark(index);
    }

    private void OnApplicationQuit() {
        WordXmlIO.SaveWordList(WordList);
    }

    public void CheckedFilter()
    {
        if (CheckFilter)
        {
            CheckFilterText.text = "☆만 표시 : X";
            CheckFilterText.color = new Color(1, 1, 1, 1);
            CheckFilter = false;
        }
        else
        {
            CheckFilterText.text = "☆만 표시 : O";
            CheckFilterText.color = new Color(1, 1, 0, 1);
            CheckFilter = true;
        }
        UpdateWord(index);
    }
}

public class Word
{
    public string Kanji { get; set; }
    public string Furigana { get; set; }
    public string Meaning { get; set; }
    public bool Checked { get; set;}
    public Word(string Kanji, string Furigana, string Meaning)
    {
        this.Kanji = Kanji;
        this.Furigana = Furigana;
        this.Meaning = Meaning;
    }
    public Word() : this(string.Empty, string.Empty, string.Empty) { }
}
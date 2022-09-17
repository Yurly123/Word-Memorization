using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.IO;
using System.Linq;

public class WordXmlIO : MonoBehaviour
{
    public static List<Word> LoadWordList()
    {
        XmlDocument Document = new XmlDocument();
        List<Word> WordList = new List<Word>();
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
                word.Checked = (Word.GetAttribute("별표") == "Checked");
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
                word.Checked = (Word.GetAttribute("별표") == "Checked");
                XmlWordList.Add(word);
            }
        }
        catch { }

        foreach (Word XmlWord in XmlWordList)
        {
            var OverlappingWords = from Word in WordList
                                   where Word.Kanji == XmlWord.Kanji
                                   select Word;
            if (OverlappingWords.Count() == 0)
            {
                WordList.Add(XmlWord);
            }
        }
        SaveWordList(WordList);
        return WordList;
    }
    
    public static void SaveWordList(List<Word> WordList)
    {
        WordList = WordList.OrderBy(Word => Word.Furigana).ToList();
        XmlDocument Document = new XmlDocument();
        XmlElement Words = Document.CreateElement("Words");
        Document.AppendChild(Words);
 
        foreach (var Word in WordList)
        {
            XmlElement WordElement = Document.CreateElement("Word");
            WordElement.SetAttribute("한자", Word.Kanji);
            WordElement.SetAttribute("후리가나", Word.Furigana);
            WordElement.SetAttribute("의미", Word.Meaning);
            if (Word.Checked) WordElement.SetAttribute("별표", "Checked");
            else WordElement.SetAttribute("별표", string.Empty);
            Words.AppendChild(WordElement);
        }

        Document.Save(Application.persistentDataPath + @"\Words.xml");
        Document.Save(Application.dataPath + @"\Words.xml");
    }
}

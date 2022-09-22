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
            Document.Load(Application.persistentDataPath + "/Words.xml");
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
        catch
        {
            if (!Directory.Exists(Application.persistentDataPath))
                Directory.CreateDirectory(Application.persistentDataPath);
        }
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

        try
        {
            Document.Load(Application.persistentDataPath + "/properties/CheckList.xml");
            XmlElement Words = Document["Words"];

            foreach (XmlElement Word in Words.ChildNodes)
            {
                var CheckedWord =  (from word in WordList
                                    where word.Kanji == Word.GetAttribute("한자")
                                    select word).First();
                CheckedWord.Checked = true;
            }
        }
        catch
        {
            if (!Directory.Exists(Application.persistentDataPath + "/properties/"))
                Directory.CreateDirectory(Application.persistentDataPath + "/properties/");
        }

        return WordList;
    }
    
    public static void SaveWordList(List<Word> WordList)
    {
        if (WordList.Count() == 0)
            WordList.Add(new Word("/storage/emulated/0\n/Android/data\n/com.DefaultCompany.com.unity.template.mobile2D\n/files/Words.xml", "Word load failed","Words.xml 파일을 다음 경로에 붙여주세요"));
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
            Words.AppendChild(WordElement);
        }
        try { Document.Save(Application.persistentDataPath + "/Words.xml"); }
        catch { }

        Document = new XmlDocument();
        Words = Document.CreateElement("Words");
        Document.AppendChild(Words);
        foreach (var Word in from word in WordList
                             where word.Checked == true
                             select word)
        {
            XmlElement WordElement = Document.CreateElement("Word");
            WordElement.SetAttribute("한자", Word.Kanji);
            Words.AppendChild(WordElement);
        }
        try { Document.Save(Application.persistentDataPath + "/properties/CheckList.xml"); }
        catch { }

        Document = new XmlDocument();
        Words = Document.CreateElement("Words");
        Document.AppendChild(Words);
        foreach (var Word in WordList)
        {
            XmlElement WordElement = Document.CreateElement("Word");
            WordElement.SetAttribute("한자", Word.Kanji);
            WordElement.SetAttribute("후리가나", Word.Furigana);
            WordElement.SetAttribute("의미", Word.Meaning);
            WordElement.SetAttribute("의미", Word.Checked ? "Checked" : string.Empty);
            Words.AppendChild(WordElement);
        }
        try { Document.Save(Application.dataPath + "/Resources/Words.xml"); }
        catch { }
    }
}

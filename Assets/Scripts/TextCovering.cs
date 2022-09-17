using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextCovering : MonoBehaviour
{
    public Button CoverButton;

    Image ButtonImage;
    TextMeshProUGUI ButtonText;
    TextMeshProUGUI Text;
    bool IsCoverVisable = false;
    // Start is called before the first frame update
    void Start()
    {
        ButtonImage = CoverButton.GetComponent<Image>();
        ButtonText = CoverButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        Text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CoverClick()
    {
        if (IsCoverVisable)
        {
            Color temp;

            temp = ButtonImage.color;
            temp.a = 0;
            ButtonImage.color = temp;

            temp = ButtonText.color;
            temp.a = 0;
            ButtonText.color = temp;

            temp = Text.color;
            temp.a = 1;
            Text.color = temp;

            IsCoverVisable = false;
        }
        else
        {
            Color temp;

            temp = ButtonImage.color;
            temp.a = 1;
            ButtonImage.color = temp;

            temp = ButtonText.color;
            temp.a = 1;
            ButtonText.color = temp;

            temp = Text.color;
            temp.a = 0;
            Text.color = temp;

            IsCoverVisable = true;
        }
    }
}

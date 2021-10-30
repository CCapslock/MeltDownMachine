using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    private Text text;
    public static Log inst;

    private void Awake()
    {
        text = GetComponent<Text>();
        inst = this;
    }

    public void Print(string inText)
    {
        text.text += inText+"\n";
    }
}

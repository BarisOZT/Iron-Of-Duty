using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string speakerName;      // Konu�mac�n�n ad�
    [TextArea] public string line;  // Konu�ma metni
    public Sprite portrait;         // Konu�mac�n�n resmi
}

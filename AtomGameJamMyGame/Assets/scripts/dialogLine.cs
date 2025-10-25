using UnityEngine;

[System.Serializable]
public class DialogLine
{
    public string speakerName;      // Konuþmacýnýn adý
    [TextArea] public string line;  // Konuþma metni
    public Sprite portrait;         // Konuþmacýnýn resmi
}

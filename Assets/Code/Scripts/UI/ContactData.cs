using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewContact", menuName = "Dialogue System/Contact")]
public class ContactData : ScriptableObject
{
    public string contactName;
    public Sprite contactIcon;
    public TextAsset dialogueFile;
    public string eventCode;
}

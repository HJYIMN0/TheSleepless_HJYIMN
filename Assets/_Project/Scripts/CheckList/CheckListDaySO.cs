using UnityEngine;

[CreateAssetMenu(fileName = "CheckListDaySO", menuName = "ScriptableObjects/CheckListDaySO", order = 1)]
public class CheckListDaySO : ScriptableObject
{
    public bool needsSleep;
    public bool needsEat;
    public bool needsHygiene;
    public bool needsDirection;
    public bool needsIntegrity;

    public bool[] needs;

    public string[] needsDescriptions;
}

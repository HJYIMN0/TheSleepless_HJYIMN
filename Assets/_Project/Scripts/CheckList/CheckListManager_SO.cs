using System.Collections.Generic;
using UnityEngine;

public class CheckListManager_SO : MonoBehaviour
{
    public CheckListDaySO currentDayCheckList { get; private set; }

    public List<bool> currentDayActivities { get; private set; }

    private void OnEnable()
    {
        currentDayCheckList = GameManager.Instance.DaysSettings[GameManager.Instance.Day];
        SetCurrentDayCheckList(currentDayCheckList);
    }

    public bool AreAllActivitiesCompleted()
    {
        foreach (bool activity in currentDayActivities)
        {
            if (!activity)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsActivityCompleted(int index)
    {
        if (index < 0 || index >= currentDayActivities.Count)
        {
            Debug.LogError("Index out of range in IsActivityCompleted: " + index);
            return false;
        }
        return currentDayActivities[index];
    }

    public void SetActivityCompleted(int index)
    {
        if (index < 0 || index >= currentDayActivities.Count)
        {
            Debug.LogError("Index out of range in SetActivityCompleted: " + index);
            return;
        }
        if (IsActivityCompleted(index))
        {
            Debug.Log("Activity already completed at index: " + index);
            return;
        }
        currentDayActivities[index] = true;
    }

    public void SetCurrentDayCheckList(CheckListDaySO dayCheckList)
    {
        bool[] temporaryCheckList = new bool[5];
        temporaryCheckList[0] = dayCheckList.needsSleep;
        temporaryCheckList[1] = dayCheckList.needsEat;
        temporaryCheckList[2] = dayCheckList.needsHygiene;
        temporaryCheckList[3] = dayCheckList.needsDirection;
        temporaryCheckList[4] = dayCheckList.needsIntegrity;

        currentDayActivities = new List<bool>();

        foreach (bool activity in temporaryCheckList)
        {
            if (activity)
            {
                currentDayActivities.Add(false);
            }
        }
    }


}

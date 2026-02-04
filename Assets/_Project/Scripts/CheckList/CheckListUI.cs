using UnityEngine;
using DG.Tweening;
using TMPro;

public class CheckListUI : MonoBehaviour
{
    [SerializeField] private GameObject checkListPanel;
    [SerializeField] private GameObject listPanelParent;
    [SerializeField] private Ease checkListEase = Ease.OutBack;
    [SerializeField] private float xMovementOffset = 50f;
    [SerializeField] private float animationDuration = 0.5f;

    private bool isCheckListMoving;

    public void ToggleCheckList()
    {
        Debug.Log("Clicked CheckList Toggle");
        if (isCheckListMoving) return;
        isCheckListMoving = true;
        if (checkListPanel.activeSelf)
        {
            CloseCheckList();
        }
        else
        {
            OpenCheckList();
        }
    }
    private void OpenCheckList()
    {
        checkListPanel.SetActive(true);
        checkListPanel.transform.DOMoveX(-xMovementOffset, animationDuration).SetEase(checkListEase).OnComplete(() => isCheckListMoving = false);
        Debug.Log("Opened CheckList");
    }

    private void CloseCheckList()
    {
        checkListPanel.transform.DOMoveX(xMovementOffset, animationDuration).SetEase(checkListEase).OnComplete(() =>
        {
            checkListPanel.SetActive(false);
            isCheckListMoving = false;
        });
        Debug.Log("Closed CheckList");
    }

    public void PopulateCheckList(CheckListDaySO dayCheckList)
    {
        GameObject[] listText = new GameObject[dayCheckList.needsDescriptions.Length];

        for (int i = 0; i < listText.Length; i++)
        {
            TextMeshProUGUI text = listText[i].AddComponent<TextMeshProUGUI>();
            text.text = dayCheckList.needsDescriptions[i];
        }
    }
}

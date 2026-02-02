using UnityEngine;
using DG.Tweening;

public class CheckListUI : MonoBehaviour
{
    [SerializeField] private GameObject checkListPanel;
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
}

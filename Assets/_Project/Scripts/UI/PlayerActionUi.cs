using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
public class PlayerActionUi : MonoBehaviour
{

    [Header("DoTween properties")]
    [SerializeField] private LayerMask scaleLayer;
    [SerializeField] private float scaleUpFactor = 1.2f;
    [SerializeField] private float duration = 0.2f;

    [Header("Buttons")]
    [SerializeField] private Button menu;

    private Tween currentTween;
    private PlayerActionSystem _actions;
   
    private void Start()
    {
        _actions = PlayerActionSystem.Instance;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.layer != scaleLayer) return;

        Debug.Log("I'm Here!");
        currentTween?.Kill();
        currentTween = transform.DOScale(transform.localScale * scaleUpFactor, duration).SetEase(Ease.OutBack);
    }



    public void Work()
    {
        _actions.Work();
    }
}

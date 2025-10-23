using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonHoverEffect", menuName = "ScriptableObjects/UI/ButtonHoverEffect", order = 1)]
public class ButtonHoverSO : ScriptableObject
{
    [Header("Scale Settings")]
    public float scaleUpFactor = 1.2f;
    public float duration = 0.3f;
    public Ease easeType = Ease.InElastic;

    [Header("Move Settings")]
    public float moveSpeed = 1f;
    public float moveDistance = 1.5f;

    [Header("Sound effect Settings")]
    public AudioClip hoverSound;
    public float soundVolume = 1.0f;

}

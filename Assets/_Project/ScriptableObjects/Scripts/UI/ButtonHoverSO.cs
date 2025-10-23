using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "ButtonHoverEffect", menuName = "ScriptableObjects/UI/ButtonHoverEffect", order = 1)]
public class ButtonHoverSO : ScriptableObject
{
    [Header("Scale Settings")]
    public float scaleUpFactor = 1.2f;    
    public float scaleSpeed = 0.3f;
    public int punchVibrato = 10;
    public float punchElasticity = 1f;
    public Ease scaleEaseType = Ease.InElastic;
   

    [Header("Move Settings")]
    public float moveSpeed = 1f;
    public float moveDistance = 1.5f;
    public Ease moveEaseType = Ease.OutBack;

    [Header("Sound effect Settings")]
    public AudioClip hoverSound;
    public float soundVolume = 1.0f;

}

using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ButtonHoverSO hoverSettings;

    private Vector3 originalScale;
    private Tween currentTween;
    private AudioSource audioSource;

    private void Awake()
    {
        originalScale = transform.localScale;

        // Setup AudioSource se c'è un suono nelle impostazioni
        if (hoverSettings != null && hoverSettings.hoverSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.clip = hoverSettings.hoverSound;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Cancella il tween precedente per evitare conflitti
        currentTween?.Kill();

        currentTween = transform.DOScale(originalScale * hoverSettings.scaleUpFactor,
                                         hoverSettings.scaleSpeed)
                                .SetEase(hoverSettings.scaleEaseType)
                                .SetUpdate(true);

        // Riproduci suono hover
        if (audioSource != null && hoverSettings.hoverSound != null)
        {
            audioSource.volume = hoverSettings.soundVolume;
            audioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        currentTween?.Kill();

        currentTween = transform.DOScale(originalScale,
                                         hoverSettings.scaleSpeed)
                                .SetEase(hoverSettings.scaleEaseType)
                                .SetUpdate(true);
    }

    private void OnDisable()
    {
        currentTween?.Kill();
        transform.localScale = originalScale;
    }
}
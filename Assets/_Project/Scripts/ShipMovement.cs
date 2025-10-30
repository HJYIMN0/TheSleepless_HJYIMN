using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShipMovement : MonoBehaviour
{
    [Header("Ship")]
    [SerializeField] private GameObject _ship;
    [SerializeField] private float _shipRotationDuration = 1f;
    [SerializeField] private float _shipMaxRotation = 9f;
    [SerializeField] private LoopType _shipRotationLoopType = LoopType.Yoyo;
    [SerializeField] private Ease _shipRotationEase = Ease.InOutSine;

    [Header("WaterTrail")]
    [SerializeField] private GameObject _waterTrail;
    [SerializeField] private float _waterTrailMaxStretch = 3f;
    [SerializeField] private float _waterTrailStretchDuration = 5f;
    [SerializeField] private LoopType _waterTrailLoopType = LoopType.Yoyo;
    [SerializeField] private Ease _waterTrailEase = Ease.InOutSine;

    private Tween _rotationTween;
    private Tween _stretchTween;


    private void Start()
    {
        MoveShip();
        StretchWaterTrail();
    }

    public void MoveShip()
    {
        if (_rotationTween != null && _rotationTween.IsActive())
            return; // evita duplicati

        _ship.transform.localRotation = Quaternion.Euler(-_shipMaxRotation, 0f, 0f);

        _rotationTween = _ship.transform
            .DOLocalRotate(new Vector3(_shipMaxRotation, 0f, 0f), _shipRotationDuration, RotateMode.Fast)
            .SetEase(_shipRotationEase)
            .SetLoops(-1, _shipRotationLoopType);
    }

    public void StretchWaterTrail()
    {
        if (_stretchTween != null && _stretchTween.IsActive())
            return; // evita duplicati

        // Salva la scala iniziale
        Vector3 originalScale = _waterTrail.transform.localScale;

        // Crea una nuova scala con l'asse Z allungato
        Vector3 stretchedScale = new Vector3(
            originalScale.x,
            originalScale.y,
            _waterTrailMaxStretch

        );

        _stretchTween = _waterTrail.transform
            .DOScale(stretchedScale, _waterTrailStretchDuration)
            .SetEase(_waterTrailEase)
            .SetLoops(-1, _waterTrailLoopType);
    }


}

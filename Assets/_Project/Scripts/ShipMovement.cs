using System.Collections;
using UnityEngine;
using DG.Tweening;

public class ShipMovement : MonoBehaviour
{
    [SerializeField] private GameObject _ship;
    [SerializeField] private float _duration = 1f;
    [SerializeField] private float _maxRotation = 9f;
    [SerializeField] private LoopType _loopType = LoopType.Yoyo;
    [SerializeField] private Ease _ease = Ease.InOutSine;

    private Tween _rotationTween;


    private void Start()
    {
        MoveShip();
    }

    public void MoveShip()
    {
        if (_rotationTween != null && _rotationTween.IsActive())
            return; // evita duplicati

        _ship.transform.localRotation = Quaternion.Euler(-_maxRotation, 0f, 0f);

        _rotationTween = _ship.transform
            .DOLocalRotate(new Vector3(_maxRotation, 0f, 0f), _duration, RotateMode.Fast)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

}

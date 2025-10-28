using UnityEngine;
using UnityEngine.EventSystems;
public class RawImageClickRaycaster : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField] private CameraController _camControl;
    [SerializeField] private PlayerController _playerControl;
    private Camera _renderCamera;
    private void Start() 
    {
        _camControl.onCameraChanged += Initialize;
        if (_playerControl == null)
        {
            _playerControl = GameObject.FindGameObjectWithTag("Player").GetComponentInParent<PlayerController>();
            if (_playerControl == null) 
            {
                Debug.LogError("Missing playerControl Reference!");
            }
        }
    }
    public void Initialize(Camera cam)
    {
        _renderCamera = cam; Debug.Log($"Raw image RenderRaycast Camera = {_renderCamera}");
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (_renderCamera == null || _playerControl == null) return;

        RectTransform rt = GetComponent<RectTransform>();
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle
            (rt, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            Vector2 normalized = Rect.PointToNormalized(rt.rect, localPoint);
            Vector2 pixelCoord = new Vector2(normalized.x * _renderCamera.pixelWidth,
                                             normalized.y * _renderCamera.pixelHeight);
            Ray ray = _renderCamera.ScreenPointToRay(pixelCoord);
            
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                _playerControl.MoveToPoint(hit.point);
            }
        }
    }
}

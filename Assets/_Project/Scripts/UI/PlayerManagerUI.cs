using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using static PlayerTemperatureController;

public class PlayerManagerUI : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private Image energy;
    [SerializeField] private Image hunger;
    [SerializeField] private Image hygiene;
    [SerializeField] private Image integrity;

    [Header("Temperature")]
    [SerializeField] private Image thermometer;
    [SerializeField] private TextMeshProUGUI temperatureText;
    [SerializeField] private Color coldColor;
    [SerializeField] private Color hotColor;
    [SerializeField] private Color comfortableColor;

    [Header("Direction")]
    [SerializeField] private Image arrow;
    [SerializeField] private ButtonHoverSO arrowSO;

    [Header("Menu")]
    [SerializeField] private GameObject menuCanva;
    [SerializeField] private ButtonHoverSO menuSO;


    private Image[] _stats;
    private CanvasGroup _menuCanvasGroup;
    private Tween _tweenAnim;
    

    private GameManager _gm;
    private PlayerTemperatureController _temperatureController;

    private void Awake()
    {
        _stats = new Image[] { energy, hunger, hygiene, integrity };
        foreach (Image stat in _stats)
        {
            if (stat != null)
            {
                stat.type = Image.Type.Filled;
                stat.fillMethod = Image.FillMethod.Horizontal;
                stat.fillOrigin = (int)Image.OriginHorizontal.Left;
                stat.fillClockwise = false;
                stat.fillAmount = 1f;
            }
        }
    }
    void Start()
    {
        _gm = GameManager.Instance;
        _gm.OnValueChanged += UpdateUI;

        _temperatureController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTemperatureController>();

        //StartCoroutine(TestCoroutine());
    }

    public IEnumerator TestCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            foreach (Image stat in _stats)
            {
                if (stat != null)
                {
                    Debug.Log("Randomizing stat fill amount");
                    UpdateUI((StatisticType)Random.Range(0, 6), Random.Range(0, 101), 100);
                }               
            }
        }
    }

    private void UpdateUI(StatisticType type, int value, int maxValue)
    {
        switch (type)
        {
            case StatisticType.Energy:
                EvaluateValueChange(energy, value, maxValue);
                break;
            case StatisticType.Hunger:
                EvaluateValueChange(hunger, value, maxValue);
                break;
            case StatisticType.Hygiene:
                EvaluateValueChange(hygiene, value, maxValue);
                break;
            case StatisticType.Integrity:
                EvaluateValueChange(integrity, value, maxValue);
                break;
            case StatisticType.Climate:
                EvaluateValueTollerance(type, thermometer, value, maxValue);
                string temperature = value.ToString() + "°C";
                temperatureText.text = temperature;
                break;
            case StatisticType.Direction:
                EvaluateValueTollerance(type, arrow, value, maxValue);
                break;
        }
    }

    public void EvaluateValueChange(Image Ui, int value, int maxValue)
    {
        if (Ui != null)
        {
            Ui.fillAmount = Mathf.Clamp01((float)value / maxValue);
        }
    }
    public bool IsValueOutOfRange(int value, int desiredValue)
    {
        return Mathf.Abs(desiredValue - value) > _gm.ValueTolerance;
    }
    public void EvaluateValueTollerance(StatisticType type ,Image Ui, int value, int desiredValue)
    {
        if (IsValueOutOfRange(value, desiredValue))
        {
            switch (type)
            {
                case StatisticType.Climate:


                    if (value < desiredValue && _temperatureController.PlayerTemperatureState != TemperatureState.Cold)
                    //Only if not already cold
                    {
                        _temperatureController.SetTemperatureState(TemperatureState.Cold);
                        Ui.color = coldColor;
                    }
                    else if (value > desiredValue && _temperatureController.PlayerTemperatureState != TemperatureState.Hot)
                    //Only if not already hot
                    {
                        _temperatureController.SetTemperatureState(TemperatureState.Hot);
                        Ui.color = hotColor;
                    }
                        break;
                case StatisticType.Direction:
                float delta = value - desiredValue;
                float rotation = 0f;
                    if (Mathf.Abs(delta) > _gm.ValueTolerance)
                    {
                    // Clamp rotation -180 e 180
                    //Only if the difference is bigger then tolerance   
                    rotation = Mathf.Clamp(delta, -180f, 180f);
                    }

                Ui.rectTransform.rotation = Quaternion.Euler(0f, 0f, rotation);
                break;
            }
        }
    }

    public void ShowMenu()
    {
        if (_tweenAnim != null && _tweenAnim.IsActive() && _tweenAnim.IsPlaying())
            return;

        if (_menuCanvasGroup == null)
                _menuCanvasGroup = menuCanva.GetComponent<CanvasGroup>();

            _menuCanvasGroup.interactable = !_menuCanvasGroup.interactable;
            Vector3 direction = _menuCanvasGroup.interactable ? Vector3.right : Vector3.left;


        _tweenAnim?.Kill();
        _tweenAnim = menuCanva.transform.DOMove(
            menuCanva.transform.position + direction * menuSO.moveDistance,
            menuSO.moveSpeed)
            .SetEase(menuSO.easeType);
    }
}

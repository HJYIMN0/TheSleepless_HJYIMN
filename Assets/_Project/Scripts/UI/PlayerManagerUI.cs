using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEditor;
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
    [SerializeField] private Button compass;
    [SerializeField] private ButtonHoverSO compassSO;
    [SerializeField] private ButtonHoverSO arrowSO;

    [Header("Menu")]
    [SerializeField] private GameObject menuCanva;
    [SerializeField] private ButtonHoverSO menuSO;


    private Image[] _stats;
    private CanvasGroup _menuCanvasGroup;    
    private Tween _tweenAnim;

    private bool isMenuVisible = false;
    private bool isCompassVisible = false;

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

        

        _menuCanvasGroup = menuCanva.GetComponent<CanvasGroup>();
        _menuCanvasGroup.interactable = false;
        _menuCanvasGroup.alpha = 0f;
        _menuCanvasGroup.blocksRaycasts = false;
        isMenuVisible = false;

        _temperatureController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTemperatureController>();
        if (_temperatureController == null)
        {
            Debug.LogError("PlayerTemperatureController not found on Player!");
        }



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
    public void EvaluateValueTollerance(StatisticType type, Image Ui, int value, int desiredValue)
    {
        switch (type)
        {
            case StatisticType.Climate:
                string temperature = value.ToString() + "°C";
                temperatureText.text = temperature;

                if (IsValueOutOfRange(value, desiredValue))
                {
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
    public void CallMenu()
    {
        if (_tweenAnim != null && _tweenAnim.IsActive() && _tweenAnim.IsPlaying())
            return;

        if (_menuCanvasGroup == null)
            _menuCanvasGroup = menuCanva.GetComponent<CanvasGroup>();

        if (isMenuVisible)
        {
            StartCoroutine(HideMenu());
        }
        else
        {
            StartCoroutine(ShowMenu());
        }
        isMenuVisible = !isMenuVisible;
    }

    private IEnumerator ShowMenu()
    {
        if (_tweenAnim != null && _tweenAnim.IsActive() && _tweenAnim.IsPlaying())
            yield break;

        if (isCompassVisible)
        {
            CallCompass();
            yield return new WaitUntil(() => !isCompassVisible);
        }

        _menuCanvasGroup.interactable = true;
        _menuCanvasGroup.alpha = 1f;

        Vector3 direction = Vector3.right;

        _menuCanvasGroup.blocksRaycasts = true;

        _tweenAnim?.Kill();
        menuCanva.transform.localScale = Vector3.one;
        menuCanva.transform.DOPunchScale(
            Vector3.one * menuSO.scaleUpFactor,
            menuSO.scaleSpeed,
            menuSO.punchVibrato,
            menuSO.punchElasticity
        );

        _tweenAnim = menuCanva.transform.DOMove(
            menuCanva.transform.position + direction * menuSO.moveDistance,
            menuSO.moveSpeed)
            .SetEase(menuSO.moveEaseType);


    }

    private IEnumerator HideMenu()
    {
        _menuCanvasGroup.interactable = false;

        Vector3 direction = Vector3.left;
        _tweenAnim?.Kill();
        _tweenAnim = menuCanva.transform.DOMove(
            menuCanva.transform.position + direction * menuSO.moveDistance,
            menuSO.moveSpeed)
            .SetEase(menuSO.moveEaseType);

        menuCanva.transform.localScale = Vector3.one;
        menuCanva.transform.DOPunchScale(
            Vector3.one * menuSO.scaleUpFactor,
            menuSO.scaleSpeed,
            menuSO.punchVibrato,
            menuSO.punchElasticity
        );

        yield return _tweenAnim.WaitForCompletion();

        _menuCanvasGroup.blocksRaycasts = false;
        _menuCanvasGroup.alpha = 0f;
    }

    public void CallCompass() => StartCoroutine(ShowCompass());

    public IEnumerator ShowCompass()
    {
        if (_tweenAnim != null && _tweenAnim.IsActive() && _tweenAnim.IsPlaying())
            yield break;

        if (isMenuVisible)
        {
            CallMenu();
            yield return new WaitUntil(() => !isMenuVisible);
        }

        if (isCompassVisible)
        {
            compass.transform.DOMove(
                compass.transform.position + Vector3.down * compassSO.moveDistance,
                compassSO.moveSpeed)
                .SetEase(compassSO.moveEaseType);

            compass.transform.DOScale(
                Vector3.one,
                compassSO.scaleSpeed)
                .SetEase(compassSO.scaleEaseType);

            ButtonHoverComponent buttonHover = compass.GetComponent<ButtonHoverComponent>();
            if (buttonHover != null)
                buttonHover.enabled = true;
        }
        else
        {

            compass.transform.DOMove(
                compass.transform.position + Vector3.up * compassSO.moveDistance,
                compassSO.moveSpeed)
                .SetEase(compassSO.moveEaseType);

            compass.transform.DOScale(
                Vector3.one * compassSO.scaleUpFactor,
                compassSO.scaleSpeed)
                .SetEase(compassSO.scaleEaseType)
                .OnComplete(() =>
                {
                    compass.transform.DOPunchScale(
                        Vector3.one * compassSO.scaleUpFactor, 
                        compassSO.scaleSpeed,
                        compassSO.punchVibrato,
                        compassSO.punchElasticity
                    );
                });

            // Disable ButtonHoverComponent to avoid scale conflict
            ButtonHoverComponent buttonHover = compass.GetComponent<ButtonHoverComponent>();
            if (buttonHover != null)
                buttonHover.enabled = false;
        }

        isCompassVisible = !isCompassVisible;
    }
}


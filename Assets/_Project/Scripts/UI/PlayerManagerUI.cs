using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class PlayerManagerUI : MonoBehaviour
{
    [SerializeField] private Image energy;
    [SerializeField] private Image hunger;
    [SerializeField] private Image hygiene;
    [SerializeField] private Image integrity;

    private Image[] stats;


    private GameManager _gm;

    private void Awake()
    {
        stats = new Image[] { energy, hunger, hygiene, integrity };
        foreach (Image stat in stats)
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

        StartCoroutine(TestCoroutine());
    }

    public IEnumerator TestCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            foreach (Image stat in stats)
            {
                if (stat != null)
                {
                    Debug.Log("Randomizing stat fill amount");
                    UpdateUI((StatisticType)Random.Range(0, 4), Random.Range(0, 101), 100);
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
        }
    }

    public void EvaluateValueChange(Image Ui, int value, int maxValue)
    {
        if (Ui != null)
        {
            Ui.fillAmount = Mathf.Clamp01((float)value / maxValue);
        }
    }
}

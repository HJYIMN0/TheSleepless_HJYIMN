using System.Collections;
using UnityEngine;

public class PlayerTemperatureController : MonoBehaviour
{

    [SerializeField] private int temperatureDecrease = 5;
    [SerializeField] private int temperatureIncrease = 5;
    [SerializeField] private float timerInterval = 5f;

    [Header("player Reference)")]
    [SerializeField] private PlayerController playerController;

    private GameManager _gm;
    

    private TemperatureState playerTemperatureState = TemperatureState.Comfortable;
    public TemperatureState PlayerTemperatureState => playerTemperatureState;

    private void Start()
    {
        _gm = GameManager.Instance;
        StartCoroutine(TemperatureRoutine());

        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
            if (playerController == null)
            {
                Debug.LogWarning("Missing player controller reference!");
                return;
            }
        }
        playerController.OnPlayerBoatLocationChange += SetTemperatureBoatLocation;
    }

    public IEnumerator TemperatureRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timerInterval);
            switch(playerTemperatureState)
            { 
                case TemperatureState.Cold:
                    _gm.IncreaseClimate(-temperatureDecrease);
                    break;

                case TemperatureState.Hot:
                    _gm.IncreaseClimate(temperatureIncrease);
                    break;

                case TemperatureState.Comfortable:
                    _gm.SetClimateOnDesiredValue(_gm.DesiredClimate);
                    break;
            }
        }
    }

    public void SetTemperatureState(TemperatureState newState)
    {
        if (playerTemperatureState != newState)
        {
            StopAllCoroutines();
            playerTemperatureState = newState;
            StartCoroutine(TemperatureRoutine());            
        }
    }

    public void SetTemperatureBoatLocation(BoatLocations locations)
    {
        if (locations != BoatLocations.Deck)
        {
            SetTemperatureState(TemperatureState.Cold);
        }
        else 
        {
            SetTemperatureState(TemperatureState.Comfortable);
        }
    }
}

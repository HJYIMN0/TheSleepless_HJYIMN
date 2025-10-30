using DG.Tweening;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private void Start()
    {
        PlayerActionUi[] playerActionUis = FindObjectsByType<PlayerActionUi>(FindObjectsSortMode.None); // true include anche gli oggetti disattivati
        Debug.Log($"Trovati {playerActionUis.Length} oggetti con PlayerActionUi");

        foreach (var ui in playerActionUis)
        {
            Debug.Log($"Oggetto: {ui.gameObject.name}");
        }
    }


}

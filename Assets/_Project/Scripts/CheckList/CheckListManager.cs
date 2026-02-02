using Ink.Runtime;
using Ink.UnityIntegration;
using System.Linq;
using TMPro;
using UnityEngine;

public class CheckListManager : MonoBehaviour
{
    [SerializeField] private TextAsset checkListJsonFile;
    [SerializeField] private TextMeshProUGUI checkListText;
    
    public bool CanGoToBed { get; private set; } = false;

    private void Start()
    {
        CompileList();
    }

    public void CompileList()
    {
        Story story = new Story(checkListJsonFile.text);
        if (story != null && story.variablesState.Contains("Day"))
        {
            story.variablesState["Day"] = GameManager.Instance.Day;
            PopulateCheckList(story);
        }
    }

    private void PopulateCheckList(Story story)
    {
        checkListText.text = "";

        // 1. Reset non strettamente necessario se usi ChoosePathString, ma utile per pulizia
        story.ResetState();

        // 2. Impostazione variabile (utile se i nodi usano la variabile internamente)
        story.variablesState["Day"] = GameManager.Instance.Day;

        // 3. PASSAGGIO CRUCIALE: Salto al Nodo
        // Con la struttura attuale del file Ink, devi entrare esplicitamente nel Knot.
        // Nota l'underscore "_" che corrisponde alla sintassi del tuo file Ink (Day_1, Day_2)
        string pathName = $"Day_{GameManager.Instance.Day}";

        try
        {
            story.ChoosePathString(pathName);
        }
        catch
        {
            // Se il giorno non ha un nodo corrispondente (es. Day_99), usciamo senza errori
            Debug.LogWarning($"CheckListManager: Il nodo '{pathName}' non esiste nel file Ink.");
            return;
        }

        // 4. Lettura
        while (story.canContinue)
        {
            string line = story.Continue().Trim();

            // Filtro per evitare righe vuote
            if (!string.IsNullOrEmpty(line))
            {
                checkListText.text += "• " + line + "\n";
            }
        }
    }
}
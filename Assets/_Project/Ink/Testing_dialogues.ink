// Dichiarazione variabili
VAR DAY = 0
VAR PARANOIA = 0

// Punto di ingresso: Controllo priorità
{
    - PARANOIA >= 50:
        -> paranoia_overdrive
    - else:
        -> dialogo_giornaliero
}

=== paranoia_overdrive ===
// Questo nodo sovrascrive ogni altra logica se PARANOIA è >= 50
Ti senti osservato. Ogni rumore è un passo, ogni silenzio è un agguato. Non importa che giorno sia, importa solo chi è dietro la porta.
-> END

=== dialogo_giornaliero ===
// Switch basato sulla variabile DAY (da 0 a 9)
{ DAY:
    - 0: Sei appena arrivato. L'aria è ancora respirabile.
    - 1: Il primo giorno di lavoro è passato. Niente di strano.
    - 2: Hai trovato un biglietto sotto la scrivania, ma è illeggibile.
    - 3: La luce del corridoio continua a sfarfallare.
    - 4: Qualcuno ha spostato i tuoi documenti di nuovo.
    - 5: Metà settimana. La stanchezza inizia a farsi sentire.
    - 6: Hai sentito una voce familiare in mensa, ma eri solo.
    - 7: Il caffè ha un retrogusto metallico stamattina.
    - 8: Le finestre non si aprono più. L'aria è pesante.
    - 9: Domani te ne andrai. Se ci arrivi.
    - else: Il tempo sembra essersi fermato.
}
-> END

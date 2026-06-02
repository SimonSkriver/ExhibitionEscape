VAR visitNum = 1
VAR isWelcomeMessageCompleted = false
VAR hasTreasure = false

===Welcome===
{isWelcomeMessageCompleted:
    -> Ship
- else:
    OHØJ kammerat!
    Dette er min ø, som jeg så meget har overtaget helt selv. 
    Min besætning?
    Ik' tænk på dem. De er ubrugelige.
    Se på dem. De er bare skind og ben...
    ...uden skind...
    ...
    Du er mit besøgsnummer {visitNum}
    hvilket betyder at jeg har noget MEGET værdifuldt til dig.
    MIN SKAT!
    Hvis du kan finde den, vil jeg lade dig sejle min båd ;D
    ER DU KLAR?
    ~ isWelcomeMessageCompleted = true
    *[JA]
        LET'S GOOOO!!!
        -> END
    *[nej]
        oh...
        ...
        Hvis du bliver sulten, er der kød inde i junglen
        -> END
}


===Ship===
Vil du forlade mig uden min skat?
*[Ja]
    Nå, men det tillader jeg ikke ;P
*[Nej]
    Jaer, du vil ikke gå glip af den ;D
--> END



===SailAway===
ARRGH du fandt MIN SKAT!
Jeg vidste jeg kunne stole på dig
YO HO HO den passer stadig perfekt
Tusind tak besøgsnummer {visitNum}
Som lovet kan du få min båd
-> END



===THROW_CANONBALL===
// End of cutscene
TID TIL AT FLYVE!
// The captain is throwing a canonball
-> END
VAR visitNum = 512
VAR isWelcomeMessageCompleted = false
VAR hasTreasure = false

===Welcome===
{hasTreasure:
    -> SailAway
- else:
    OHØJ kammerat {visitNum}
    Vil du gerne ud på eventyr i denne mega seje båd?
    Den er næsten klar til brug
    Det der mangler er også det VIGTIGSTE for at kunne sejle jorden rundt.
    Du kan finde den et eller andet sted her på øen.
    Hvis du finder den, er du mere end klar til...
    DIT LIVS EVENTYR!!!
    -> END
}



===SailAway===
ARRGH du fandt HATTEN!
Gør klar til dit livs eventyr!
FOR NU KAN DU SEJLE AFSTED!!!
-> END



===THROW_CANONBALL===
// End of cutscene
TID TIL AT FLYVE!
// The captain is throwing a canonball
-> END
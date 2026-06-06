VAR isWelcomeMessageCompleted = false
VAR hasTreasure = false
VAR hasMadeTheEndChoice = false

EXTERNAL StartBoatCutscene()
EXTERNAL GiveCapHat()


===Captain===
{hasTreasure:
    {hasMadeTheEndChoice:
        Nå, fick du andra tankar?
        *[Ja, låt mig komma undan!]
            ~ GiveCapHat()
            Tack så mycket, ha en trevlig resa!
            ~ StartBoatCutscene()
            -> END
        
    - else:
        Du har hittat min förlorade skatt!
        Om jag kan få tillbaka den...
        så lånar jag dig min båt så att du kan komma bort från den här ön.
        
        *[ Ja, självklart!]
            ~ GiveCapHat()
            Tusen tack, ha en trevlig resa!
            ~ hasMadeTheEndChoice = true
            ~ StartBoatCutscene()
            -> END
            
        *[Nej, jag behåller den för mig själv.]
            Nå, vad ska jag göra åt det...?
            Om du inte ger mig skatten,
            kommer du att vara fångad här för alltid, precis som vi andra.
            -> NO
    }
    
- else:
    {isWelcomeMessageCompleted:
        Jag har ingen karta...
        *[Okej]
            -> END
        *[Kan du repetera uppdraget?]
            Ja, självklart!
            -> TreasureQuest
        
    - else:
        Ohoj!
        -> WelcomeMessage
    }
}



===WelcomeMessage===
Jag ser att du också är strandsatt på den här ön liksom många andra före dig.
Du vill inte vara instängd här för alltid som vi andra!
Som tur är har jag ett mäktigt skepp som du kan använda för att fly.
Men ingenting är gratis...
~ isWelcomeMessageCompleted = true
-> TreasureQuest

===TreasureQuest===
Om du kan hitta min förlorade skatt..
låter jag dig segla iväg i mitt skepp!
Men var försiktig...
Du kommer att upptäcka att vägen till skatten är full av hinder.
Hitta min skatt så att du kan fly från ön!
-> END



===NO===
*[Okej, varsågod]
    ~ GiveCapHat()
    Tusen tack, ha en trevlig resa!
    ~ hasMadeTheEndChoice = true
    ~ StartBoatCutscene()
    -> END
    
*[Nej, jag vill behålla den.]
    Okej, det är din förlust. Då måste du vara strandsatt här för alltid.
    ~ hasMadeTheEndChoice = true
    -> END



===THROW_CANONBALL===
// End of cutscene
DAGS ATT FLYGA!
// The captain is throwing a canonball
-> END
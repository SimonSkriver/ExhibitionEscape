VAR isWelcomeMessageCompleted = false
VAR hasTreasure = false
VAR hasMadeTheEndChoice = false

EXTERNAL StartBoatCutscene()
EXTERNAL GiveCapHat()


===Captain===
{hasTreasure:
    {hasMadeTheEndChoice:
        Nåå, kom du bedre tanker?
        *[Ja, lad mig slippe væk!]
            ~ GiveCapHat()
            Tusind tak, god sejltur!
            ~ StartBoatCutscene()
            -> END
        
    - else:
        Du har fundet min forsvundne skat!
        Hvis jeg må få den tilbage...
        vil jeg lade dig låne min båd, så du kan slippe væk fra denne ø.
        
        *[Ja selvfølgelig!]
            ~ GiveCapHat()
            Tusind tak, god sejltur!
            ~ hasMadeTheEndChoice = true
            ~ StartBoatCutscene()
            -> END
            
        *[Nej, den beholder jeg selv]
            Nå... Det kan jeg jo ikke gøre så meget ved...
            Men tænk dig om, hvis du ikke giver mig skatten,
            vil du være fanget her til evig tid ligesom os andre.
            -> NO
    }
    
- else:
    {isWelcomeMessageCompleted:
        Jeg har ikke et kort...
        *[Okay]
            -> END
        *[Vil du gentage opgaven?]
            Ja, selvfølgelig!
            -> TreasureQuest
        
    - else:
        Ohøj!
        -> WelcomeMessage
    }
}



===WelcomeMessage===
Jeg kan se, at du også er strandet på denne ø som mange andre før dig.
Du må endelig ikke være fanget på øen i al evighed ligesom os andre!
Heldigvis har jeg et mægtigt skib, du kan bruge til at slippe væk.
Men intet er gratis...
~ isWelcomeMessageCompleted = true
-> TreasureQuest

===TreasureQuest===
Hvis du kan finde min forsvundne skat..
vil jeg lade dig sejle afsted med mit skib!
Men tag dig i agt...
Du vil snart opdage, at vejen til skatten ikke er uden forhindringer.
Find min skat, så du kan slippe væk fra øen!
-> END



===NO===
*[Okay, du må gerne få den]
    ~ GiveCapHat()
    Tusind tak, god sejltur!
    ~ hasMadeTheEndChoice = true
    ~ StartBoatCutscene()
    -> END
    
*[Nix, jeg beholder den]
    Okay, dit tab. Så må du være strandet her for altid.
    ~ hasMadeTheEndChoice = true
    -> END



===THROW_CANONBALL===
// End of cutscene
TID TIL AT FLYVE!
// The captain is throwing a canonball
-> END
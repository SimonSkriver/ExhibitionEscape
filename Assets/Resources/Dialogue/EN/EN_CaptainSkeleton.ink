VAR isWelcomeMessageCompleted = false
VAR hasTreasure = false
VAR hasMadeTheEndChoice = false

EXTERNAL StartBoatCutscene()
EXTERNAL GiveCapHat()


===Captain===
{hasTreasure:
    {hasMadeTheEndChoice:
        Oh, did you get second thoughts?
        *[Yes, let me off the island, please.]
            ~ GiveCapHat()
            Thank you very much, fare well!
            ~ StartBoatCutscene()
            -> END
        
    - else:
        It seems you have found my long lost treasure!
        If you will let me have it back...
        I will let you use my boat to escape from this island.
        
        *[Of course!]
            ~ GiveCapHat()
            Thank you very much, fare well!
            ~ hasMadeTheEndChoice = true
            ~ StartBoatCutscene()
            -> END
            
        *[No, I want to keep it for myself.]
            Well, what am I going to do about it...?
            But if you don't give me the treasure..
            ..you'll be stranded on this island forever like the rest of us.
            -> NO
    }
    
- else:
    {isWelcomeMessageCompleted:
        I don't have a map...
        *[Okay]
            -> END
        *[Can you repeat the quest?]
            Yes, of course!
            -> TreasureQuest
        
    - else:
        Ahoy!
        -> WelcomeMessage
    }
}



===WelcomeMessage===
I see you have stranded on the island like many before you.
You don't want to be trapped here forever like the rest of us!
Lucky for you, I have a mighty ship, you can use to escape.
But nothing in this world is free...
~ isWelcomeMessageCompleted = true
-> TreasureQuest

===TreasureQuest===
If you can find my lost treasure on this island..
I will let you sail away with my ship!
But beware...
You will find that the road to the treasure is filled with obstacles.
Bring me my treasure, and you can escape the island!
-> END



===NO===
*[Alright, you can have it.]
    ~ GiveCapHat()
    Thank you very much, fare well!
    ~ hasMadeTheEndChoice = true
    ~ StartBoatCutscene()
    -> END
    
*[Nope, I want to keep it.]
    Alright, that's your loss. You will be trapped here forever!
    ~ hasMadeTheEndChoice = true
    -> END



===THROW_CANONBALL===
// End of cutscene
TIME TO SCUTTLE!
// The captain is throwing a canonball
-> END
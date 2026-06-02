VAR visitNum = 512
VAR isWelcomeMessageCompleted = false
VAR hasTreasure = false

===Welcome===
{isWelcomeMessageCompleted:
    -> SailAway
- else:
    AHOY camrade {visitNum}
    
    
    
    This is my island, that I totally captured by myself.
    My crew?
    Don't worry about them. They're useless.
    I mean look at them. They're just skin and bones...
    ...without skin...
    ...
    You are my visitor number {visitNum}
    and I have something VERY valuable for you
    MY TREASURE is somewhere on this island
    If you can find it, I will let you sail away in my boat ;D
    ARE YOU READY?
    ~ isWelcomeMessageCompleted = true
    *[YES]
        LET'S GOOOO!!!
        -> END
    *[no]
        oh...
        ...
        If you get hungry, there's meat in the jungle
        -> END
}



===SailAway===
ARRGH you found THE HAT!
Be ready for your greatest adventure!
BECAUSE YOU CAN NOW SAIL AWAY!!!
-> END



===THROW_CANONBALL===
// End of cutscene
TIME TO SCUTTLE!
// The captain is throwing a canonball
-> END
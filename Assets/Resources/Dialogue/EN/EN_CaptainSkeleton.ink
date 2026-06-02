VAR visitNum = 1
VAR isWelcomeMessageCompleted = false
VAR hasTreasure = false

===Welcome===
{isWelcomeMessageCompleted:
    -> Ship
- else:
    AHOY camrade!
    This is my island, that I totally captured by myself.
    My crew?
    Don't worry about them. They're useless.
    I mean look at them. They're just bare bones.
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



===Ship===
{hasTreasure:
    -> SailAway
- else:
    Planning to leave without my treasure?
    *[Yes]
        Well, I won't let you ;P
        -> END
    *[No]
        Yeah, you wouldn't miss out ;D
        -> END
}


===SailAway===
ARRGH you found MY TREASURE!
I knew I could trust you
YO HO HO it still fits perfectly
Thank you visitor {visitNum}
As promised you can have my boat
-> END



===THROW_CANONBALL===
// End of cutscene
TIME TO SCUTTLE!
// The captain is throwing a canonball
-> END
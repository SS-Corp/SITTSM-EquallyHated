# every changes! cool

## additions
* **elevator** Weight Limit (co-op)
    * "WEIGHT LIMIT REACHEd"
    * "FIGHT FOR IT"
* floor intro talk
* **character** 
    * crawl (runCrippledState)

## changes
* **GAME** LevelManager
    * ._friendlyFireOn = True **<ins>(the start of it all)</ins>**
* **GAME** PassiveManager (kinda ruins purpose)
    * fartsCanSplit = True
    * projectilesCanHome = True 
* **GAME** Experience (so you can focus on the fight)
    * keeps getting xp
    * recieve presents every xp filled
    * 1 door opened
* **GAME** PresentSpawner
    * spawns 1 present
    * spawns 1 more present if IsAScrounger();
* **GAME** MusicManager
    * title screen menu has 3 levels of the *Corporate Jingle*
        * low
        * mid
        * high
* **CHARACTER** 
    * burning = 10 seconds
    * chance to CrippleLimbs()
    * players can be sticky (EnemiesStickToWalls)
    * players can be clumsy if missed attack (ClumsyEnemies)
    * enemies can grab other enemies (devs make enemies grab others and release for fun!)
    * movement state things
    * teleport knocks other players
    * yapping makes other players sleep (your partner cant stand you [this goes in irl too])
* **CAMERA** 
    * shake is rigid 
    * shake is random (i would like it to be relative where shake is BUT it will take too long!)
    * shakes hard when PLAYER or BOSS dies
* **UI** 
    * .renderMode = RenderMode.WorldSpace
    * parent = CameraHolder
* **OBJECT**
    * farts explode on despawn
    * black hole affects other players
    * Traps
        * TrapElectrical Affects Players (Servers, Lights, etc)
        * TrapCamera Affects Enemies
* **Health Bar** 
    * overtime messages
        * "OVERTIME",
        * "WATCH OUT"
        * "CLOSE ONE"
        * "THAT HURTS YOU"
        * "LAST CHANCE"
        * "OKAYYY"
        * "NOT CREDIT TAKING"
        * "NICE DODGE DUDEEE"
        * "I SHOULD JUST PUT YOU DOWN!"
        * "KYS"
        * "STOP BEING RISKY!"

## comments
* WHAT IS *MakingOutMovementState*
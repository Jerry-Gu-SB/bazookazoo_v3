# bazookazoo_v3  
3rd write of bazookazoo  

Version 1: https://github.com/Jerry-Gu-SB/bazookazoo-PROTOTYPE  
Version 2: https://github.com/Jerry-Gu-SB/bazookazoo_v2  

### Artificial Rules
- Network Variables are for synced states (i.e. HP, ammo, etc.)
- Client Rpcs are for one-off events and not for persisting data. So things like explosions or shots or something.
- Player stats should be a separate class of network variables that gets sent over. And then you can modify them.

### Current problems:
1. Bazookas are too coupled to the player
2. Player Manager + Player Movement scripts are overloaded
3. Decouple scoring and HP modifications from the rockets
4. Rewrite death and rocket functionality in general
5. Separate the main menu manager into different components
6. Make the ticker a Global Network Ticker
7. Rewrite respawning to server authority and reduce indeterminism
8. No more scene transitions. Adds unnecessary complexity. Just have 1 god scene and call it a day. The game isn't complex enough to justify more.
9. Do I even need the Game State Manager? Scope down Game State Manager. It should be more like a normal maanger that handles:
    - Map loading
    - Game mode + Settings handling
    - Victory/defeat
    - Game countdown at the start of the game
    - killstreak announcements

    - Gamemode + settings + victory and defeat should be separated into a different script
    - Maybe something like:
      1. Round config script:  
         - Game mode
         - Settings
         - Victory/defeat conditions
         - Map loading
      2. Send round config as its own struct/class  
      3. Round manager  
         - Scoring
         - Game countdown
         - Win condition tracking
     
      - Maybe remove game states altogether?
        - assume clients spawn before game start
        - only have pre-game and in-game state
       
### Solutions:
1. Bazooka Prefab on its own. Swap out Prefabs
2. Split the following functionality all in their own scripts. Then have a Player Manager delegate and control the order in which events happen by calling these scripts.
     - health + death handling
     - respawning
     - scoreboard
     - Visuals (sprite flips, animation)
     - Input
     - Physics simulation
     - Player Network Movement
     - Player UI
3. Rockets: Should only hold the shooter network ID, call a method to apply force and knockback on an object WITHIN THAT OBJECT. The rocket should NEVER modify the object properties directly.
4. Death responsiblity should not be handled by the rockets at all either.
5. Main menus should have states and are a FSM. Connect --> Options -(start)--> Hide menus --(game over)--> Options
   - Options should have Map, Mode, and any other settings ALL ON ONE SCREEN instead of a new screen per setting.
6. Global Network Ticker: Anything using time that must be synced must use the global tick
7. Honestly, just hardcode these in the god scene, and attach them to the spawn maanger pre-serialized. Coding on-time is too unreliable. You can just serialize a list for this instead of grabbing points on runtime.
8. Just have 1 god scene. that's it. Then enable or disable map elements. Maybe do a quick loading screen? (probably not)

# Space Salvage

**Space Salvage is a 2-d, Unity-based game which has been SIGGD's project since January 2015.**

* [SIGGD Website](http://purduesiggd.github.io/)  
* [Trello Design Board](https://trello.com/b/7PS36bNg/spring-2015-space-salvage) 

The object of the game is to gather as many resources as you can, without losing suit integrity, oxygen, or health. Losing enough health will lead you to die. 

The player is spawned in a random, procedurally generated world which will change each time the player leaves the area.

Surrounding the player is derelict space ships, and the player must find their way through these ships and salvage what they can.

# What you will encounter

**Items able to be salvaged:**
- Coins
- Crates (medical supplies, food, cash)

Crates can be grabbed using your claw, and should be dragged back to your ship before you leave.

The player must also try to avoid several enemies which can disable them or tear the player apart.

**Enemies so far:**
- Turret (EMP and Damaging)
- Chasers 

There are also several environmental challenges such as 

- Lasers
- EMP Fields
- Air Jets
- Doors/Airlocks
- Slow field (goo or something)

![Screenshot](Art/SSScreenshot1.png?raw=true)

# Watching out for yourself:

The main 4 values you need to watch out for are *suit integrity*, *oxygen*, *health*, and *tube length*.

**Suit integrity:** 
- Suit integrity is what defends you from lasers, missiles, and heavy impacts.
- Can be upgraded for a cost by buying armor
- Losing too much suit integrity will let you leak oxygen, even if attached to your cable, and lets you take more damage to your health.

**Oxygen**
- You are spawned connected to a cable, and if that cable is damaged or released (pressing 'G' default), you will start losing oxygen.
- Must be replenished if low
- Too low oxygen will start decreasing your health and visibility
- Can be replenished by your cable, an oxygen box, or the ship.

**Health**
- Healed every time you leave and enter the area
- Can be damaged through armor, at a rate inverse to the amount of armor and suit integrity

**Tube length**
- If you let go of your cord, it will swing around and wait to be picked up.
- If your cord is damaged and severed, it will be sucked into the ship and will have a fee in order to be repaired again. 
- You can extend or retract your cables using shift and control, respectively. 

Extract our latest build in ~/executables today, and report all bugs to this document: https://docs.google.com/document/d/1i1WLIQKyGDf_0D_Ympt59YJ-68DcO0LPfyavdCLRTYU/edit?usp=sharing
# Automakiller
# Overview: 
	This game will be a top-down arena shooter with melee combat elements. As the player character you are equipped with your choice of a sword as a melee weapon or a gun as a projectile weapon. The goal will be to survive as long as possible in the limited arena while also destroying as many enemies (robots) as possible before getting killed yourself. The enemies will continue to spawn until you are defeated so there is very little rest in between encounters and you must always be on your toes. Enemies will die in one shot but are also strong enough to kill the player in one shot as well so you must be careful. They will chase you around the map from the multiple areas that they spawn from with the possibility of cutting you off if you are not too careful. The player will be able to dash on cooldown so must be used carefully as well. The map will be a confined arena with areas to get out of the line of sight of enemies. Again, you must be careful not to get backed into a corner or get overwhelmed from the number of enemies. There will also be special attacks that the player can use like a force push or some other ability. I am thinking that there will be different sword colors and each option will have a different special ability the player can use.

# Controls: 
	Mouse and Keyboard controls
  
-Movement: Keyboard ‘W’, ‘A’, ‘S’, ‘D’ keys to walk up, left, right, and down across the game map. 

-Dash: Keyboard ‘Space’ in combination with walking movement to get a movement boost in that direction. Will be on a cooldown timer once used in order to prevent spamming the dash action.

-Mouse cursor: Player character will be facing the direction 

-Attack: Left mouse click will perform a normal attack (either melee ROE or fire a shot in the direction the mouse cursor is pointing to)

-Reload: If the player is using a projectile weapon and any that weapon is also not fully loaded, pressing ‘R’ on the keyboard will reload the weapon’s ammo back to full

-Special Attack: Right mouse click will activate a special attack move in the direction the mouse cursor is pointing to. This ability will be on a cooldown once activated. 


# Art Assets:
-Player character: 2D pixel sprite will have walking, dashing, attacking, and death animation

-Enemies: 2D pixel sprite will have walking, attacking, and death animation

-HUD: Displays key information like ability cooldowns and number of enemies destroyed

-Arena map: Confined area for the battle with obstacles that the player can use to their advantage, granted that they don’t get backed into a corner


# Audio Assets:
-Background: Slow soundtrack that gets more intense with enemies closing in

-Player character: Walking, Dashing, weapon noises (melee and shooting)

-Enemies: Walking, weapon noises, death noises

-Game over: Death noise, Sad audio queue


# Concerns:
-AI pathing: I have a general idea of how I want to code enemies to chase and shoot at the player using a utility based agent similar to that of Pacman’s red ghost. I know that this will require quite a bit of testing to get working smoothly so I hope that any map geometry I make doesn’t cause too many issues for the AI.

-Melee mechanic for an arena shooter: I haven’t played or know too many arena shooters where the main weapon is a melee so I must account for how large the area of effect for the weapon is where it still feels balanced. That is why I wanted to include a projectile weapon to fall back on if it does not work but it would be cool to do both.

-Speed of player and enemies: I want the game to have moments where the combat felt fast paced but not so fast as it was hard to track what was happening as it can leave the game feeling unfair. I will have to tune this as I go for the best feel.


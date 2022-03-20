# Unity Fighting-Game (In progress)

This was the first game I ever worked on- I love fighting games and aliens so I had a lot of fun learning Unity while doing this. I ended up putting it aside because I wanted to focus on a game that I could finish in a reasonable amount of time while working full time. The idea for this was to make a very free-form fighting game with mechanics similar to Under Night: In-Birth, except with more air mobility and defensive options. 


## Game Overview
![Game Overview](https://media.giphy.com/media/IHRSaNCOnim1cp5eY2/giphy.gif)

Attacking and blocking

![Game Overview](https://media.giphy.com/media/w0Zu1OMDNl8LjjvHUZ/giphy.gif)

Gameplay with hit and hurtboxes visible

![Game Overview](https://media.giphy.com/media/3FwVQJ8Srudsk8jFWp/giphy.gif)

Newly added: platforms? and the camera moves now with the characters

## Attack Chaining and Blocking Mid, Low, High
![Attack Chaining](https://media.giphy.com/media/eNnVygjtRF5vV6KqCS/giphy.gif)

You can chain attacks in any order, but you can't use the same move twice (similar to Under Night's chaining system). Hold back or down-back to block. I'm still working on the block and hitstun animations, because currently there's no crouching animation for either and characters dont get pushed away when they block.

## Main Menu
![Main Menu](https://media.giphy.com/media/dZIe04gejQrGL2ReAb/giphy.gif)

## Bone Based Animations
![Animations](https://media.giphy.com/media/XLr7J1y5WkoejXCF7a/giphy.gif)


## Easily Customize the Frame Data for Attacks
![Animations](https://media.giphy.com/media/XRlhIb6RHaASuKBUMV/giphy.gif)

Ive made attacks an inheritable class that allows for every attack in the game to be customized easily with little code. Set various attributes like hitstun, blockstun, set the attack to mid, overhead, or low, etc.

## List of features not explicitly shown
- Custom input system to allow for motion inputs similar to other fighting games (quarter circle, double tap for dashes)
- Custom hitbox and hurtbox implementations to allow for easy customization and inheritance
- Blocking attacks
- Health bars
- 1 player or 2 player
- Movement options: Walk, dash, back dash, crouch, air dash, aerial drift

## Things that aren't done
- The biggest thing is just better animations and UI
- Still need to flesh out the offense system (offensive meter,balancing hitstun duration, special attacks)
- Add more defensive options (pushblock, defensive meter, burst, hitstun pushblock (maybe?)
- More characters and stages

## Tech Stack
- C#
- Unity
- Krita for drawing

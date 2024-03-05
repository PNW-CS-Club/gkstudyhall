# gkstudyhall
Gatekeeper Study Hall Edition

Recreating the simplified version of Gatekeeper found on https://gatekeeperonline.net/play

4 player board game 



- Project Details:
- Unity Version: 2022.3.19f1
- 2D Top-Down

# How to Contribute

View the issues available at https://github.com/PNW-CS-Club/gkstudyhall/issues
Assign Yourself to an Issue
- Add a title
- Add a description
- Assign yourself to the issue
- Add appropriate label
- Choose GatekeeperStudyHall-Feature-Board as the project
- Select a milestone if available
- Select the branch associated with the issue

View the feature board in Projects at https://github.com/orgs/PNW-CS-Club/projects/2
- Add an item to Planning or Ready
- Move an item from Planning to Ready, create an issue for the item, create a branch for the issue
- Move an item from Ready to In Progress, create an issue for the item, then follow the steps for assigning yourself to an issue

When finished with a branch/issue
- Create a pull request
When 2 people have approved the request, pull the changes to main. Ensure that there are no merge conflicts before pulling

# Game Loop
Start Menu -> Character Select -> Gameplay -> End Menu -> Start Menu
### Start Menu
* Start Game
* Close Game
### Character Select
* 1 Player Minimum
* Can add up to 3 more Players, any Players that are not added are filled with Bots
* Each Player chooses a Character, Bots will be randomly given a remaining Character
### Gameplay
* Perform Trait Roll
  - If possible, select gate or player to attack
* End Turn, switch to next player and restart Gameplay loop
* If a win condition is met, go to End Menu
### End Menu
* Display which player won the game
* Display some stats (Ex: damage to gates, damage to players)

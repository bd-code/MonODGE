# MonODGE.IO
A player input library for MonoGame.

**Dependencies**: MonoGame 3.8 or higher.

---

- Keyboard and GamePad-based input. 
  - *MonODGE.IO does not have mouse support by design.*
- Key and Button mapping. User defined commands map to specific Keyboard and/or GamePad input.

---

## Usage

The OdgeIO class works as a singleton with minimal setup required.

In Game1.Initialize() setup your keybindings.
```
// OdgeIOHelper comes with a number of helpful methods to make common
// keybindings easier. 

// Here's one that maps the Keyboard's WASD Keys to the following 
// string commands: { "_UP", "_LEFT", "_RIGHT", "_DOWN" }
OdgeIOHelper.MapWASD();

// Now add your own custom commands.
OdgeIO.KeyMap.Add("Menu", Keys.M);
OdgeIO.KeyMap.Add("Jump", Buttons.A);
OdgeIO.KeyMap.Add("Attack", Keys.NumPad3, Buttons.Y);
```

Update OdgeIO at the beginning of Game1.Update().
```
OdgeIO.Input.Update();
```

If you need to use a separate input manager you can manually update OdgeIO's device components with values from elsewhere.
```
OdgeIO.KB.Update(Keyboard.GetState());
OdgeIO.GP.Update(0, GamePad.GetState(0));
```

Now anywhere in your game's code you can poll for input.
```
// Again, OdgeIOHelper provides some useful methods here.
// If you used one of the OdgeIOHelper directional mapping 
// methods described above, you can do this: 
if (OdgeIOHelper.UP)
    player.MoveUp();

// If you're using multiple GamePads (multiplayer) pass 
// the player's GamePad index when polling commands.
if (OdgeIO.isCommandDown(intGamePadIndex, "Attack"))
    player.Attack();

// If your game is single player, 
// or if you're only accepting Keyboard input,
// you can omit the GamePad index:
if (OdgeIO.isCommandDown("Attack"))
    player.Attack();
```

You can also poll devices directly without using keybindings.
```
if (OdgeIO.KB.IsKeyPress(Keys.M))
    OpenMenu();

if (OdgeIO.GP.IsButtonHold(0, Buttons.B))
    isRunning = true;
```
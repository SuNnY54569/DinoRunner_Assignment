# Endless Runner - Game Developer Assignment

A 2D Endless Runner game developed in Unity as part of the Junior Game Developer technical assignment.

The player controls a dinosaur character that must jump over and crouch under obstacles while the game continuously increases in speed over time.

The main focus of this project is gameplay programming, code structure, responsive controls, game feel, and reliable game-state management rather than visual complexity.

---

## 1. Project Information

**Engine:** Unity 2022.3.62f3
**Language:** C#  
**Platform:** PC  
**Main Scene:** `Endless`

### How to Play

| Input | Action |
|---|---|
| Space / Up Arrow | Jump |
| Down Arrow | Crouch |
| Restart Button | Restart the game after Game Over |

The player remains in a fixed horizontal position while obstacles move from right to left.

---

# 2. Implementation Status

## Player Character & Controls

- [x] Player remains in a fixed horizontal position
- [x] Jump
- [x] Crouch
- [x] Jump only when grounded / within Coyote Time
- [x] Prevent double jumping
- [x] Variable jump height / Jump Cut
- [x] Player collider changes size when crouching
- [x] Crouching during the jump causes faster falling
- [x] Jumping while crouching is supported
- [x] Player returns to crouching after landing if crouch is still held
- [x] Coyote Time
- [x] Input Buffering
- [x] Screen Shake
- [x] Hit Stop
- [x] Particle
- [x] Forgiving Collider

## Obstacle System

- [x] Multiple obstacle types
- [x] Obstacles spawn from the right side of the screen
- [x] Obstacles move toward the player
- [x] Game speed increases over time
- [x] Maximum game speed
- [x] Spawn distance is calculated based on current game speed
- [x] Obstacles can be unlocked progressively using unlock time
- [x] Obstacles are removed after leaving the playable area

## Collision & Game Over

- [x] Player-obstacle collision detection
- [x] Game Over state
- [x] Game Over UI
- [x] Current score display
- [x] Session high score
- [x] Restart functionality
- [x] Game speed reset on restart
- [x] Player state reset on restart
- [x] Existing obstacles cleared on restart
- [x] Time.timeScale reset on restart

## Game Feel

- [x] Coyote Time
- [x] Input Buffering
- [x] Forgiving Collider
- [x] Screen Shake
- [x] Hit Stop
- [x] Running / Landing Particle Effects

## Frame Rate Independence

- [x] Time-based gameplay calculations
- [x] Physics-based player movement handled through Rigidbody2D
- [x] Time.deltaTime used for frame-rate-independent timers and movement

---

# 3. Design Assumptions

The assignment leaves some gameplay details open, so the following assumptions were made:

### Player Movement

The player does not move horizontally. Instead, obstacles move from right to left to create the illusion of forward movement.

This keeps the gameplay focused on timing-based obstacle avoidance and also makes the game speed easier to control centrally.

### Jumping

The player can only initiate a jump while grounded or within the configured Coyote Time window.

Jumping again while already airborne does not create another jump.

The jump uses a variable-height system. Releasing the jump button while the player is moving upward reduces the player's vertical velocity, allowing both short and long jumps.

### Crouching

Crouching is a held input rather than a toggle.

The player must continue holding the crouch button to remain crouched.

If the player jumps while crouching, the player temporarily returns to the standing collider and visual while airborne. If crouch is still held when the player lands, the player returns to the crouching state.

### Game Speed

Game speed starts at a predefined value and increases linearly over time until reaching a maximum speed.

A maximum speed is used to prevent the game from becoming unnecessarily difficult or physically unreliable at extreme speeds.

### Obstacle Spawning

Obstacle spacing is represented as a distance rather than a fixed amount of time.

The next spawn time is calculated using:

    spawnTime = spawnDistance / currentGameSpeed

This means that the approximate distance between obstacles remains consistent even as the game becomes faster.

### High Score

High Score is treated as a session-based value.

Restarting the game resets the current score but does not reset the session high score.

---

# 4. Player State Management

The player uses a simple state-based approach using `PlayerState`.

The main states are:

- `Grounded`
- `Crouching`
- `Jumping`
- `Falling`
- `Dead`

The state is determined from the player's grounded status, vertical Rigidbody2D velocity, and crouch input.

The state is used to control gameplay-related behavior such as:

- Player collider size
- Player visual
- Crouching
- Jumping
- Falling
- Player reset

A simple state-based approach was chosen because the player has a small number of mutually exclusive gameplay states. It keeps the logic easy to follow without introducing unnecessary complexity such as a full state-machine framework.

---

# 5. Rigidbody2D vs Manual Movement

I chose to use Unity's `Rigidbody2D` for the player rather than implementing gravity and vertical movement manually.

The main reasons are:

1. Unity's physics system provides consistent gravity and collision handling.
2. The player needs reliable interaction with the ground and obstacles.
3. Rigidbody2D makes vertical velocity easy to control for jumping, jump cutting, and fast falling.
4. It reduces the amount of custom physics code required for this relatively small game.

The player's jump is performed by setting the Rigidbody2D vertical velocity rather than directly changing the Transform position.

Example:

    rb.velocity = new Vector2(
        rb.velocity.x,
        jumpForce
    );

---

# 6. Frame Rate Independence

Frame-rate independence is handled by using `Time.deltaTime` for gameplay calculations that are based on elapsed time.

Examples include:

- Game speed progression
- Score / distance calculation
- Spawn timers
- Coyote Time
- Jump Buffer timers
- Obstacle movement
- Other time-based effects

For example, obstacle movement uses:

    transform.Translate(
        Vector2.left * (moveSpeed * Time.deltaTime)
    );

This ensures that the obstacle moves according to elapsed time rather than the number of frames rendered.

### Update()

`Update()` is used for:

- Reading player input
- Updating timers
- Updating player state
- Updating game speed
- Updating score
- Updating obstacle spawn timers
- Other frame-based gameplay logic

### FixedUpdate()

`FixedUpdate()` is used for the player's jump physics because the player uses `Rigidbody2D`.

The jump applies a change to the Rigidbody2D velocity, so it is handled through the physics update loop.

This separation allows input and timer logic to remain responsive while physics-related changes are synchronized with Unity's physics system.

---

# 7. Game Feel

I implemented several game-feel techniques to make the controls feel responsive, readable, and fair.

The main goal was to reduce frustration caused by small timing mistakes while keeping the core challenge of the game unchanged.

---

## Coyote Time

**Why I chose it**

The game is based heavily on timing jumps against approaching obstacles. Without Coyote Time, pressing the jump button just a fraction of a second after leaving the ground can result in an unexpected failure.

I chose to implement approximately 0.1 seconds of Coyote Time to make the jump timing more forgiving without giving the player a significant gameplay advantage.

**How it changes the game feel**

Coyote Time makes jumping feel more responsive and consistent. Players are less likely to feel that the game ignored their input when they were visually close to the edge of the platform.

It makes the game feel more forgiving while preserving the importance of timing.

---

## Input Buffering

**Why I chose it**

Endless Runner gameplay requires the player to react quickly to obstacles. When an obstacle approaches while the player is landing, it is possible for the player to press Jump slightly before the character actually touches the ground.

I chose to implement a short jump input buffer so that the game can remember this input and execute the jump immediately when the player becomes able to jump.

**How it changes the game feel**

Input Buffering makes the controls feel more responsive because players do not have to press the button on exactly the correct frame.

This is especially useful when obstacles appear in quick succession and helps the controls feel consistent rather than frame-perfect.

---

## Forgiving Collider

**Why I chose it**

The game uses visual sprites that are larger than the actual gameplay-critical areas of the character. I chose to make the collision area slightly smaller than the visual representation to avoid collisions that feel unfair, especially when the player visually appears to have narrowly avoided an obstacle.

**How it changes the game feel**

A forgiving collider makes near-miss situations feel more natural and fair.

The player can visually come very close to an obstacle without immediately losing, which makes the game feel less punishing while still requiring the player to correctly jump or crouch.

---

## Screen Shake

**Why I chose it**

Collision with an obstacle is the most important failure event in the game. I wanted the player to immediately understand that the collision had occurred without relying only on the Game Over UI.

I chose a short screen shake because it provides strong visual feedback without affecting the player's controls or gameplay rules.

**How it changes the game feel**

The screen shake gives the collision more physical impact.

Instead of the game simply stopping and displaying "Game Over", the player receives an immediate visual response that reinforces the feeling of being hit.

I kept the effect short and subtle so that it adds impact without becoming distracting.

---

## Hit Stop

**Why I chose it**

The collision that causes Game Over happens very quickly, so the player may not clearly perceive the exact moment of impact.

I chose to add a very short Hit Stop effect to emphasize this moment before transitioning to the Game Over screen.

**How it changes the game feel**

The brief pause creates a stronger sense of impact and makes the collision feel more significant.

Because the pause is intentionally very short, it does not interfere with normal gameplay and is only used as feedback for the failure event.

---

## Particle Effects

**Why I chose it**

The player remains in approximately the same horizontal position while the environment moves toward them. Because of this, visual feedback is important for communicating that the character is actively running rather than simply standing still.

I chose to use dust particles while running and when landing to reinforce the character's movement.

**How it changes the game feel**

The particles make the player's movement feel more dynamic and responsive.

The landing effect also provides additional feedback that the jump has finished and the player has returned to the ground.


---

# 8. Obstacle System

Obstacles are represented using `ObstacleData` ScriptableObjects.

Each obstacle data asset can define information such as:

- Obstacle prefab
- Unlock time

This allows obstacle configuration to be changed in the Inspector without modifying the spawner code.

The spawner maintains a list of available obstacle data and filters obstacles based on their unlock time.

For example:

    if (elapsedTime >= data.unlockTime)
    {
        availableObstacles.Add(data);
    }

This allows more difficult obstacle types to appear later in the run.

---

## Spawn Distance

Obstacle spacing is defined using a minimum and maximum spawn distance.

Instead of directly using these values as seconds, the spawner converts distance into time based on the current game speed:

    spawnTimer = spawnDistance / gameManager.CurrentSpeed;

This was chosen because using a fixed time interval would cause the physical distance between obstacles to become smaller as the game speed increased.

Using distance-based spawning keeps the approximate spacing more consistent as difficulty increases.

---

# 9. Game Speed

Game speed is managed centrally by `GameManager`.

The speed starts at `startSpeed` and increases over time:

    currentSpeed =
        startSpeed + elapsedTime * speedIncrease

The value is clamped using `Mathf.Min()` so it cannot exceed `maxSpeed`.

This provides predictable difficulty progression while preventing the game from becoming impossible or physically unreliable.

The current speed is shared with systems such as the obstacle spawner and obstacles themselves.

---

# 10. Score System

The score is based on the distance travelled by the game world.

Since the player remains stationary horizontally, the distance is simulated using the current game speed:

    distance += Time.deltaTime * gameManager.CurrentSpeed;

The current score is then calculated from the accumulated distance.

This makes the score directly represent how far the player has survived.

The `ScoreManager` maintains two values:

- `CurrentScore`
- `HighScore`

The current score is reset when restarting the game, while the high score remains for the current session.

---

# 11. Game Over & Restart

When the player collides with an obstacle:

    GameManager.GameOver()

is called.

The GameManager then:

1. Changes the game state to Game Over.
2. Displays the Game Over UI.
3. Stops gameplay using `Time.timeScale = 0`.

When Restart is pressed:

1. `Time.timeScale` is restored to `1`.
2. Game Over state is cleared.
3. Game speed is reset.
4. Elapsed game time is reset.
5. Current score is reset.
6. Player state and position are reset.
7. Existing obstacles are destroyed.
8. Obstacle spawner is reset.
9. Game Over UI is hidden.

The high score is intentionally not reset because it represents the highest score achieved during the current session.

---

# 12. Code Structure

The project is divided into small components with separate responsibilities.

### `GameManager`

Responsible for the overall game state.

Responsibilities:

- Game Over state
- Game speed
- Game speed progression
- Restarting the game
- Coordinating major systems

---

### `PlayerController`

Responsible for player gameplay.

Responsibilities:

- Player state
- Jumping
- Jump Cut
- Coyote Time
- Input Buffer
- Crouching
- Fast falling
- Collider switching
- Player visual state
- Player reset

---

### `PlayerInput`

Responsible for reading player input.

It converts raw keyboard input into gameplay-oriented values such as:

- `JumpPressed`
- `JumpReleased`
- `JumpHeld`
- `CrouchHeld`

This keeps input detection separate from player movement logic.

---

### `ObstacleSpawner`

Responsible for generating obstacles.

Responsibilities:

- Spawn timing
- Spawn distance
- Selecting available obstacle types
- Unlocking obstacles over time
- Passing the current game speed to spawned obstacles
- Resetting spawned obstacles

---

### `Obstacle`

Responsible for individual obstacle movement and collision-related behavior.

Each obstacle receives its movement speed from the `ObstacleSpawner`.

---

### `ObstacleData`

A ScriptableObject used to store obstacle configuration.

This separates obstacle data from spawning logic and allows different obstacle types to be configured independently.

---

### `ScoreManager`

Responsible for:

- Distance tracking
- Current score
- Session high score
- Score reset

---

### `GameOverUI`

Responsible for displaying:

- Current score
- High score
- Game Over panel
- Restart UI

Keeping UI logic separate from gameplay systems makes the GameManager and ScoreManager easier to maintain.

---

# 13. Object Cleanup

For this assignment, obstacles are currently destroyed after they are no longer needed rather than using Object Pooling.

The reason is that the game only creates a relatively small number of simple obstacle objects and the assignment prioritizes gameplay correctness and implementation time.

A pooling system would be a reasonable optimization for a larger-scale endless runner where many obstacles are continuously spawned and destroyed.

However, for the scope of this assignment, the simpler `Instantiate` / `Destroy` approach keeps the implementation easier to understand and reduces unnecessary system complexity.

---

# 14. QA Bug Analysis

The following two bugs were selected for analysis.

---

## BUG-01: Player Sometimes Passes Through Obstacles

### Possible Cause

A likely cause is a physics tunneling issue.

As game speed increases, the obstacle or player may move a relatively large distance between physics simulation steps. If the collision detection mode is not configured appropriately, an object can potentially move from one side of another collider to the other between physics checks.

This becomes more likely at higher game speeds, which matches the reported behavior of the bug occurring more frequently later in the game.

### How I Would Verify the Hypothesis

Before changing the code, I would:

1. Reproduce the issue at low speed and high speed.
2. Compare the Rigidbody2D velocity in both cases.
3. Inspect the collision detection mode of the relevant Rigidbody2D components.
4. Temporarily reduce the game speed and check whether the bug disappears.
5. Enable collision visualization / debugging to determine whether the objects physically overlap without generating a collision.

If the issue only occurs at high velocity and collision detection mode affects the reproduction rate, this would support the tunneling hypothesis.

### Proposed Fix

I would use an appropriate continuous collision detection mode for Rigidbody2D objects that require reliable collision detection at high speeds.

I would also verify:

- Rigidbody2D collision detection settings
- Collider configuration
- Physics layer configuration
- Physics timestep
- Obstacle movement implementation

### Prevention

I would add a regression test where the game is allowed to reach maximum speed and repeatedly test Player-obstacle collisions.

The test should be performed at the game's maximum configured speed rather than only at the starting speed.

---

## BUG-03: Player Falls Through the Ground After Switching Applications

### Possible Cause

A likely cause is a large elapsed time / physics timing discrepancy when the application loses focus and then resumes.

If the physics simulation does not resume in a controlled state, the Rigidbody2D may receive an unexpected physics update or accumulated timing difference, potentially causing the player to move significantly between collision checks.

### How I Would Verify the Hypothesis

I would:

1. Start the game and place the player on the ground.
2. Switch to another application.
3. Wait for different amounts of time.
4. Return to the game.
5. Observe the player's Rigidbody2D velocity and position.
6. Check whether the issue can be reproduced consistently.
7. Compare behavior with and without pausing the game when the application loses focus.

I would also inspect `Time.timeScale`, Rigidbody2D velocity, physics timestep settings, and the application's pause/focus behavior.

### Proposed Fix

I would explicitly handle application focus changes and ensure that the gameplay state is stable when the application resumes.

Depending on the desired game behavior, I would pause gameplay while the application is not focused and resume it when focus returns.

I would also ensure that the player's Rigidbody2D state is not given an unexpectedly large physics step after resuming.

### Prevention

I would add an application-focus regression test:

    Play
    ↓
    Lose application focus
    ↓
    Wait
    ↓
    Return to game
    ↓
    Verify player position / velocity / grounded state

This test should be performed both during normal gameplay and while the player is airborne.

---

# 15. If I Had 4 More Hours

If I had four additional hours, I would prioritize improvements in the following order:

### 1. Object Pooling

I would replace the current Instantiate / Destroy obstacle workflow with Object Pooling.

This would reduce repeated allocation and destruction of GameObjects during long runs and would make the system more scalable for a larger number of obstacle types.

---

### 2. More Robust Spawn Validation

I would add additional validation to ensure that randomly selected obstacles always produce a fair and reachable sequence.

For example, the spawner could take the current speed and obstacle properties into account when determining whether a particular obstacle combination is safe.

This would improve the reliability of the "always possible to dodge" requirement.

---

### 3. Automated / Repeatable Testing

I would create a small testing setup for:

- Restart behavior
- Maximum game speed
- Score reset
- High score persistence
- Obstacle spawning
- Player collision
- Player state transitions

This would make regression testing easier when modifying gameplay systems.

---

### 4. Code and Inspector Cleanup

I would perform another pass to:

- Remove unused references
- Improve naming consistency
- Organize serialized fields
- Add tooltips where useful
- Improve validation for missing references
- Review component dependencies

The goal would be to make the project easier for another developer to understand and maintain.

---

# 16. Development Process

The project was developed incrementally rather than as one large implementation.

The Git history is separated into multiple commits to show the development process and progression of the game.

The implementation was built in stages:

1. Player movement and jumping
2. Player state and crouching
3. Jump feel improvements
4. Obstacle implementation
5. Obstacle spawning
6. Game speed progression
7. Score system
8. Game Over and Restart
9. Game Feel
10. Visual polish and final testing

This approach allowed each gameplay system to be implemented and tested independently before adding the next system.

---

# 17. AI Usage

AI tools were used as a development assistant during this assignment.

They were primarily used for:

- Discussing implementation approaches
- Reviewing code structure
- Identifying potential edge cases
- Suggesting clean-code improvements
- Discussing gameplay programming concepts
- Reviewing technical decisions

The final implementation was reviewed, tested, and integrated manually.

I am responsible for understanding the submitted code and can explain the implementation and design decisions during the interview.


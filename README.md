# Behavior Tree Core

A lightweight, engine-agnostic behavior tree framework for Unity 6.3. Pure C#
nodes (no MonoBehaviour coupling except the single runner component), a
blackboard for sharing data, and a fluent builder for assembling trees in
code. Built to be dropped into any project — grid-based, top-down, 3D
side-scroller, whatever — without modification.

## Why this structure

- **Every node is plain C#.** Selector, Sequence, Decorator, etc. know
  nothing about GameObjects, NavMesh, or your game's data types. That's what
  makes the *same* Runtime/ folder reusable across different game types —
  the only per-project code is the leaf actions you plug in.
- **Blackboard, not fields.** Nodes read/write a shared `Blackboard` instead
  of holding references to game objects directly, so a tree built for a
  dungeon crawler enemy and one built for a top-down shooter enemy can share
  identical Selector/Sequence structure even though the underlying gameplay
  is different.
- **One MonoBehaviour.** `BehaviorTreeRunner` is the sole bridge into Unity's
  update loop. Everything else is testable outside Play mode.

## Reusing across your projects

Treat this folder as its own package, separate from any single game repo:

1. Put `com.yourstudio.behaviortree/` in its own git repository.
2. In each project (dungeon crawler, top-down shooter, Discovering The Hive),
   open **Window > Package Manager > + > Add package from git URL** and
   point it at that repo. All three projects then pull from one source —
   fix a bug in Selector once, it's fixed everywhere.
3. Alternatively, for now, just copy the `Runtime/` folder into each
   project's `Packages/` or `Assets/` — upgrading to the git-URL approach
   later is a matter of relocating the same files, no code changes.

## Core pieces

| Node | Role |
|---|---|
| `Selector` | Try children in order; succeed on the first success ("try this, or this") |
| `Sequence` | Run children in order; fail on the first failure ("do this, then this") |
| `Parallel` | Tick all children every frame with configurable success/failure policy |
| `Inverter` | Flips Success/Failure |
| `Repeater` | Re-runs a child N times or forever |
| `Cooldown` | Rate-limits a child |
| `ConditionDecorator` | Guards a subtree behind a boolean check |
| `ActionNode` / `ConditionNode` | Leaves wrapping your game-specific functions |

## Example

See `Samples~/EnemyAIExample.cs` for a patrol → chase → attack enemy built
with `BehaviorTreeBuilder`. The `~` suffix keeps Unity from compiling it by
default (standard convention for UPM sample folders) — import it via the
Package Manager's Samples tab, or just copy it in and rename the folder.

## Extending later

- **Visual editor**: nothing here assumes code-only authoring. A custom
  Editor window could serialize to the same `Node` tree — worth adding once
  you're hand-authoring a lot of trees rather than a handful of enemy types.
- **ScriptableObject configs**: for designer-tunable values (chase range,
  cooldowns), wrap parameters in a `ScriptableObject` and read them into the
  `Blackboard` at `Awake`, keeping the tree structure itself in code.

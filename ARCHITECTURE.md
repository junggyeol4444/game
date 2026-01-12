# System Architecture

## Overview Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                         Unity Game Engine                        │
│                         (2022.3 LTS)                             │
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                        GameManager                               │
│  • Main game controller                                          │
│  • Player data management                                        │
│  • Save/Load system                                              │
│  • Session management                                            │
└─────────────────────────────────────────────────────────────────┘
                                  │
         ┌────────────────────────┼────────────────────────┐
         ▼                        ▼                        ▼
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  DataManager    │    │   UIManager     │    │   Core Systems  │
│  • JSON Loading │    │   • Navigation  │    │   (Singletons)  │
│  • Templates    │    │   • Panels      │    │                 │
│  • Caching      │    │   • Dialogs     │    │                 │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                                              │
         │                                              │
         ▼                                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                        Game Systems                              │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ CardSystem   │  │BattleSystem  │  │ GachaSystem  │         │
│  │              │  │              │  │              │         │
│  │ • Create     │  │ • Grid (7x5) │  │ • Pools      │         │
│  │ • Level Up   │  │ • Turns      │  │ • Rarity     │         │
│  │ • Evolution  │  │ • Combat     │  │ • Pity       │         │
│  │ • Breeding   │  │ • AI         │  │ • Rates      │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│                                                                  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │ QuestSystem  │  │ IdleSystem   │  │Exploration   │         │
│  │              │  │              │  │              │         │
│  │ • Main       │  │ • Training   │  │ • World Map  │         │
│  │ • Side       │  │ • Dispatch   │  │ • Encounters │         │
│  │ • Daily      │  │ • Rewards    │  │ • Random     │         │
│  │ • Weekly     │  │ • Time-based │  │ • Fixed      │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│                                                                  │
│  ┌──────────────┐                                               │
│  │ EventSystem  │                                               │
│  │              │                                               │
│  │ • NPC        │                                               │
│  │ • Dungeon    │                                               │
│  │ • Choices    │                                               │
│  │ • Outcomes   │                                               │
│  └──────────────┘                                               │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                         Data Layer                               │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │  CardData    │  │  SkillData   │  │  QuestData   │         │
│  │  • Stats     │  │  • Effects   │  │  • Objectives│         │
│  │  • Skills    │  │  • Targets   │  │  • Rewards   │         │
│  │  • Evolution │  │  • Costs     │  │  • Progress  │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│                                                                  │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │  PlayerData  │  │  BattleData  │  │  GachaData   │         │
│  │  • Level     │  │  • Grid      │  │  • Pools     │         │
│  │  • Resources │  │  • Units     │  │  • Items     │         │
│  │  • Cards     │  │  • Turn      │  │  • Rates     │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
                                  │
                                  ▼
┌─────────────────────────────────────────────────────────────────┐
│                     Persistent Storage                           │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  StreamingAssets/Data/          Application.persistentDataPath  │
│  • cards.json                   • player_save.json              │
│  • skills.json                                                   │
│  • quests.json                                                   │
│  • gacha_pools.json                                              │
│  • encounters.json                                               │
│  • events.json                                                   │
│                                                                  │
└─────────────────────────────────────────────────────────────────┘
```

## Data Flow

### Game Initialization
```
Unity Start
    ↓
GameManager.Awake()
    ↓
Initialize DataManager
    ↓
Load JSON Templates
    ↓
Initialize All Systems
    ↓
Load Player Save or Create New
    ↓
Game Ready
```

### Battle Flow
```
Player Action (Exploration)
    ↓
Trigger Encounter
    ↓
ExplorationSystem.StartEncounter()
    ↓
BattleSystem.StartBattle()
    ↓
Initialize Grid (7x5)
    ↓
Place Units
    ↓
Calculate Turn Order
    ↓
┌─────────────────┐
│  Turn Loop      │
│  • Player Turn  │
│  • Enemy Turn   │
│  • Effects      │
│  • Check Win    │
└─────────────────┘
    ↓
Battle End
    ↓
Award Rewards
    ↓
Return to Exploration
```

### Gacha Flow
```
Player Opens Gacha
    ↓
Select Pool (Standard/Premium)
    ↓
Check Resources (Gems/Tickets)
    ↓
Perform Pull
    ↓
Calculate Rarity (with rates)
    ↓
Check Pity System
    ↓
Select Card from Pool
    ↓
Award to Player
    ↓
Update Pity Counter
    ↓
Display Results
```

### Quest Flow
```
Quest Available
    ↓
Player Accepts Quest
    ↓
Add to Active Quests
    ↓
Player Performs Actions
    ↓
Update Objectives
    ↓
Check Completion
    ↓
All Objectives Complete?
    ↓
Award Rewards
    ↓
Move to Completed
```

## System Communication

### Event-Based Communication (Ready for Implementation)
```
System A                    System B
   │                           │
   │──── Trigger Event ───────▶│
   │                           │
   │                      Process Event
   │                           │
   │◀──── Response (Optional)──│
   │                           │
```

### Direct Communication (Currently Used)
```
System A                    System B
   │                           │
   │──── Method Call ─────────▶│
   │                           │
   │◀──── Return Value ────────│
   │                           │
```

## Mobile Architecture

```
┌─────────────────────────────────────┐
│         Mobile Platform             │
│      (Android 22+ / iOS 11.0+)      │
└─────────────────────────────────────┘
                  │
                  ▼
┌─────────────────────────────────────┐
│         Unity Runtime               │
│         IL2CPP Backend              │
└─────────────────────────────────────┘
                  │
        ┌─────────┴─────────┐
        ▼                   ▼
┌──────────────┐    ┌──────────────┐
│  Rendering   │    │   Touch      │
│  (OpenGL ES/ │    │   Input      │
│   Metal)     │    │   System     │
└──────────────┘    └──────────────┘
```

## Performance Optimization Points

```
┌─────────────────────────────────────┐
│      Performance Strategy           │
├─────────────────────────────────────┤
│                                     │
│  1. Object Pooling (To Implement)   │
│     • Card objects                  │
│     • Battle units                  │
│     • UI elements                   │
│                                     │
│  2. Texture Atlasing (To Implement) │
│     • Card sprites                  │
│     • UI elements                   │
│     • Skill effects                 │
│                                     │
│  3. Batching (Configured)           │
│     • Static batching ON            │
│     • Dynamic batching ON           │
│                                     │
│  4. Memory Management               │
│     • Template caching              │
│     • JSON lazy loading             │
│     • Auto save/unload              │
│                                     │
│  5. Target 60 FPS                   │
│     • Lightweight shaders           │
│     • Optimized draw calls          │
│     • Efficient update loops        │
│                                     │
└─────────────────────────────────────┘
```

## Scalability Considerations

### Horizontal Scaling (Content)
- Add new cards → Edit cards.json
- Add new skills → Edit skills.json
- Add new quests → Edit quests.json
- Add new encounters → Edit encounters.json
- Add new events → Edit events.json

### Vertical Scaling (Features)
- New battle mechanics → Extend BattleSystem
- New card types → Extend CardData enum
- New quest types → Extend QuestSystem
- New gacha pools → Add to gacha_pools.json

### System Integration
```
New System
    ↓
1. Create in Assets/Scripts/Systems/
    ↓
2. Implement Singleton pattern
    ↓
3. Initialize in GameManager
    ↓
4. Add data model if needed
    ↓
5. Create JSON data if needed
    ↓
6. Integrate with existing systems
```

## Security Considerations

```
┌─────────────────────────────────────┐
│      Security Measures              │
├─────────────────────────────────────┤
│                                     │
│  Client-Side:                       │
│  • Save file encryption (TODO)      │
│  • Data validation                  │
│  • Input sanitization               │
│                                     │
│  Server-Side (Future):              │
│  • Server-side validation           │
│  • Anti-cheat measures              │
│  • Secure transactions              │
│  • Cloud save verification          │
│                                     │
└─────────────────────────────────────┘
```

## Development Workflow

```
1. Design Feature
    ↓
2. Update Data Model (if needed)
    ↓
3. Implement System Logic
    ↓
4. Create/Update JSON Data
    ↓
5. Test in Unity Editor
    ↓
6. Create UI (future)
    ↓
7. Test on Device
    ↓
8. Optimize Performance
    ↓
9. Deploy
```

This architecture provides a solid foundation for a scalable, maintainable mobile card strategy RPG game.

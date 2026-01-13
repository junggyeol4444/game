# Mobile Turn-Based Card Strategy RPG

A mobile turn-based card strategy RPG game built with Unity 2022.3 LTS for Android and iOS platforms.

## Project Overview

This is a comprehensive card-based strategy RPG featuring:
- Turn-based tactical combat on a 7x5 grid
- Card collection and progression system
- Gacha mechanics for card acquisition
- Exploration and random encounters
- Idle game mechanics (training, dispatch missions)
- Quest system (main, side, daily, weekly)
- Event system with NPC interactions

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/          # Core game systems
│   ├── Data/          # Data models and structures
│   ├── Systems/       # Game systems (Battle, Card, Gacha, etc.)
│   ├── Managers/      # Manager classes
│   └── UI/            # UI controllers
├── Scenes/            # Unity scenes
├── Prefabs/           # Reusable game objects
├── Resources/         # Runtime-loaded assets
└── StreamingAssets/
    └── Data/          # JSON data files
        ├── cards.json
        ├── skills.json
        ├── quests.json
        ├── gacha_pools.json
        ├── encounters.json
        └── events.json
```

## Features Implemented

### 1. Card System
- Card attributes (health, attack, defense, speed, magic, resistance)
- Level-up and experience system
- Card evolution mechanics
- Skill assignment to cards
- Breeding system for card combinations
- Relationship system between cards

### 2. Battle System
- 7x5 grid-based tactical combat
- Turn-based mechanics with speed-based turn order
- Player actions: Move, Attack, Use Skill, Defend
- AI for enemy units
- Status effects and buffs/debuffs
- Victory/defeat conditions

### 3. Exploration System
- World map exploration
- Random encounter system
- Fixed encounter locations
- Area transitions
- Battle initialization from encounters

### 4. Gacha System
- Multiple gacha pools (Standard, Premium, Event)
- Rarity-based card acquisition
- Pity system for guaranteed high-rarity cards
- Single and multi-pull mechanics
- Rate-up banners support

### 5. Quest System
- Main story quests
- Side quests
- Daily quests (auto-refresh)
- Weekly quests (auto-refresh)
- Quest objectives tracking
- Reward distribution

### 6. Idle Systems
- Training system for card experience
- Dispatch missions for passive rewards
- Idle reward accumulation
- Time-based completion tracking

### 7. Event System
- NPC interaction events
- Dungeon events
- Choice-based outcomes
- Requirement checking
- Rewards and consequences

### 8. Resource Management
- Gold, Gems, and Tickets currency
- Energy system with regeneration
- Player level and experience
- Save/load functionality

## Technical Details

### Unity Version
- Unity 2022.3 LTS

### Target Platforms
- Android (API Level 22+)
- iOS (11.0+)

### Orientation
- Landscape only

### Target Performance
- 60 FPS on mobile devices
- Optimized for touch controls

### Data Management
- JSON-based data storage
- Persistent save system
- StreamingAssets for game data

## Setup Instructions

1. **Open Project**
   - Open Unity Hub
   - Add project from disk
   - Select Unity 2022.3 LTS
   - Open the project

2. **Build for Android**
   - File → Build Settings
   - Select Android platform
   - Switch Platform
   - Configure Player Settings
   - Build APK

3. **Build for iOS**
   - File → Build Settings
   - Select iOS platform
   - Switch Platform
   - Configure Player Settings
   - Build Xcode project

## Game Systems Documentation

### Card Data Structure
Cards have the following properties:
- ID, Name, Description
- Rarity (Common to Mythic)
- Type (Warrior, Mage, Ranger, Tank, Support, Assassin)
- Level and Experience
- Stats (HP, Attack, Defense, Speed, Magic, Resistance)
- Skills
- Evolution path
- Breeding compatibility

### Battle Mechanics
- Grid-based movement with range limits
- Speed determines turn order
- Damage calculation: `BaseDamage - (Defense / 2)`
- Skills have cooldowns and mana costs
- Status effects persist for multiple turns

### Gacha Rates
- Common: ~40-50%
- Uncommon: ~25-35%
- Rare: ~10-20%
- Epic: ~3-8%
- Legendary: ~1-3%
- Mythic: ~0.1-1%

### Energy System
- Max energy determined by player level
- Regenerates 1 energy per 5 minutes
- Required for battles and exploration

## Adding New Content

### Adding a New Card
1. Edit `Assets/StreamingAssets/Data/cards.json`
2. Add new card entry with all required fields
3. Assign skills and stats
4. Set rarity and type

### Adding a New Skill
1. Edit `Assets/StreamingAssets/Data/skills.json`
2. Define skill properties and effects
3. Set mana cost, cooldown, and range

### Adding a New Quest
1. Edit `Assets/StreamingAssets/Data/quests.json`
2. Create quest with objectives
3. Define rewards and prerequisites

## Mobile Optimization

- Texture compression enabled
- Batching for UI elements
- Object pooling for frequently spawned objects
- Lightweight shaders
- Optimized particle effects
- Touch input handling

## Future Enhancements

Potential features for future development:
- PvP battle system
- Guild/clan system
- Real-time multiplayer
- Seasonal events
- Card skin system
- Achievement system
- Leaderboards
- Push notifications
- Cloud save sync
- More card types and rarities
- Additional battle mechanics
- Story mode cutscenes

## Development Notes

### Code Architecture
- Singleton pattern for managers
- Data-driven design using JSON
- Separation of concerns (Data, Systems, UI)
- Event-driven communication between systems

### Performance Considerations
- Mobile-first optimization
- Asset bundle support for future updates
- Memory management for large card collections
- Efficient UI updates

## Credits

Developed using Unity 2022.3 LTS
Target platforms: Android and iOS

## License

[Add your license information here]
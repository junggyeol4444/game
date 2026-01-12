# Quick Start Guide

## For Developers

### Opening the Project

1. **Install Unity Hub** (if not already installed)
   - Download from: https://unity.com/download

2. **Install Unity 2022.3 LTS**
   - Open Unity Hub
   - Go to Installs tab
   - Click "Install Editor"
   - Select Unity 2022.3 LTS
   - Include Android Build Support and iOS Build Support

3. **Open Project**
   - In Unity Hub, click "Add"
   - Navigate to the project folder
   - Select the folder containing this README
   - Click "Open"

### Understanding the Project

#### Key Files to Start With

1. **Assets/Scripts/Managers/GameManager.cs**
   - Main entry point for the game
   - Handles player data and game state
   - Good place to understand overall flow

2. **Assets/Scripts/Systems/BattleSystem.cs**
   - Core combat mechanics
   - Turn-based battle logic
   - AI implementation

3. **Assets/StreamingAssets/Data/cards.json**
   - Sample card definitions
   - Easy to add new cards here

#### Testing the Systems

To test systems in Unity Editor:

1. **Create a Test Scene**
   - Create new scene: File → New Scene
   - Add empty GameObject named "GameManager"
   - Attach GameManager.cs component

2. **Test Card System**
   ```csharp
   // In any MonoBehaviour:
   void Start()
   {
       CardData card = CardSystem.Instance.CreateCard("card_001", 1);
       Debug.Log($"Created card: {card.name}");
   }
   ```

3. **Test Battle System**
   ```csharp
   // Create a simple battle:
   BattleData battle = new BattleData();
   // ... setup battle data
   BattleSystem.Instance.StartBattle(battle);
   ```

### Project Structure at a Glance

```
Assets/
├── Scripts/
│   ├── Data/           → Data structures (read these first)
│   ├── Systems/        → Game logic (read these second)
│   ├── Managers/       → Singletons that control everything
│   ├── UI/             → User interface controllers
│   └── Utils/          → Helper functions and constants
├── StreamingAssets/
│   └── Data/           → JSON files (easiest to edit)
├── Scenes/             → Unity scenes
└── Prefabs/            → Reusable game objects (to be created)
```

### Common Tasks

#### Adding a New Card

1. Open `Assets/StreamingAssets/Data/cards.json`
2. Copy an existing card entry
3. Change the ID and properties
4. Save the file
5. The card is now available in the game!

#### Adding a New Skill

1. Open `Assets/StreamingAssets/Data/skills.json`
2. Add new skill entry
3. Reference the skill ID in a card's `skillIds` array

#### Creating a New Quest

1. Open `Assets/StreamingAssets/Data/quests.json`
2. Add quest with objectives
3. Set rewards
4. Done!

### Building and Testing

#### Quick Test in Editor
- Press Play button in Unity Editor
- Check Console for any errors
- Systems initialize automatically

#### Build for Android
1. File → Build Settings
2. Select Android
3. Click "Build"
4. Wait for APK generation

#### Build for iOS
1. File → Build Settings
2. Select iOS
3. Click "Build"
4. Open generated Xcode project
5. Build from Xcode

### Debugging Tips

1. **Use Debug.Log extensively**
   ```csharp
   Debug.Log("Battle started!");
   Debug.LogWarning("Low health!");
   Debug.LogError("Something went wrong!");
   ```

2. **Check Console Window**
   - Window → General → Console
   - Shows all debug messages and errors

3. **Use Unity Profiler**
   - Window → Analysis → Profiler
   - Monitor performance in real-time

4. **Breakpoints in Visual Studio**
   - Attach Unity debugger
   - Set breakpoints in code
   - Step through execution

### What's Implemented vs What Needs UI

#### ✅ Fully Implemented (Backend)
- Card system with all mechanics
- Battle system with AI
- Gacha system
- Quest system
- Idle systems
- Exploration
- Events
- Save/Load

#### 🎨 Needs Implementation (Frontend)
- UI screens
- Visual card displays
- Battle animations
- Touch controls
- Menus and navigation
- Effects and particles
- Sound effects
- Music

### Next Steps for Development

1. **Create UI Screens**
   - Main menu
   - Card collection view
   - Battle interface
   - Gacha screen
   - Quest log

2. **Add Visual Assets**
   - Card artwork
   - Skill effects
   - Battle grid visuals
   - Character sprites

3. **Implement Touch Controls**
   - Drag and drop for cards
   - Tap to select
   - Swipe for navigation

4. **Add Polish**
   - Animations
   - Transitions
   - Particle effects
   - Sound design

### Getting Help

- **Documentation**: See README.md and SETUP_GUIDE.md
- **Implementation Details**: See IMPLEMENTATION_SUMMARY.md
- **Code Comments**: Most classes have inline documentation
- **Unity Manual**: https://docs.unity3d.com/Manual/index.html

### Important Notes

⚠️ **Before You Start Coding**
- Read through the existing code to understand the architecture
- Follow the existing patterns (Singleton for managers, etc.)
- Keep data separate from logic (use JSON files)
- Test frequently in Unity Editor

⚠️ **Mobile Development**
- Always test on actual devices
- Monitor performance with Profiler
- Keep 60 FPS target in mind
- Optimize for touch input

⚠️ **Version Control**
- Commit frequently
- Write descriptive commit messages
- Don't commit generated files (Library, Temp, etc.)
- .gitignore is already set up

### Quick Reference

#### Singleton Access Pattern
```csharp
GameManager.Instance.GetPlayerData();
CardSystem.Instance.CreateCard("card_001", 1);
BattleSystem.Instance.StartBattle(battleData);
```

#### Loading Data
```csharp
// Data loads automatically on game start
CardData template = DataManager.Instance.GetCardTemplate("card_001");
```

#### Save/Load
```csharp
// Auto-saves on pause/quit
// Manual save:
GameManager.Instance.SavePlayerData();

// Manual load:
GameManager.Instance.LoadPlayerData();
```

### Troubleshooting

**Problem**: Scripts not compiling
- **Solution**: Check Console for errors, fix any syntax issues

**Problem**: Data not loading
- **Solution**: Verify JSON files are in StreamingAssets/Data folder

**Problem**: Scene not loading
- **Solution**: Check Build Settings → Scenes in Build

**Problem**: Build fails
- **Solution**: Clear build cache, restart Unity, try again

## Happy Coding! 🎮

Start by exploring the code, running the project in Unity Editor, and experimenting with the JSON data files. The systems are all functional and ready for you to build the UI on top of!

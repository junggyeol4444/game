# Setup and Development Guide

## Prerequisites

### Required Software
- Unity Hub (latest version)
- Unity 2022.3 LTS
- Visual Studio 2022 or Visual Studio Code with C# extension
- Git

### For Android Development
- Android SDK (API Level 22 or higher)
- Android NDK
- JDK 8 or higher
- Gradle

### For iOS Development (Mac only)
- Xcode 13 or later
- iOS SDK 11.0 or higher
- CocoaPods

## Initial Setup

### 1. Clone the Repository
```bash
git clone <repository-url>
cd game
```

### 2. Open in Unity
1. Open Unity Hub
2. Click "Add" and select the project directory
3. Ensure Unity 2022.3 LTS is installed
4. Open the project

### 3. Project Configuration
The project should be pre-configured, but verify:
- **Orientation**: Landscape (Left/Right)
- **Target API**: Android 22+, iOS 11.0+
- **Scripting Backend**: IL2CPP for both platforms
- **Architecture**: ARM64 (both platforms)

## Project Structure Explained

### Scripts Organization

#### /Assets/Scripts/Data/
Data models and structures:
- `CardData.cs` - Card entity structure
- `SkillData.cs` - Skill definitions
- `QuestData.cs` - Quest system data
- `PlayerData.cs` - Player state and progression
- `BattleData.cs` - Battle state management
- `ExplorationData.cs` - World exploration data
- `GachaData.cs` - Gacha system data
- `EventData.cs` - Event system data

#### /Assets/Scripts/Systems/
Game logic systems:
- `CardSystem.cs` - Card management, leveling, evolution
- `BattleSystem.cs` - Combat mechanics and AI
- `GachaSystem.cs` - Card acquisition system
- `QuestSystem.cs` - Quest tracking and completion
- `IdleSystem.cs` - Passive gameplay features
- `ExplorationSystem.cs` - World navigation
- `EventSystem.cs` - Event handling

#### /Assets/Scripts/Managers/
Singleton managers:
- `GameManager.cs` - Main game controller
- `DataManager.cs` - JSON data loading

#### /Assets/Scripts/UI/
User interface:
- `UIManager.cs` - UI navigation and state

### Data Files (/Assets/StreamingAssets/Data/)

All game data is stored in JSON format:

#### cards.json
Defines all cards in the game with:
- Basic info (id, name, description)
- Rarity and type
- Base and current stats
- Evolution paths
- Available skills
- Breeding compatibility

#### skills.json
Defines all skills with:
- Skill info (id, name, description)
- Type and target
- Cost and cooldown
- Range and power
- Effects and status conditions

#### quests.json
Defines all quests with:
- Quest info (id, name, type)
- Objectives and requirements
- Rewards (gold, gems, exp, cards)
- Prerequisites

#### gacha_pools.json
Defines gacha pools with:
- Pool info (id, name, type)
- Card drop rates by rarity
- Costs and pity system
- Rate-up cards

#### encounters.json
Defines battle encounters with:
- Enemy compositions
- Encounter types (random/fixed)
- Rewards and drop rates

#### events.json
Defines world events with:
- Event descriptions
- Choices and outcomes
- Requirements and rewards

## Development Workflow

### Adding New Features

1. **Create Data Models**
   - Define data structures in `/Assets/Scripts/Data/`
   - Use `[Serializable]` for JSON support

2. **Implement System Logic**
   - Create system class in `/Assets/Scripts/Systems/`
   - Use singleton pattern if needed
   - Integrate with DataManager

3. **Add UI Components**
   - Create UI scripts in `/Assets/Scripts/UI/`
   - Hook up to UIManager

4. **Create JSON Data**
   - Add sample data to appropriate JSON file
   - Test loading in DataManager

### Testing

1. **Play Mode Testing**
   - Test in Unity Editor play mode
   - Use Debug.Log for diagnostics

2. **Build Testing**
   - Test on Android device/emulator
   - Test on iOS device/simulator
   - Monitor performance

### Common Development Tasks

#### Adding a New Card
1. Edit `cards.json`
2. Add card entry with all fields
3. Create skill entries in `skills.json`
4. Test in game

#### Creating a New Quest
1. Edit `quests.json`
2. Define objectives
3. Set rewards
4. Test completion flow

#### Implementing a New Battle Feature
1. Modify `BattleSystem.cs`
2. Update `BattleData.cs` if needed
3. Test in various scenarios

## Building the Game

### Android Build

1. **Configure Build Settings**
   ```
   File → Build Settings
   Platform: Android
   ```

2. **Player Settings**
   - Company Name: [Your Company]
   - Product Name: CardStrategyRPG
   - Package Name: com.yourcompany.cardstrategyrpg
   - Version: 1.0
   - Bundle Version Code: 1
   - Minimum API Level: 22
   - Target API Level: Automatic (highest installed)
   - Scripting Backend: IL2CPP
   - Target Architectures: ARM64

3. **Build**
   - Click "Build" or "Build And Run"
   - Select output location
   - Wait for build to complete

### iOS Build

1. **Configure Build Settings**
   ```
   File → Build Settings
   Platform: iOS
   ```

2. **Player Settings**
   - Company Name: [Your Company]
   - Product Name: CardStrategyRPG
   - Bundle Identifier: com.yourcompany.cardstrategyrpg
   - Version: 1.0
   - Build: 1
   - Target minimum iOS Version: 11.0
   - Architecture: ARM64

3. **Build**
   - Click "Build"
   - Select output location
   - Open generated Xcode project
   - Configure signing in Xcode
   - Build and run from Xcode

## Performance Optimization

### Mobile Performance Tips

1. **Texture Settings**
   - Use compressed texture formats
   - Enable mipmaps for 3D textures
   - Use sprite atlases for UI

2. **Batching**
   - Enable static batching
   - Use dynamic batching for small meshes
   - Group UI elements

3. **Draw Calls**
   - Minimize material count
   - Use texture atlases
   - Combine meshes where possible

4. **Memory Management**
   - Unload unused assets
   - Use object pooling
   - Avoid memory allocations in update loops

5. **Physics**
   - Minimize physics calculations
   - Use simple colliders
   - Optimize raycast usage

## Debugging

### Common Issues

#### Data Not Loading
- Check StreamingAssets path
- Verify JSON syntax
- Check file names match code

#### Battle System Issues
- Verify grid initialization
- Check turn order calculation
- Monitor unit positions

#### Save/Load Issues
- Check Application.persistentDataPath
- Verify JSON serialization
- Test on actual device

### Debug Tools

- Unity Profiler for performance
- Android Logcat for Android logs
- Xcode Console for iOS logs
- Debug.Log statements in code

## Version Control

### Git Workflow

1. Create feature branch
2. Make changes
3. Test thoroughly
4. Commit with descriptive message
5. Push and create PR
6. Merge after review

### .gitignore
Already configured to ignore:
- Unity generated files
- Build output
- IDE files
- OS files

## Deployment

### Android Deployment

1. **Google Play Console**
   - Create app listing
   - Upload APK/AAB
   - Fill store listing
   - Submit for review

2. **Signing**
   - Create keystore
   - Configure in Unity
   - Keep keystore secure

### iOS Deployment

1. **App Store Connect**
   - Create app record
   - Upload build via Xcode
   - Fill app information
   - Submit for review

2. **Certificates**
   - Create distribution certificate
   - Create provisioning profile
   - Configure in Xcode

## Maintenance

### Regular Tasks

- Update Unity version (LTS releases)
- Update SDKs (Android/iOS)
- Review and update dependencies
- Monitor crash reports
- Respond to user feedback

### Data Updates

- Add new cards periodically
- Create seasonal quests
- Update gacha pools
- Add new events

## Support and Resources

### Unity Documentation
- [Unity Manual](https://docs.unity3d.com/Manual/index.html)
- [Scripting API](https://docs.unity3d.com/ScriptReference/index.html)

### Platform Documentation
- [Android Developer](https://developer.android.com/)
- [iOS Developer](https://developer.apple.com/)

### Community
- Unity Forums
- Stack Overflow
- Reddit r/Unity3D

## Troubleshooting

### Build Errors
1. Clean build folder
2. Reimport all assets
3. Check console for errors
4. Verify SDK installations

### Runtime Errors
1. Check device logs
2. Test in Unity Editor
3. Use try-catch blocks
4. Enable detailed logging

### Performance Issues
1. Use Profiler to identify bottlenecks
2. Optimize hot paths
3. Reduce draw calls
4. Optimize memory usage

## Next Steps

1. Create UI scenes and prefabs
2. Add placeholder art assets
3. Implement audio system
4. Create tutorial system
5. Add analytics
6. Implement monetization
7. Add localization support
8. Create automated tests

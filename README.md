# Zombieverse AR

![Platform](https://img.shields.io/badge/Platform-Android-green?style=flat-square)
![Engine](https://img.shields.io/badge/Engine-Unity-black?style=flat-square)
![Language](https://img.shields.io/badge/Language-C%23-239120?style=flat-square)

---

## About

Zombieverse AR is a mobile first-person zombie shooter built in Unity that runs in two modes: a full **AR mode**, where zombies are spawned into your real surroundings using AR Foundation and ARCore, and a **Virtual mode**, where the same game runs on a generated terrain with gyroscope-based camera look. The player fights off endless waves of zombies that scale up through easy, medium, and hard difficulty tiers as the kill count climbs.

This repository contains the complete Unity project, open-sourced for reference and learning.

---

## Demo

[![Watch the gameplay demo](https://img.youtube.com/vi/ngsoClk9jGU/hqdefault.jpg)](https://youtu.be/ngsoClk9jGU)

*Click the thumbnail to watch on YouTube (GitHub doesn't allow videos to autoplay inside a README).*

---

## Features

- Dual game modes: real-world AR spawning (AR Foundation / ARCore) or a Virtual mode on a generated terrain with gyroscope camera control
- Endless wave-based zombie spawning with three escalating difficulty tiers (easy -> medium -> hard), each unlocking a new weapon (pistol -> AKM -> machine gun)
- Raycast hitscan weapon system with magazine/reload logic
- Multiple zombie types with their own animation controllers and ragdoll death physics
- Antidote power-up system: earn antidotes from high-score bonuses, activate them for a timed in-game effect
- Daily login bonus system
- Minimap and in-game HUD (health, ammo, kill counter)
- Rewarded video ads (Google Mobile Ads) tied to the in-game revive/bonus flow, plus banner ads
- Push notifications and a "invite a friend" share flow
- Firebase integration for backend services

---

## Architecture

```
Zombieverse
|
+-- Scripts/                       # Core gameplay code
|   +-- RaycastWeapon.cs               # Hitscan shooting, damage, reload
|   +-- gameManager.cs                 # Mode switching (AR/Virtual), pause menu, antidote mode
|   +-- EndlessLevels.cs               # Wave spawning, difficulty scaling, weapon unlocks
|   +-- PlayerHealth.cs                # Player damage, death, ad-based revive
|   +-- ZombieHealth.cs                # Zombie damage, death, scoring
|   +-- HitBox.cs                      # Hit detection relay to weapons
|   +-- Ragdoll.cs                     # Physics ragdoll on zombie death
|   +-- healthBar.cs / AntidoteBar.cs  # HUD elements
|   +-- MiniMap.cs                     # Top-down minimap tracking
|   +-- GyroCameraController.cs        # Gyroscope-driven camera for Virtual mode
|   +-- CrossHairTarget.cs             # Aim reticle logic
|   +-- movePath.cs                    # Zombie navigation/movement
|   +-- AudioManager.cs                # SFX/music playback
|   +-- Banner.cs / RewardedAdManager.cs   # Google Mobile Ads integration
|   +-- notifications.cs / pushNotifications.cs  # Local/push notifications
|   +-- HomeNav.cs                     # Main menu, stats, daily bonus, share/invite flow
|
+-- Firebase/                      # Firebase Unity SDK
+-- GoogleMobileAds/                # Google Mobile Ads (AdMob) SDK
+-- Custom Assets/                 # Guns, zombies, UI art
+-- ZombiePrefabs/                 # Zombie prefab variants
+-- ExampleAssets/                 # Unity AR Foundation sample assets
+-- TextMesh Pro/                  # UI text rendering
```

---

## Tech Stack

| Layer      | Technology                                              |
| ---------- | -------------------------------------------------------- |
| Engine     | Unity                                                     |
| Language   | C#                                                        |
| Platform   | Android (AR + non-AR mobile touch controls)               |
| AR         | AR Foundation, ARCore                                     |
| Backend    | Firebase                                                   |
| Ads        | Google Mobile Ads (AdMob) - rewarded video + banner        |
| UI         | TextMesh Pro                                               |
| Physics    | Unity ragdoll + rigidbody-based hit reactions               |

---

## Project Structure

```
Zombieverse-AR/
+-- Assets/
|   +-- Scripts/               # Core gameplay scripts (weapons, health, waves, UI, ads)
|   +-- Custom Assets/         # Guns, zombies, UI art
|   +-- ZombiePrefabs/         # Zombie prefab variants
|   +-- Firebase/              # Firebase Unity SDK
|   +-- GoogleMobileAds/       # AdMob SDK
|   +-- ExampleAssets/         # AR Foundation sample scripts/prefabs
|   +-- Scenes/                # Game scenes
|   +-- TextMesh Pro/          # UI text rendering package
+-- Packages/                  # Unity package manifest
+-- ProjectSettings/           # Unity project configuration
```

---

## Getting Started

### Prerequisites

- Unity Hub with a Unity version compatible with this project (see `ProjectSettings/ProjectVersion.txt`)
- Android Build Support module, with ARCore support enabled, for on-device builds
- A Firebase project, if you want Firebase-backed features to work
- An AdMob account, if you want real (non-test) ads

### Setup

1. Clone the repository:

   ```
   git clone https://github.com/nikhilkeetha/Zombieverse-AR.git
   ```

2. Open the project folder in Unity Hub.

3. Let Unity import all assets and resolve packages (this can take a few minutes on first open).

4. Firebase config isn't included in this repo. Copy `Assets/google-services.json.example` to `Assets/google-services.json` and fill it in with your own Firebase project's values from the Firebase Console (Project Settings > Your apps).

5. This repo ships with Google's public **test** AdMob ad unit/app IDs wired up (`RewardedAdManager.cs` and `Assets/GoogleMobileAds/Resources/GoogleMobileAdsSettings.asset`). Swap in your own AdMob IDs before publishing a build.

6. Open the main scene under `Assets/Scenes` and hit Play.

---

## Disclaimer

This project is open-sourced as a reference implementation of an AR mobile shooter built in Unity, not a maintained or production-ready game.

---

## License

This project is open source and available under the [MIT License](LICENSE).

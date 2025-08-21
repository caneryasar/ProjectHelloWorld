# 🎮 3D Platformer Technology Demo – Unity (Master's Project)

This repository contains a technology demo developed using the Unity game engine as a part of the Master's Degree Graduation Project at Hacettepe University, Computer Animation and Game Technologies program.

The project demonstrates a modular, extensible, and performance-optimized third-person 3D platformer framework featuring core gameplay mechanics, enemy AI, animation systems, and UI integration—built using modern development principles and Unity's latest tools.

---

## 🧠 Project Purpose

The goal of this project is to explore and implement the core mechanics of a 3D platformer game, emphasizing:
- Hands-on application of Unity technologies
- Strong modular code architecture
- Reusable gameplay systems (e.g., event system, enemy AI, state machines)
- A polished player experience through responsive controls and animations

This demo serves as both a learning exercise and a foundational framework for future game projects.

---

## 🔑 Key Features

### 🧍 Player Mechanics
- Third-person controller using `CharacterController`
- Walk, run, jump, **double jump**, and **dash**
- Smooth camera with Cinemachine
- Input management with Unity's New Input System

### 🤖 Enemy AI
- Hierarchical enemy control system using parent/child structure
- State machine-based AI with states for idle, chase, attack, etc.
- Visual feedback via world-space UI indicators (status, health)

### ⚔️ Combat System
- Melee combat interactions
- Event-driven system to broadcast and react to in-game events
- Health tracking for enemies and player

### 📺 UI & Feedback
- World-space and screen-space canvases
- Dynamic health bars and focus indicators
- Responsive player feedback through animation and VFX

### 🎞️ Animations
- Unity Mecanim system with layered animation blending
- Player and enemy animations controlled via parameter-driven state transitions

---

## 🛠️ Technologies Used

| Tool / Framework         | Description                                     |
|--------------------------|-------------------------------------------------|
| **Unity**                | Unity 2022+ (URP)                               |
| **Language**             | C#                                              |
| **IDE**                  | JetBrains Rider                                 |
| **Input System**         | Unity New Input System                          |
| **Camera System**        | Unity Cinemachine                               |
| **Level Design**         | Unity ProBuilder                                |
| **AI Navigation**        | Unity NavMesh                                   |
| **Animation**            | Unity Mecanim Animation System                  |
| **Tweening**             | DoTween                                         |
| **Reactive Programming** | UniRx                                           |
| **Profiling**            | Unity Profiler                                  |
| **Version Control**      | Git + GitHub                                    |

---

## 🖥️ References and Credits

### Assets

- [Jammo Character](https://assetstore.unity.com/packages/3d/characters/jammo-character-mix-and-jam-158456**) by Mix and Jam

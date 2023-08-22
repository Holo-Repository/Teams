# HoloRepository: Unity3D-ModelViewer

## Table of Contents
1. [Introduction](#introduction)
2. [Features](#features)
3. [Getting Started](#getting-started)
4. [Usage](#usage)
5. [Known Issues](#known-issues)

## Introduction
HoloCollab is a cutting-edge collaboration platform designed to seamlessly integrate with Microsoft Teams, allowing users to manipulate 2D and 3D models in real-time. Developed as part of the Software Systems Engineering MSc Industry Project, this platform leverages the power of the Unity engine to provide a robust model manipulation framework.

## Features

### Model Retrieval and Manipulation
- **Direct URL Access**: Users can retrieve models from any source by simply providing a directly accessible URL.
- **Real-time Manipulation**: Once a model is loaded, users can:
  - Rotate the model.
  - Zoom in and out.
  - Annotate on the model's surface.

### Waypoint System
- **Record and Load**: Users can record their model manipulation setups (rotation and zoom) and load them at any time.

### Direct Annotation
- **Draw on Models**: Users can directly draw or annotate on the 3D model's surface.
- **Pre-made Assets**: Due to the diversity of 3D model formats, we utilize pre-made assets to ensure compatibility and functionality.

## Getting Started

### Prerequisites
- Ensure you have the latest version of Unity installed.
- Microsoft Teams integration requires a valid Teams account and that your account is allowed to sideload apps.

### Installation
1. Clone the repository: git clone https://github.com/[username]/HoloCollab.git
2. Open the project in Unity.
3. Load the scene "Main" from the Assets/Scenes file.

## Usage

### Manipulating a Model
1. Models can be moved by right-clicking and dragging on the screen.
2. Zoom in and out using the mouse wheel or with an equivalent touchpad motion.

### Loading a Model
1. Open the HoloCollab platform within Microsoft Teams.
2. Provide a directly accessible URL to load your desired model.
3. Once loaded, use the provided tools to manipulate the model as needed.

### Using the Waypoint System
1. Manipulate the model to your desired setup.
2. Click on the "Save Waypoint" button represented by a floppy disc.
3. To load a setup, select the desired waypoint from the list.

### Annotating on a Model
1. Annotations can be made using the left-click button on your mouse or touchpad device.
2. Click and drag on the model's surface to draw or annotate.
3. Only drawing with a single brush type with red paint is supported for now.

## Known Issues
- Some 3D model formats may not be fully compatible with the direct annotation feature. We recommend testing your model beforehand.

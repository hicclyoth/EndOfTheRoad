# End of The Road

## Table of Contents

1. [Project Setup](#project-setup)
2. [Git Workflow](#git-workflow)
3. [Keep your Fork Up to Date](#keeping-your-fork-up-to-date)
4. [Using the Trap System](#using-the-trap-system)

## Project Setup

### 1. Clone the Repository

To get started, clone this repository to your local machine. Ensure you have **Git** and **GitHub Desktop** set up.

- **Using GitHub Desktop:**

  1. Open **GitHub Desktop**.
  2. Go to **File > Clone Repository**.
  3. Select **"Platformer Game Repository"** and choose a directory to clone it.

- **Using Command Line:**
  ```bash
  git clone https://github.com/your-username/your-repo-name.git
  ```

### 2. Open the Project in Unity

1. Open **Unity Hub**.
2. Click **Add** and navigate to the cloned repository folder.
3. Select the folder and click **Open**.

Unity will automatically open the project, and you will see all the assets, scripts, and game settings.

## Git Workflow

Follow these steps for a smooth collaboration workflow:

### 1. Fork the Repository

If you do not have direct push access to the main repository, you need to fork it first:

- Go to the repository page on GitHub.
- Click the **Fork** button in the top right corner.
- Choose your GitHub account to create your fork.

### 2. Clone Your Fork

Clone your forked repository to your local machine:

```bash
git clone https://github.com/your-username/your-repo-name.git
```

### 3. Create a New Branch

After cloning your fork, navigate to the repository directory and create a new branch for your feature or bug fix:

```bash
git checkout -b feature/TrapMovement
```

### 4. Make Changes and Commit

Make the necessary changes to the project. When you’re done, commit your changes with a meaningful message:

```bash
git commit -m "Added customizable directional movement to traps"
```

### 5. Push Changes to Your Fork

Push your changes to your fork on GitHub (not the main repository):

```bash
git push origin feature/TrapMovement
```

### 6. Create a Pull Request

Go to your forked repository on GitHub and create a pull request (PR) to merge your feature branch into the `dev` branch of the main repository.

- Click on the **Compare & pull request** button.
- Select `dev` as the base branch and your feature branch as the compare branch.
- Add a meaningful title and description.

#### Example PR Title:

```text
Added customizable trap movement direction
```

## Keeping Your Fork Up to Date

If your fork becomes out of sync with the main repository, follow these steps to update it:

### 1. Add the Original Repository as a Remote (Upstream)

Open your terminal in your local repository directory and run:

```bash
git remote add upstream https://github.com/hicclyoth/EndOfTheRoad.git
```

### 2. Fetch the Latest Changes

Fetch the latest changes from the main repository:

```bash
git fetch upstream
```

### 3. Merge Changes into Your Local Branch

Switch to your local `dev` branch (or any branch you want to update):

```bash
git checkout dev
```

Merge the changes from the upstream `dev` branch:

```bash
git merge upstream/dev
```

### 4. Push the Updated Branch to Your Fork

After merging, push the updated branch to your fork:

```bash
git push origin dev
```

### 5. Update Your Feature Branch (if needed)

If you are working on a feature branch, update it by rebasing or merging the latest `dev`:

#### Using rebase:

```bash
git checkout feature/TrapMovement
git rebase dev
```

#### Or using merge:

```bash
git checkout feature/TrapMovement
git merge dev
```

## Using the Trap System

This project contains a customizable trap movement system. The following steps will guide you through setting it up.

### 1. Assign the Player

Each trap needs to know where the player is located. Assign the player GameObject in Unity:

- **Tag the Player**: Ensure the player GameObject has the **"Player"** tag assigned.
  - Select the player GameObject in the Inspector.
  - In the **Tag** dropdown, select **"Player"**.

### 2. Setting Up the Proximity Detector

Each trap requires a proximity detector to trigger its movement:

1. **Create a Trigger Object**:

   - Select the **Trap GameObject**.
   - Right-click in the **Hierarchy** and choose **Create Empty**.
   - Rename it to **"Trigger"**.
   - Add a **Box Collider 2D** to the Trigger object by clicking **Add Component** and searching for **Box Collider 2D**.
   - In the **Box Collider 2D**, check the **Is Trigger** checkbox.

2. **Add the Proximity Detector Script**:

   - Select the **Trigger** GameObject.
   - Attach the **ProximityDetector** script to the Trigger by clicking **Add Component** and searching for **ProximityDetector**.

3. **Assign the Player to the Proximity Detector**:

   - In the **Inspector**, find the **Player** field in the **ProximityDetector** component.
   - Drag and drop the **Player GameObject** into this field.

4. **Assign the Trap Target**:
   - Drag and drop the **Trap GameObject** into the **Target Trap** field of the **ProximityDetector** script.

### 3. Configuring the Moving Trap

The **MovingTrap** script controls the trap's movement. You can customize the direction and distance as follows:

1. Select the trap GameObject and add the **MovingTrap** script.
2. Configure the **Direction** dropdown to specify the movement (Left, Right, Up, or Down).
3. Set the **Move Distance** (e.g., `4` to move 4 units).
4. Adjust the **Move Speed** for how fast the trap should move.

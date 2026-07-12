#!/bin/bash

# Exit instantly if any command fails
set -e

# --- Configuration ---
MOD_NAME="Faster-Resource"
MODS_DIR="/mnt/240GB_SSD/SteamLibrary/steamapps/common/RimWorld/Mods"
TARGET_DIR="$MODS_DIR/$MOD_NAME"

echo "Building Mod Assemblies..."

# 1. Compile the project using dotnet
dotnet build -c Release

echo "Cleaning up extra build files..."

# 2. Remove any generated .pdb files from the local Assemblies folder
if [ -f "$MOD_NAME/Assemblies/$MOD_NAME.pdb" ]; then
    rm "$MOD_NAME/Assemblies/$MOD_NAME.pdb"
    echo "Removed $MOD_NAME.pdb successfully."
fi

echo "Deploying to RimWorld Mods folder..."

# 3. Create the target folder if it doesn't exist yet
mkdir -p "$TARGET_DIR"

# 4. Sync files cleanly. On CachyOS, standard 'cp' with the update flag is incredibly fast,
# but we add trailing slashes to guarantee the exact contents sync without nested folder issues.
cp -rT "$MOD_NAME/" "$TARGET_DIR/"

echo -e "\nMod successfully deployed to: $TARGET_DIR"
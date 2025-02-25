#!/bin/bash

# Project variables
PROJECT_NAME="Faster-Resources"
SOURCE_FILE="Faster-Resources.cs"
OUTPUT_DLL="${PROJECT_NAME}.dll"
RIMWORLD_PATH="/mnt/3d363189-85bd-44c3-82d7-02a1c7a8ed1f/SteamLibrary/steamapps/common/RimWorld" # Replace with your RimWorld path

# Compile the class library
mcs -target:library -out:"${OUTPUT_DLL}" "${SOURCE_FILE}" \
    -reference:"${RIMWORLD_PATH}/RimWorldLinux_Data/Managed/Assembly-CSharp.dll" \
    -reference:"${RIMWORLD_PATH}/RimWorldLinux_Data/Managed/UnityEngine.dll" \
    -reference:"${RIMWORLD_PATH}/RimWorldLinux_Data/Managed/UnityEngine.CoreModule.dll"

# Move the DLL to my RimWorld mod's Assemblies folder
MOD_ASSEMBLIES_PATH="${RIMWORLD_PATH}/Mods/Faster-Resources/Assemblies"
mv "${OUTPUT_DLL}" "${MOD_ASSEMBLIES_PATH}"

echo "Compilation complete: ${OUTPUT_DLL}"

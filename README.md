# ProceduralGeneratingWorld
The main goal of the project is to build a procedurally generated rooms-based world AND NOTHING MORE at the moment. The project was started because of pure boredom.

**Responsibilities of the classes in the project:**

**MapGenerator** - Merger - Holds outputs and inputs from room & dungeon creating classes and merging them into one room. It prints out the array\[,] that holds generated rooms onto the world.

**RoomTemplateGenerator** - Rooms' base - Creates the base of the dungeon's rooms. The base consists of floor tiles randomly pulled from FloorTiles array\[] and walls randomly pulled from Walls array\[] that are built on the edges of the room's floor.

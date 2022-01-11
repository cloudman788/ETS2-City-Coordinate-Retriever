## ETS2 Log to Coordinates ##

**What is it?**
ETS2 Log to Coordinates (ETS2LTC) is a small project to both capture the location of ETS2's (and ATS's) cities and convert the information to easily readable JSON. Please bear in mind that this is not meant for a novice user, as it extensively uses hacks to work.

Please note that it uses some low-level keyboard emulation that has only been tested with a US international keyboard layout. If the key to open the console is not tilde (`), and underscore is not shift + dash, then chances are it will not work properly.

**How it works:**

 1. Extract the cities from ETS2. These can be found in def\city. For convenience, both ETS2's and ProMods/Rusmap's cities have been included. If you wish to use another map, extract them manually.
 2. Open `ets2-city-coordinate-retriever.exe.config` and edit the settings. The city directory should point to the folder with all the city files ({cityname}.sii), debugMode shows the current city instead of a loading bar and the SleepMultiplier adjusts the time the script takes. More on this later.
 3. Open Euro Truck Simulator 2, make sure you have access to the console (tilde) and bug form (F11) and drive. Leave it open (with the console **closed**), and then run `ets2-city-coordinate-retriever.exe`
 4. Press enter, return to the game and sit back. You will see the game hopping to all the cities you provided. If it malfunctions (due to the game not loading quick enough), please adjust the SleepMultiplier to a higher value.

Once that part is done, we move on to the next.

 1. Open Documents\Euro Truck Simulator 2\bugs.txt, look for the topmost city. Copy all city bug lines to a seperate text file (please do not leave any unattended newlines, as they will not be removed :-) )
 2. Open `ETS2 Log to Coordinates.exe.config`, edit it with the same city directory, the InputFile being the file you just created in step one and the outputfile to a file that will contain all JSON output.
 3. Run ETS2 Log to Coordinates.exe, it should automatically spout out some output (mainly cityname - realName and the JSON file content), and the JSON file should appear.

And there you have your JSON formatted city list with coordinates.

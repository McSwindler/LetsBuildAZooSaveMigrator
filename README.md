# Let's Build A Zoo Save File Migration
This is a program to migrate save files from the PC Gamepass version of Let's Build A Zoo into the Steam version of the game. A modified version of [ContainerReader](https://github.com/Tom60chat/ContainerReader) that allows for reading the Xbox containers.index files. This version takes it a step further and will read the contents of each save file, convert them to the necessary format, then save them in the expected friendly naming scheme.

## Disclaimers
I am not a C# developer, so please ignore my poorly written code.

Please be sure to backup both versions of save files before continuing. I can't help you if the originals get corrupted or destroyed.

## Usage
First, you must copy the `SEngine.dll` file from the [steam] game's install directory into the same directory as this program.

Then run:
```
ContainerReader path\to\containers.index path\to\save\files\
```
After completion, you should have a list of files named ZSV_*, copy all of them into the Steam save folder (`steamapps\common\Let's Build a Zoo\Save`) and the game should run fine with your existing save.

The containers.index file can be found in: `%LocalAppData%\Packages\NoMoreRobots.LetsBuildaZoo_671zbmwb2bw9p\SystemAppData\wgs`

I recommend making a backup of this entire folder and using the copy as the source for this app.

## Restrictions
Some types of containers.index files are not supported by this program yet. You will know if it is not supported - it will tell you. The most common type, 0xD, is fully supported.

Currently, the game will crash if you attempt to create a new zoo (dino or sandbox zoos). This is because the existing save file uses a different map size than is expected by the Steam version.

## Pull requests?
Feel free. If you feel you can improve this, do not hesitate to submit a pull request.

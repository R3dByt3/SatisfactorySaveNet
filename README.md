# SatisfactorySaveNet
I'm very happy to announce the first stable release of this component (v0.1.0).
 **EDIT:** Still consider using versions starting from v1.0.0 due to some bugfixes.

By this I consider this software as **stable** and **correct**!
You can integrate it using [nuget.org](https://www.nuget.org/packages/SatisfactorySaveNet/).

A fully managed C# save file reader (and soon writer) for the game [Satisfactory](https://www.satisfactorygame.com/) by Coffee Stain Studios.
This component currently allows reading all contained data of the *.sav file type in a typesafe and detailed manner .
**Also all kinds of readers are injectable and in case of need by that replaceable or wrappable!**

It is planned to add writing capabilities if this project finds interested users.

Further, (external and promising looking) documentation of the save game format is available [here](https://github.com/moritz-h/satisfactory-3d-map/blob/master/docs/SATISFACTORY_SAVE.md). **Link fixed now sorry**

## How to use
```CSharp
ISaveFileSerializer serializer = SaveFileSerializer.Instance;
var saveGame = serializer.Deserialize(@"C:\mySaveFile.sav");
```

## Injection example
```CSharp
ISaveFileSerializer Instance = new SaveFileSerializer(HeaderSerializer.Instance, ChunkSerializer.Instance, BodySerializer.Instance);
```
# SatisfactorySaveNet
I'm very happy to announce the first stable release of this component (v0.1.0).
By this I consider this software as **stable** and **correct**!
You can integrate it using [nuget.org](https://www.nuget.org/packages/SatisfactorySaveNet/).

A fully managed C# save file reader (and soon writer) for the game [Satisfactory](https://www.satisfactorygame.com/) by Coffee Stain Studios.
This component currently allows reading all contained data of the *.sav file type in a typesafe and detailed manner .
**Also all kinds of readers are injectable and in case of need by that replaceable or wrappable!**

It is planned to add writing capabilities if this project finds interested users.

Further, (external and promising looking) documentation of the save game format is available [here](docs/SATISFACTORY_SAVE.md).

## How to use
```CSharp
ISaveFileSerializer serializer = SaveFileSerializer.Instance;
var saveGame = serializer.Deserialize(@"C:\mySaveFile.sav");
```

## Injection example
```CSharp
ISaveFileSerializer Instance = new SaveFileSerializer(HeaderSerializer.Instance, ChunkSerializer.Instance, BodySerializer.Instance);
```

## License
> Copyright (C) 2021 - 2024 Marvin Gerdel
>
> This program is free software: you can redistribute it and/or modify
> it under the terms of the GNU General Public License as published by
> the Free Software Foundation, either version 3 of the License, or
> (at your option) any later version.
>
> This program is distributed in the hope that it will be useful,
> but WITHOUT ANY WARRANTY; without even the implied warranty of
> MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
> GNU General Public License for more details.
>
> You should have received a copy of the GNU General Public License
> along with this program.  If not, see <https://www.gnu.org/licenses/>.

The source code of SatisfactorySaveNet itself is licensed under the GNU GPLv3.
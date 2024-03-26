<!-- LICENSE -->
## How to use

```CSharp
ISaveFileSerializer serializer = SaveFileSerializer.Instance;
var saveGame = serializer.Deserialize(@"C:\mySaveFile.sav");
```
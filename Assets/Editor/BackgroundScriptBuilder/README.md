# Unity Background Script Builder
Automatically builds C# scripts when they change without having to alt+tab into Unity


This editor script listens for changes to .cs files in tthe provided folder and triggers
a script rebuild when they do. It is meant to slightly improve quality of life for developers,
as the build process starts as soon as the file is saved rather than when Unity gains focus.

#### Usage:
1) Download latest zip from releases

2) Place the script in `{your project}/Assets/Editor/`

3) In the Unity menu bar, go to `Window > Asset Management > Background Script Builder` and place
   the window that opens somewhere in your editor. Configure as you like, then feel free to close it.
   #### Note: You must open this window every time to open Unity to enable the script's functionality during that session

4) Change the `Script Folder` field to wherever you keep your scripts (relative to `Assets`).
   Note that the script watches all child folders of the specified folder as well.
   
   To watch the entire Assets folder, change it to `/`

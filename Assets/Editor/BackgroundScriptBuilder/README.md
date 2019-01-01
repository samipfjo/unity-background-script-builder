# Unity Background Script Builder
Automatically builds C# scripts when they change without having to alt+tab into Unity


This editor script listens for changes to .cs files in tthe provided folder and triggers
a script rebuild when they do. It is meant to slightly improve quality of life for developers,
as the build process starts as soon as the file is saved rather than when Unity gains focus.

#### Usage:
1) Download latest zip from releases

2) Place the script in `{your project}/Assets/Editor/`

3) In the Unity menu bar, go to `Window > Asset Management > Background Script Builder` and place
   the window that opens somewhere in your editor.

4) Change the `Script Folder` field to wherever you keep your scripts (relative to `Assets`).
   Note that the script watches all child folders of the specified folder as well.
   
   To watch the entire Assets folder, change it to `/`
   
5) Do whatever you like with the tab -- closing it won't stop the script from running
    
   #### Each time you start a new session (launch Unity), you'll need to open the tab again to get the script running. Your settings from last time are preserved.

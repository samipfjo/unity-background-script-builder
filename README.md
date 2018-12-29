# Unity Background Script Builder
Automatically builds C# scripts when they change without having to alt+tab into Unity


This editor script listens for changes to .cs files in tthe provided folder and triggers
a script rebuild when they do. It is meant to slightly improve quality of life for developers,
as the build process starts as soon as the file is saved rather than when Unity gains focus.

#### Usage:
1) Place the script in `{your project}/Assets/Editor/`

2) In the Unity menu bar, go to `Window > Asset Management > Background Script Builder` and place
   the window that opens somewhere in your editor.

   Due to the way that Unity handles GUI elements (which is usually a good thing), the window
   must exist somewhere in the editor for this to work (eg, it has to exist as a tab somewhere
   in your layout, but the tab doesn't need to be open).

3) Change the `Script Folder` field to wherever you keep your scripts (relative to `Assets`).
   Note that the script watches all child folders of the specified folder as well.
   To watch the entire Assets folder, change it to `/`

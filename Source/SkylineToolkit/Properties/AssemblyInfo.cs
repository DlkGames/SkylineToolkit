using System.Reflection;
using System.Runtime.InteropServices;

// Allgemeine Informationen über eine Assembly werden über die folgenden 
// Attribute gesteuert. Ändern Sie diese Attributwerte, um die Informationen zu ändern,
// die mit einer Assembly verknüpft sind.
[assembly: AssemblyTitle("SkylineToolkit")]
[assembly: AssemblyDescription("Toolkit for easier Cities: Skylines mod creation.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("DlkGames")]
[assembly: AssemblyProduct("SkylineToolkit")]
[assembly: AssemblyCopyright("Copyright © DarkLiKally 2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Durch Festlegen von ComVisible auf "false" werden die Typen in dieser Assembly unsichtbar 
// für COM-Komponenten.  Wenn Sie auf einen Typ in dieser Assembly von 
// COM zugreifen müssen, legen Sie das ComVisible-Attribut für diesen Typ auf "true" fest.
[assembly: ComVisible(false)]

// Die folgende GUID bestimmt die ID der Typbibliothek, wenn dieses Projekt für COM verfügbar gemacht wird
[assembly: Guid("695b2232-b829-4870-9fa3-4c41509bd125")]

// Versionsinformationen für eine Assembly bestehen aus den folgenden vier Werten:
//
//      Hauptversion
//      Nebenversion 
//      Buildnummer
//      Revision
//
// Sie können alle Werte angeben oder die standardmäßigen Build- und Revisionsnummern 
// übernehmen, indem Sie "*" eingeben:
// [assembly: AssemblyVersion("1.0.*")]
#if DEBUG
[assembly: AssemblyVersion("1.0.*")]
#else
// Fix for http://www.reddit.com/r/CitiesSkylinesModding/comments/2zeiwx/how_i_broke_everyones_savegame_and_fixed_them/
[assembly: AssemblyVersion("1.0.0.15")]
[assembly: AssemblyFileVersion("1.0.0.15")]
#endif
//[assembly: AssemblyFileVersion("1.0.*")]

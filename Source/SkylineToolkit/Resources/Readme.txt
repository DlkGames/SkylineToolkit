Place dll files in this directory and load it from code with

EmbeddedAssembly.Load("Example.Library.dll");

External mods can do the same using

EmbeddedAssembly.Load("Example.Library.dll", typeof(MyModClass).Assembly, "MyMod.Resources");
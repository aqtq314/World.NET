📢 Check out [TorchScriptWORLD](https://github.com/aqtq314/TorchScriptWORLD) — a PyTorch reimplementation of the WORLD vocoder optimized for parallel GPU computation, with TorchScript checkpoint export supported so that the model can be used in any TorchScript runtime (e.g., C++, .NET, Rust, Java, etc.).

# World.NET - A C# wrapper for WORLD vocoder

The binary distributions provided at [release](../../releases) page targets **.NET Framework 4.7**, although the source code can be recompiled for lower framework versions with an explicit reference to System.ValueTuple assembly.

Please refer to [the original repo in C](//github.com/mmorise/World) by [mmorise](//github.com/mmorise) for a detailed description of the WORLD vocoder.

Please refer to file [Tests/Program.cs](Tests/Program.cs) for example usage of this library.


``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i3-8350K CPU 4.00GHz (Coffee Lake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
|         Method |      Mean |     Error |    StdDev |
|--------------- |----------:|----------:|----------:|
|         OpenTk | 10.984 ns | 0.1087 ns | 0.1017 ns |
| SystemNumerics |  3.895 ns | 0.0185 ns | 0.0164 ns |
|      MathSharp |  1.622 ns | 0.0109 ns | 0.0102 ns |

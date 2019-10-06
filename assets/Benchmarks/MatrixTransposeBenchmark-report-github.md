``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i3-8350K CPU 4.00GHz (Coffee Lake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host]     : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT
  DefaultJob : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT


```
|         Method |      Mean |     Error |    StdDev |
|--------------- |----------:|----------:|----------:|
|      MathSharp |  1.070 ns | 0.0122 ns | 0.0114 ns |
| SystemNumerics |  3.744 ns | 0.0153 ns | 0.0143 ns |
|         OpenTk | 15.875 ns | 0.3438 ns | 0.5146 ns |

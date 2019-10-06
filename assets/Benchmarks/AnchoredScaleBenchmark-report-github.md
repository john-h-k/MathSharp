``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i3-8350K CPU 4.00GHz (Coffee Lake), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100
  [Host] : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT
  Core   : .NET Core 3.0.0 (CoreCLR 4.700.19.46205, CoreFX 4.700.19.46214), 64bit RyuJIT

Job=Core  Runtime=Core  

```
|         Method |       Mean |     Error |    StdDev | Rank |
|--------------- |-----------:|----------:|----------:|-----:|
|      MathSharp |  0.8542 ns | 0.0084 ns | 0.0079 ns |    1 |
| SystemNumerics |  2.0281 ns | 0.0123 ns | 0.0115 ns |    2 |
|         OpenTk | 37.4250 ns | 0.1585 ns | 0.1483 ns |    3 |

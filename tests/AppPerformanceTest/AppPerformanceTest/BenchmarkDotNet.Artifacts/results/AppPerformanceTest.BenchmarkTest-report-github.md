``` ini

BenchmarkDotNet=v0.11.5, OS=Windows 10.0.18362
Intel Core i7-8650U CPU 1.90GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=2.2.202
  [Host] : .NET Core 2.2.3 (CoreCLR 4.6.27414.05, CoreFX 4.6.27414.05), 64bit RyuJIT
  Core   : .NET Core 2.2.3 (CoreCLR 4.6.27414.05, CoreFX 4.6.27414.05), 64bit RyuJIT

Job=Core  Runtime=Core  

```
| Method |     Mean |     Error |    StdDev |   Median | Rank |  Gen 0 |  Gen 1 |  Gen 2 | Allocated |
|------- |---------:|----------:|----------:|---------:|-----:|-------:|-------:|-------:|----------:|
|    Get | 179.6 us | 19.263 us | 54.959 us | 167.0 us |    2 | 1.4648 | 0.4883 |      - |   4.19 KB |
|    Add | 232.5 us | 23.249 us | 66.705 us | 214.3 us |    3 | 3.4180 | 1.2207 | 0.2441 |   4.31 KB |
| Update | 115.4 us |  2.285 us |  4.819 us | 115.3 us |    1 | 2.4414 | 0.9766 |      - |   4.18 KB |
| Delete | 220.5 us | 14.710 us | 41.490 us | 213.3 us |    3 | 3.1738 | 0.9766 |      - |   4.31 KB |

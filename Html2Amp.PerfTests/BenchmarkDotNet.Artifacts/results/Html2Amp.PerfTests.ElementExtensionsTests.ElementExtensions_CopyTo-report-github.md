``` ini

BenchmarkDotNet=v0.10.12, OS=Windows 10 Redstone 3 [1709, Fall Creators Update] (10.0.16299.192)
Intel Core i5-6600K CPU 3.50GHz (Skylake), 1 CPU, 4 logical cores and 4 physical cores
Frequency=3421874 Hz, Resolution=292.2375 ns, Timer=TSC
.NET Core SDK=2.1.4
  [Host]     : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT
  DefaultJob : .NET Core 2.0.5 (Framework 4.6.26020.03), 64bit RyuJIT


```
|       Method |     Mean |     Error |    StdDev |
|------------- |---------:|----------:|----------:|
| CopyToMethod | 5.724 us | 0.0891 us | 0.0833 us |

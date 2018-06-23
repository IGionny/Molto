2018-06-22
----------

    ORM |             Type |   Method | Return |     Mean |    Error |    StdDev | Rank |  Gen 0 | Allocated |
------- |----------------- |--------- |------- |---------:|---------:|----------:|-----:|-------:|----------:|
 Dapper | DapperBenchmarks | Query<T> |   Post | 69.85 us | 2.325 us | 0.6040 us |    1 | 3.2813 |  13.68 KB |
  Molto |  MoltoBenchmarks | Query<T> |   Post | 90.11 us | 4.300 us | 1.1170 us |    2 | 3.7500 |  15.42 KB |

2018-06-23
----------
  
    ORM |             Type |   Method | Return |     Mean |     Error |    StdDev | Rank |  Gen 0 | Allocated |
------- |----------------- |--------- |------- |---------:|----------:|----------:|-----:|-------:|----------:|
 Dapper | DapperBenchmarks | Query<T> |   Post | 70.23 us | 2.3429 us | 0.6085 us |    1 | 3.2813 |  13.68 KB |
  Molto |  MoltoBenchmarks | Query<T> |   Post | 74.72 us | 0.2270 us | 0.0590 us |    2 | 3.4375 |  14.39 KB |
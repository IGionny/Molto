Molto
=====

Molto is a new Micro-ORM for .Net Core inspired by the beautiful syntax of NPoco and the speed of Dapper.

Tecnology goals
---------------

1. .NET Core only
1. Fully tested and testable
1. Extensible by plugins and proper implementation
1. Comprensible design
1. Simple 
1. Fast

Current tasks
--------------

- [ ] Define design / api
- [ ] Implement basic CRUD API
	- [ ] Create (Insert)
	    - [x] Insert all fields
		- [ ] Insert and return Database Identity
	- [ ] Read 
		- [x] Query IList<T> - Db.Query<Test>()
		- [x] Paged result - Db.PageAsync<Test>(1, 5, "ORDER BY Id")
		- [X] Single result - Db.Single<Test>("WHERE id = @0", 1)
		- [x] Single field (multiple rows) - Db.Query<string>("SELECT name from TEST")
		- [x] Count
	- [x] Update
	- [x] Delete
- [ ] Execute commands
	- [x] Execute commands with arguments
	- [ ] Execute StoreProcedures
- [ ] Multi database support  (Dialect and Mapping)
	- [x] MsSql
	- [x] SqlLite
	- [x] Postgresql
	- [ ] MySql (?)
- [ ] Mapping Abilities
	- [ ] field name
	- [ ] table name
	- [ ] readonly 
	- [ ] ignore
	- [ ] insert only (v2?)
	- [ ] update only (v2?)
- [ ] Mapping Strategies
	- [ ] Assembly scanner
	- [ ] Mapping By Attribute
	- [ ] Mapping By Convention
	- [ ] Mapping via Fluent
- [ ] Null-Value support
- [ ] Private fields
- [ ] Composite Primary Key
- [ ] Make Async calls
- [ ] Cache mapping and sub-elements
- [ ] Custom converter per property
- [ ] Custom entity constructor
- [ ] Commons to simplify tests
- [ ] Full Domain for tests
- [ ] Implement transactions
- [ ] Timeout on command
- [ ] Implement a DefaultFactory to simplify startup
- [x] Banchmark with BenchmarkDotNet
- [ ] Log via MS interface ILogger

Next:
- [ ] Dynamic support
- [ ] Named parameters


Benchmark results history
==========================

Note: the Benchmarks are 'freely' inspired by the Dapper's ones.

2018-06-22 - Query/MsSql only
-----------------------------

    ORM |             Type |   Method | Return |     Mean |    Error |    StdDev | Rank |  Gen 0 | Allocated |
------- |----------------- |--------- |------- |---------:|---------:|----------:|-----:|-------:|----------:|
 Dapper | DapperBenchmarks | Query<T> |   Post | 69.85 us | 2.325 us | 0.6040 us |    1 | 3.2813 |  13.68 KB |
  Molto |  MoltoBenchmarks | Query<T> |   Post | 90.11 us | 4.300 us | 1.1170 us |    2 | 3.7500 |  15.42 KB |

2018-06-23 - Query/MsSql only
-----------------------------
  
    ORM |             Type |   Method | Return |     Mean |     Error |    StdDev | Rank |  Gen 0 | Allocated |
------- |----------------- |--------- |------- |---------:|----------:|----------:|-----:|-------:|----------:|
 Dapper | DapperBenchmarks | Query<T> |   Post | 70.23 us | 2.3429 us | 0.6085 us |    1 | 3.2813 |  13.68 KB |
  Molto |  MoltoBenchmarks | Query<T> |   Post | 74.72 us | 0.2270 us | 0.0590 us |    2 | 3.4375 |  14.39 KB |
  
2018-06-29 - Query/MsSql only
-----------------------------

    ORM |             Type |   Method | Return |     Mean |     Error |    StdDev | Rank |  Gen 0 | Allocated |
------- |----------------- |--------- |------- |---------:|----------:|----------:|-----:|-------:|----------:|
 Dapper | DapperBenchmarks | Query<T> |   Post | 71.90 us | 3.8657 us | 1.0041 us |    1 | 3.2813 |  13.68 KB |
  Molto |  MoltoBenchmarks | Query<T> |   Post | 76.01 us | 0.2326 us | 0.0604 us |    2 | 3.4375 |  14.39 KB |


Fun fact
--------

"Molto" is an italian word to say "much" and "Poco" (Aka Plain Old C Object) in italian means "Scant"



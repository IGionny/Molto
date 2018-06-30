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


Fun fact
--------

"Molto" is an italian word to say "much" and "Poco" (Aka Plain Old C Object) in italian means "Scant"



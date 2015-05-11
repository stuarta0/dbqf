# dbqf
Database Query Framework

## What is it?

A library aimed at **simplifying complex data mining for the end user** in .NET applications while minimising developer time to implement.  The intent is to provide a way of mapping a complete relational data structure and allowing the UI to dynamically construct itself for various purposes.  Construction of the UI is modular with use of factories, adapters and (in the example Standalone project) dependency injection.  The code is database engine independent allowing you to create whatever SQL is required to suit your engine (currently tested: `MSSQL`, `SQLite`).  Note this is **not an ORM** but could conceivably have an adapter to an ORM's mapping.

This project follows **Test Driven Development** (TDD) and contains NUnit test cases for the core library.  It is also based on **Fluent** coding style allowing quick code-based setup.

### Why would I want to use it?
- Automated UI generation for boring, repetitive search fields, operators and execution.
- UI controls generated from factories with out-of-the-box behaviour based on field types (but completely customisable).
- Hierarchical data fetching to organise a tree of related data in any which way *(undergoing rewrite)*.
- Modular code allowing replacement of any functionality: what fields to display, what controls to create, what operators are available, how SQL statements are generated, etcetera.
- For WinForms: 
-- Preset Control lists fields with name and a control with a common operator (`between` for numbers and dates, `contains` for string; customisable).
-- Simple Control allows selection of field and operator with a control created dependant on both.
-- Advanced Control allowing full drill-down of related fields, operators, controls dependant on path and hierarchical combinations (AND, OR) of all parameters.

## Documentation

For full documentation, see ...

## License

This project has an MIT license to allow the most freedom of use.  That said, myself and other users of the library would greatly appreciate any contributions you can make.
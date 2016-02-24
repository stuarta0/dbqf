![dbqf](https://raw.githubusercontent.com/stuarta0/dbqf/master/resources/dbqf.png)

Database Query Framework

## TL;DR

dbqf is a set of libraries to help you make searching, reporting and data mining easy for users in .NET applications.  Oh and it's easy for you too.

## Requirements

- Visual Studio Community 2015 or greater
- .NET 2.0+ for core libraries, .NET 4.5 for the standalone application

## The long version

dbqf is a library primarily aimed at **simplifying presentation of complex data mining for the end user** in .NET applications.  It's a full stack solution that gives you the boring SQL building components right up to the UI controls to dump into your application.

The project comes with the building blocks for you to construct whatever you need with tight integration into your application.  In addition it has a standalone application that you can use side-by-side with your application with a single XML project file of your making.

Construction of the UI is modular with use of factories, adapters and (in the example Standalone project) dependency injection.  The code is database engine agnostic allowing you to create whatever SQL is required to suit your engine (currently tested: `MSSQL`, `SQLite`).  Note this is **not an ORM** but could conceivably have an adapter to an ORM's mapping.

The library employs many patterns from Gang of Four and the UI is abstracted using a variety of the **Presentation Model** (and subsequently **MVVM**).  By doing this, the core behaviour can be reused across many UI toolkits with two provided out-of-the-box: WinForms and WPF.  This project has **unit tests** for the core libraries, but less so for the UI.  It also employs **fluent** patterns throughout the code to make things easier to work with.

No external libraries are needed for the core and WinForms libraries.  The Standalone applications do require a number of external libraries but can be updated with automatic package restore.

### Why would I want to use it?
- Automated UI generation for boring, repetitive search fields, operators and wiring.
- UI controls generated from factories with out-of-the-box behaviour based on field types (but completely customisable).
- Modular code allowing modification or replacement of any functionality: 
  - what fields to display, 
  - what controls to create, 
  - what operators are available, and
  - how SQL statements are generated.
- For both WinForms and WPF:
  - Preset Search Control lists fields with name and a control with a common operator (`between` for numbers and dates, `contains` for string; customisable).
  - Simple Search Control allows selection of field and operator with a control created dependant on both.
  - Advanced Search Control allowing full drill-down of related fields, operators, controls dependant on path and hierarchical combinations (AND, OR) of all parameters.
  - and a stack of individual components that makes the above possible.


## Examples!

### Standalone Application
Provided standalone application in .NET 4.5 WinForms that uses inversion of control, loads XML projects, has custom parsing and threaded behaviour.

![Example-Standalone](https://raw.githubusercontent.com/stuarta0/dbqf/master/docs/example-loading.png)

### Preset Search Control 
Fully customisable list of field / value pairs.

![Example-Preset](https://raw.githubusercontent.com/stuarta0/dbqf/master/docs/example-preset.png)

### Standard Search Control
Fully customisable list of field / operator / value combinations.

![Example-Standard](https://raw.githubusercontent.com/stuarta0/dbqf/master/docs/example-standard.png)

### Advanced Search Control
Complete control over parameters with AND/OR from any field or path from within the database as well as selection of operator / value.

![Example-Advanced](https://raw.githubusercontent.com/stuarta0/dbqf/master/docs/example-advanced.png)

### Custom Output Fields Control
Choice of fields to retrieve using drag-drop from hierarchical representation of data structure.

![Example-Output](https://raw.githubusercontent.com/stuarta0/dbqf/master/docs/example-output.png)

### Generated SQL
Example of generated parameterised SQL (in psuedo form) created from core components in the library.

![Example-Output](https://raw.githubusercontent.com/stuarta0/dbqf/master/docs/example-sql.png)

### Detailed Component Explanation (Preset Search Control)

![Preset](https://raw.githubusercontent.com/stuarta0/dbqf/master/docs/preset.png)

## Documentation

Full documentation TBA.  Code has XML documentation, test cases show usage, and Standalone project shows real-world use.

### Sample Code

Using the core, Sql and WinForms libraries, here's sample usage in a WinForms application:

```c#
  var config = new dbqf.Sql.Configuration.MatrixConfiguration()
      .Subject(new dbqf.Configuration.Subject("Test")
          .Sql("SELECT * FROM [Test]")
          .FieldId(new dbqf.Configuration.Field("Id", typeof(int)))
          .FieldDefault(new dbqf.Configuration.Field("Name", typeof(string)))
          .Field(new dbqf.Configuration.Field("Total", typeof(int)))
          .Field(new dbqf.Configuration.Field("Date Created", typeof(DateTime))));

  var preset = new dbqf.WinForms.PresetView(new dbqf.Display.Preset.PresetAdapter<Control>(
      new dbqf.WinForms.UIElements.WinFormsControlFactory(), 
      new dbqf.Display.ParameterBuilderFactory()));

  preset.Adapter.SetParts(new dbqf.Display.FieldPathFactory().GetFields(config[0]));
  preset.Dock = DockStyle.Fill;
  this.Controls.Add(preset);
```

We create a configuration fluently, which contains one subject called Test with Sql that selects from a table named Test and has four fields; an id, name, total and date created.

Once we have our configuration set up, we want to display it.  We'll use the `PresetView` in the WinForms library which requires a `PresetAdapter<Control>`.  The generic type of the PresetAdapter (in this case `Control`) tells us that we need to provide a ControlFactory that dynamically creates UI elements of type `Control`.  This should make sense since we're writing a WinForms application, so we want WinForms controls.  In the example we use the built in `WinFormsControlFactory` class to do this for us.

Next we need to provide a `ParameterBuilderFactory`.  The purpose of this class is to provide defaults for operators used for each field.  What we mean by this is if you have a field `Name` (like in the sample code above) that is a `typeof(string)` then the most common operator an end user would want is `Name CONTAINS "foo"`.  This is what the built in `ParameterBuilderFactory` does - it looks at a field's type and determines the most common operator to use.

Once we've got our `PresetView` instantiated, we initialise it by calling `SetParts`.  We pass it a list of fields that we want to show in the `PresetView`.  If we use the built in `FieldPathFactory` it'll get us all fields for a subject excluding the Id field.

Then we add the `PresetView` to our `Form` and the user will see the fields.

Note: in terms of execution the flow will actually be:
- SetParts(list of fields):
- For each field, ask the `ParameterBuilderFactory` what operator should be used (numbers/dates will be BETWEEN, strings will be CONTAINS)
- For each field/operator, ask the `WinFormsControlFactory` for a `Control` to represent the combination (e.g. field Name + CONTAINS = TextBox, field Total + BETWEEN = a control with two TextBoxes for lower and upper limit)
- Add a label and the given control to the view


## License

This project has an MIT license to allow the most freedom of use.  That said, myself and other users of the library would greatly appreciate any contributions you can make.
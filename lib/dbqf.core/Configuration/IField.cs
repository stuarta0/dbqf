using System;
namespace dbqf.Configuration
{
    public interface IField
    {
        ISubject Subject { get; set; }
        string SourceName { get; set; }
        Type DataType { get; set; }

        string DisplayName { get; set; }
        string DisplayFormat { get; set; }
        IFieldList List { get; set; }
    }
}

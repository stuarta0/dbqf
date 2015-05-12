using System;
namespace dbqf.Configuration
{
    public interface IRelationField : IField
    {
        ISubject RelatedSubject { get; set; }
    }
}

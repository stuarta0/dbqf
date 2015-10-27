using System;
using System.Collections.Generic;
namespace dbqf.Configuration
{
    public interface ISubject : IList<IField>
    {
        IConfiguration Configuration { get; set; }
        string DisplayName { get; set; }
        IField IdField { get; set; }
        IField DefaultField { get; set; }
        IField this[string sourceName] { get; }
    }
}

using dbqf.Configuration;
using System;
namespace dbqf.Commands
{
    public interface ICommand
    {
        // TODO: Relocate these properties
        string Description { get; set; }
        string ImageKey { get; set; }
        bool RequirePlaceholders { get; set; }

        bool CanExecute(ISubject subject, object id);
        void Execute(ISubject subject, object id);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace dbqf.Commands
{
    /// <summary>
    /// Represents an action to perform when a Subject is chosen in a user interface.
    /// </summary>
    [Serializable]
    public abstract class SubjectCommand : ICommand
    {
        /// <summary>
        /// Gets or sets the description of what this action will perform.
        /// </summary>
        [XmlElement]
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets whether this action is only valid if all placeholders are a non-null value.
        /// </summary>
        [XmlAttribute]
        public virtual bool RequirePlaceholders { get; set; }

        /// <summary>
        /// Gets or sets the image key to index an image when using dbqf.IO.QuerySetup.Images.
        /// </summary>
        [XmlElement]
        public virtual string ImageKey { get; set; }

        /// <summary>
        /// Perform the current action.
        /// </summary>
        /// <param name="queryer"></param>
        /// <param name="subject"></param>
        /// <param name="id"></param>
        public abstract void Execute(ISubject subject, object id);

        /// <summary>
        /// Using the value of RequirePlaceholders, validate that this action can be executed.
        /// </summary>
        /// <param name="queryer"></param>
        /// <param name="subject"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool CanExecute(ISubject subject, object id);
    }
}

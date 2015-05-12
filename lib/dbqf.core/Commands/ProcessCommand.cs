using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using System.Xml.Serialization;

namespace dbqf.Commands
{
    /// <summary>
    /// Enables the execution of a process.
    /// </summary>
    [Serializable]
    public class ProcessCommand : SubjectCommand 
    {
        /// <summary>
        /// Gets or sets the file path and name to start as a process.  This can include placeholders from fields in the Subject source SQL and also environment variables.
        /// </summary>
        [XmlElement]
        public string Filename { get; set; }

        /// <summary>
        /// Gets or sets the arguments to be used when opening the Filename.  This can include placeholders from fields in the Subject source SQL and also environment variables.
        /// </summary>
        [XmlElement]
        public string Arguments { get; set; }

        /// <summary>
        /// Executes the process based on the given object id.
        /// </summary>
        /// <param name="queryer"></param>
        /// <param name="subject"></param>
        /// <param name="id"></param>
        public override void Execute(ISubject subject, object id)
        {
            // run both the filename and arguments through the placeholder replacement
            //TODO
            string filename = null; // queryer.ReplacePlaceholders(subject, id, Filename);
            string args = null; // queryer.ReplacePlaceholders(subject, id, Arguments);

            // also expand any environment variables
            filename = Environment.ExpandEnvironmentVariables(filename);

            if (!String.IsNullOrEmpty(args))
                args = Environment.ExpandEnvironmentVariables(args);

            // now shell out the process with these values
            System.Diagnostics.Process.Start(filename, args);
        }

        /// <summary>
        /// Checks whether the placeholders can be populated and whether the resulting file or directory exists.
        /// </summary>
        /// <param name="queryer"></param>
        /// <param name="subject"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool CanExecute(ISubject subject, object id)
        {
            // run both the filename and arguments through the placeholder replacement
            //TODO
            bool placeholdersOk = false; // (queryer.HasAllPlaceholderValues(subject, id, Filename) && queryer.HasAllPlaceholderValues(subject, id, Arguments));

            // if placeholders all check out, ensure process can start
            if (placeholdersOk)
            {
                //TODO
                string filename = null; // Environment.ExpandEnvironmentVariables(queryer.ReplacePlaceholders(subject, id, Filename));
                return System.IO.File.Exists(filename) || System.IO.Directory.Exists(filename);
            }

            return placeholdersOk;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using dbqf.Configuration;
using System.Xml.Serialization;

namespace dbqf.Commands
{
    /// <summary>
    /// Enables calling a protocol handler.
    /// </summary>
    [Serializable]
    public class UriCommand : SubjectCommand 
    {
        /// <summary>
        /// Gets or sets the URI address that will be navigated.
        /// </summary>
        [XmlElement]
        public string Address { get; set; }
        
        /// <summary>
        /// Starts a process with the Address based on the given object id.
        /// </summary>
        /// <param name="queryer"></param>
        /// <param name="subject"></param>
        /// <param name="id"></param>
        public override void Execute(ISubject subject, object id)
        {
            // run the address through the placeholder replacement
            //TODO
            string address = null; // queryer.ReplacePlaceholders(subject, id, Address);

            // now shell out the protocol handler with the address
            System.Diagnostics.Process.Start(address);
        }

        /// <summary>
        /// Checks whether the address is able to have it's placeholders populated.
        /// </summary>
        /// <param name="queryer"></param>
        /// <param name="subject"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool CanExecute(ISubject subject, object id)
        {
            // run the address through the placeholder replacement
            //TODO
            return false; // queryer.HasAllPlaceholderValues(subject, id, Address);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace EAAddins.SystemArchitecture.ReferenceTools
{
    /// <summary>
    /// Data model class to store GUID of original and reference property element. 
    /// </summary>
    public class ReferenceContext
    {
        public string ReferenceGUID = "";
        public string OriginalGUID = "";

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ReferenceContext()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="referenceGUID">GUID of reference property element.</param>
        /// <param name="originalGUID">GUID of original property element.</param>
        public ReferenceContext(string referenceGUID, string originalGUID)
        {
            this.ReferenceGUID = referenceGUID;
            this.OriginalGUID = originalGUID;
        }
    }
}

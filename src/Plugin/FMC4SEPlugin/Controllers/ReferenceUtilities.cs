using System;
using System.Collections.Generic;
using System.Text;
using log4net;

namespace EAAddins.SystemArchitecture.ReferenceTools
{
    public class ReferenceUtilities
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(ReferenceUtilities));

        private EA.Repository Rep;
        
        /// <summary>
        /// key: originalGUID, value: List of referenceContext objects for the original
        /// </summary>
        private Dictionary<string, List<ReferenceContext>> ReferenceContextList = new Dictionary<string, List<ReferenceContext>>();

        /// <summary>
        /// Constructor
        /// </summary>
        public ReferenceUtilities(EA.Repository rep)
        {
            this.Rep = rep;
        }

        /// <summary>
        /// This method checks if a given element is a reference or not.
        /// </summary>
        /// <param name="el">The element to check.</param>
        /// <param name="isUniqueReference">Returen true if the refwerence is unique.</param>
        /// <param name="originalEl">The connected original element.</param>
        /// <returns>True if the element is a reference, false otherwise.</returns>
        public bool IsReference(EA.Element el, out bool isUniqueReference, out EA.Element originalEl)
        {
            List<EA.Element> references = new List<EA.Element>();
            bool isReference = false;

            for (short i = 0; i < el.Connectors.Count; i++)
            {
                EA.Connector con = el.Connectors.GetAt(i) as EA.Connector;
                if (con.Type == "Dependency" && con.Stereotype == "referenceOf" && con.ClientID == el.ElementID)
                {
                    EA.Element refEl = Rep.GetElementByID(con.SupplierID) as EA.Element;
                    bool addElement = true;

                    //if (this.CopyFeatureDriven)
                    //{
                    //    bool constraintResult = this.CheckConnectorConstraint(con);
                    //    addElement &= constraintResult;
                    //}

                    if (addElement)
                    {
                        references.Add(refEl);
                    }
                }
            }

            if (references.Count == 0)
            {
                isUniqueReference = true;
                originalEl = null;
                isReference =  false;
            }
            else if (references.Count == 1)
            {
                isUniqueReference = true;
                originalEl = references[0];
                isReference =  true;
            }
            else
            {
                isUniqueReference = false;
                originalEl = null;
                isReference =  true;
            }
            /*el.ElementGUID.ToString();
            log.Info("ElementGUID: " + el.ElementGUID.ToString());
            log.Info("isUniqueReference: " + isUniqueReference.ToString());
            log.Info("isReference: " + isReference.ToString());
            log.Info("references.Count: " + references.Count.ToString());*/
            return isReference;
        }


        /// <summary>
        /// This method checks if a given element is a reference or not.
        /// </summary>
        /// <param name="el">The element to check.</param>
        /// <param name="isUniqueReference">Returen true if the refwerence is unique.</param>
        /// <param name="originalEl">The connected original element.</param>
        /// <returns>True if the element is a reference, false otherwise.</returns>
        public bool IsReferenceMulti(EA.Element el, out bool isUniqueReference, out EA.Element originalEl, out int amount)
        {
            List<EA.Element> references = new List<EA.Element>();
            bool isReference = false;

            for (short i = 0; i < el.Connectors.Count; i++)
            {
                EA.Connector con = el.Connectors.GetAt(i) as EA.Connector;
                if (con.Type == "Dependency" && con.Stereotype == "referenceOf" && con.ClientID == el.ElementID)
                {
                    EA.Element refEl = Rep.GetElementByID(con.SupplierID) as EA.Element;
                    bool addElement = true;

                    //if (this.CopyFeatureDriven)
                    //{
                    //    bool constraintResult = this.CheckConnectorConstraint(con);
                    //    addElement &= constraintResult;
                    //}

                    if (addElement)
                    {
                        references.Add(refEl);
                    }
                }
            }

            if (references.Count == 0)
            {
                isUniqueReference = true;
                originalEl = null;
                isReference = false;
            }
            else if (references.Count == 1)
            {
                isUniqueReference = true;
                originalEl = references[0];
                isReference = true;
            }
            else
            {
                isUniqueReference = false;
                originalEl = references[0];
                isReference = true;
            }
           
            /* el.ElementGUID.ToString();
            log.Info("ElementGUID: " + el.ElementGUID.ToString());
            log.Info("isUniqueReference: " + isUniqueReference.ToString());
            log.Info("isReference: " + isReference.ToString());
            log.Info("references.Count: " + references.Count.ToString());
            */

            amount = references.Count;
            return isReference;
        }


        public int GetReferenceCount(string referenceGUID, string originalGUID)
        {
            //if (originalGUID == "{329E92C0-3324-40ff-A209-0033F76B4F0B}")
            //{
            //    ; // used for setting a break point
            //}

            if (this.ReferenceContextList.ContainsKey(originalGUID))
            {
                List<ReferenceContext> refContexts = this.ReferenceContextList[originalGUID];

                for (int cnt = 0; cnt < refContexts.Count; cnt++)
                {
                    ReferenceContext rc = refContexts[cnt];
                    if (rc.ReferenceGUID == referenceGUID)
                    {
                        return cnt + 1;
                    }
                }
                refContexts.Add(new ReferenceContext(referenceGUID, originalGUID));
                return refContexts.Count;
            }
            else
            {
                List<ReferenceContext> rcList = new List<ReferenceContext>();
                rcList.Add(new ReferenceContext(referenceGUID, originalGUID));

                this.ReferenceContextList.Add(originalGUID, rcList);
                return 1;
            }
        }

        
    }
}

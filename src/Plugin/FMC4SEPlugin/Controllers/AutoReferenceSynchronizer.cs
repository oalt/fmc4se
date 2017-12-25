using System;
using System.Collections.Generic;
using System.Text;
using EAAddins.SystemArchitecture.Helpers;
using log4net;

namespace EAAddins.SystemArchitecture.ReferenceTools
{
    /// <summary>
    /// This class is responsible for automatic reference element synchronization.
    /// 
    /// $Id: SystemArchitecture/ReferenceTools/AutoReferenceSynchronizer.cs 1.2 2011/10/11 15:27:58CEST AltO in_process  $
    /// </summary>
    public class AutoReferenceSynchronizer
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AutoReferenceSynchronizer));

        private EA.Repository Rep;
        private string StartElementGuid = "";

        private Dictionary<string, string> SynchronizedReferences = new Dictionary<string, string>();

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="rep">The EA repository.</param>
        /// <param name="startElementGuid">The GUID of the top most element in the system decomposition.</param>
        public AutoReferenceSynchronizer(EA.Repository rep, string startElementGuid)
        {
            this.Rep = rep;
            this.StartElementGuid = startElementGuid;
        }

        /// <summary>
        /// Call this method to start the reference synchronization.
        /// </summary>
        public void StartReferenceSynchronization()
        {
            try
            {
                DecompositionTreeWalker dtw = new DecompositionTreeWalker(this.Rep, this.Rep.GetElementByGuid(this.StartElementGuid));

                dtw.ReferenceWalked += new DecompositionTreeWalker.ElementWalked(dtw_ReferenceWalked);

                dtw.StartWalking();
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// Event handler. Called when a reference is walked by the DecompositionTreeWalker.
        /// </summary>
        /// <param name="el">The EA Element.</param>
        void dtw_ReferenceWalked(EA.Element el)
        {
            string guid = el.ElementGUID;
            if (!this.SynchronizedReferences.ContainsKey(guid))
            {
                this.SynchronizedReferences.Add(guid, guid);
                if (!el.Locked)
                {
                    PortSynchronizer psynch = new PortSynchronizer(this.Rep, el);
                    psynch.SynchronizeFeatureConstraints = true;
                    psynch.SynchronizeReferences();
                }
                else
                {
                    log.Info("Skipping locked element...");
                }
            }
            else
            {
                log.Info("Reference was still synchronized.");
            }
        }
    }
}

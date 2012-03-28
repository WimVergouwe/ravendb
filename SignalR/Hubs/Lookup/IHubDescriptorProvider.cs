﻿using System.Collections.Generic;
using SignalR.Hubs.Lookup.Descriptors;

namespace SignalR.Hubs.Lookup
{
    /// <summary>
    /// Describes hub descriptor provider, which provides information about available hubs.
    /// </summary>
    public interface IHubDescriptorProvider
    {
        /// <summary>
        /// Retrieve all avaiable hubs.
        /// </summary>
        /// <returns>Collection of hub descriptors.</returns>
        IEnumerable<HubDescriptor> GetHubs();

        /// <summary>
        /// Tries to retrieve hub with a given name.
        /// </summary>
        /// <param name="hubName">Name of the hub.</param>
        /// <param name="descriptor">Retrieved descriptor object.</param>
        /// <returns>True, if action has been found</returns>
        bool TryGetHub(string hubName, out HubDescriptor descriptor);
    }
}
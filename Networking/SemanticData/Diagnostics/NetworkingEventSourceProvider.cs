﻿//____________________________________________________________________________
//
//  Copyright (C) 2019, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//____________________________________________________________________________

using System.ComponentModel.Composition;
using System.Diagnostics.Tracing;
using UAOOI.Networking.Core;

namespace UAOOI.Networking.SemanticData.Diagnostics
{
  /// <summary>
  /// Class NetworkingEventSourceProvider - gets access to an instance of <see cref="EventSource"/> to be registered by the logging infrastructure.
  /// </summary>
  /// <seealso cref="UAOOI.Networking.SemanticData.Diagnostics.INetworkingEventSourceProvider" />
  [Export(typeof(INetworkingEventSourceProvider))]
  public class NetworkingEventSourceProvider : INetworkingEventSourceProvider
  {
    /// <summary>
    /// Gets the part event source.
    /// </summary>
    /// <returns>Returns an instance of <see cref="EventSource" />.</returns>
    public EventSource GetPartEventSource()
    {
      return ReactiveNetworkingEventSource.Log;
    }
  }
}

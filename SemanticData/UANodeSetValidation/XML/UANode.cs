﻿//___________________________________________________________________________________
//
//  Copyright (C) 2019, Mariusz Postol LODZ POLAND.
//
//  To be in touch join the community at GITTER: https://gitter.im/mpostol/OPC-UA-OOI
//___________________________________________________________________________________

using System;

namespace UAOOI.SemanticData.UANodeSetValidation.XML
{
  /// <summary>
  /// Class UANode.
  /// Implements the <see cref="IEquatable{UANode}"/>
  /// </summary>
  /// <seealso cref="IEquatable{UANode}" />
  public abstract partial class UANode : IEquatable<UANode>
  {
    /// <summary>
    /// Indicates whether the current object is equal to another object of the same type.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns>true if the current object is equal to the <paramref name="other">other</paramref> parameter; otherwise, false.</returns>
    public virtual bool Equals(UANode other)
    {
      if (object.ReferenceEquals(other, null))
        return false;
      if (Object.ReferenceEquals(this, other))
        return true;
      return
        ParentEquals(other) &&
        this.AccessRestrictions == other.AccessRestrictions &&
        this.DisplayName.LocalizedTextArraysEqual(other.DisplayName) &&
        this.NodeId == other.NodeId &&
        this.ReleaseStatus == ReleaseStatus &&
        this.RolePermissions.RolePermissionsEquals(other.RolePermissions) &&
        this.SymbolicName == other.SymbolicName &&
        this.UserWriteMask == other.UserWriteMask &&
        this.WriteMask == other.WriteMask &&
        this.BrowseName == other.BrowseName &&
        this.Documentation == other.Documentation &&
        this.Description.LocalizedTextArraysEqual(other.Description) &&
        this.References.ReferencesEquals(other.References);
    }
    /// <summary>
    /// Implements the == operator. Determines whether two instances of <see cref="UANode"/> represent the same information.
    /// </summary>
    /// <param name="value1">The first object to compare, or null.</param>
    /// <param name="value2">The second object to compare, or null.</param>
    /// <returns>The result of the operator. The <see cref="UANode.Equals"/> procedure is used to compare.</returns>
    public static bool operator == (UANode value1, UANode value2)
    {
      if (Object.ReferenceEquals(value1, null))
        return Object.ReferenceEquals(value2, null);
      return value1.Equals(value2);
    }
    /// <summary>
    /// Implements the != operator. Determines whether two instances of <see cref="UANode"/> don't represent the same information.
    /// </summary>
    /// <param name="value1">The first object to compare, or null.</param>
    /// <param name="value2">The second object to compare, or null.</param>
    /// <returns>The result of the operator. The <see cref="UANode.Equals"/> procedure is used to compare.</returns>
    public static bool operator !=(UANode value1, UANode value2)
    {
      if (Object.ReferenceEquals(value1, null))
        return !Object.ReferenceEquals(value2, null);
      return ! value1.Equals(value2);
    }
    /// <summary>
    /// Indicates whether the the inherited parent object is also equal to another object.
    /// </summary>
    /// <param name="other">An object to compare with this object.</param>
    /// <returns><c>true</c> if the current object is equal to the <paramref name="other">other</paramref>; otherwise,, <c>false</c> otherwise.</returns>
    protected abstract bool ParentEquals(UANode other);
  }
}
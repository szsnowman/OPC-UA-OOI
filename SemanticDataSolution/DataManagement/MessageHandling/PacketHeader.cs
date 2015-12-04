﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UAOOI.SemanticData.DataManagement.Encoding;
using System.Collections.ObjectModel;

namespace UAOOI.SemanticData.DataManagement.MessageHandling
{

  /// <summary>
  /// Class PackageHeader - represent information in the protocol package header.
  /// </summary>
  /// <remarks>
  /// #98: PackageHeader - must be refined
  /// Because the specification is subject of further development this class mus be refined according to further protocol modification.
  /// The following topics must be addressed:
  /// * PublisherId - how to use it and it is static so exchange it is waste of bandwidth.
  /// * Naming convention publisher => producer; subscriber => consumer
  /// * SecurityTokenId - how to use it, how to define it if producer is not OPC UA Server, why exchange it over the wire
  /// </remarks>
  public abstract class PacketHeader
  {

    #region public API
    /// <summary>
    /// Gets the producer package header.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="producerId">The producer identifier.</param>
    /// <param name="dataSetWriterIds">The data set writer ids list. The size of the list must be equal to the <see cref="PacketHeader.MessageCount"/>.</param>
    /// <returns>An instance of the <see cref="PacketHeader"/>.</returns>
    public static PacketHeader GetProducerPackageHeader(IBinaryHeaderWriter writer, Guid producerId, IList<System.UInt32> dataSetWriterIds)
    {
      return new ProducerHeader(writer, producerId, dataSetWriterIds);
    }
    /// <summary>
    /// Gets the consumer package header.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>PackageHeader.</returns>
    public static PacketHeader GetConsumerPackageHeader(IBinaryDecoder reader)
    {
      return new ConsumerHeader(reader);
    }
    /// <summary>
    /// Synchronizes this instance content with the header.
    /// </summary>
    public abstract void WritePacketHeader();
    #endregion

    #region Header
    /// <summary>
    /// If implemented gets or sets the identifier of producer that sends the data.
    /// </summary>
    /// <value>The <see cref="Guid"/> representing the producer.</value>
    public abstract Guid PublisherId { get; set; }
    /// <summary>
    /// If implemented gets or sets the protocol version.
    /// </summary>
    /// <value>The protocol version.</value>
    public abstract byte ProtocolVersion { get; set; }
    /// <summary>
    /// If implemented gets or sets the packet flags.
    /// </summary>
    /// <value>The packet flags.</value>
    public abstract byte PacketFlags { get; set; }
    /// <summary>
    /// If implemented gets or sets the security token identifier.
    /// </summary>
    /// <value>The security token identifier.</value>
    public abstract byte SecurityTokenId { get; set; }
    /// <summary>
    /// If implemented gets or sets the number of messages contained in the packet.
    /// </summary>
    /// <value>The message count.</value>
    public abstract byte MessageCount { get; }
    /// <summary>
    /// If implemented gets or sets the data set writer ids list. The size of the list is defined by the <see cref="PacketHeader.MessageCount"/>.
    /// It identifies the publisher and the message writer responsible for sending Messages for the DataSet.
    /// </summary>
    /// <value>The data set writer ids.</value>
    public abstract ReadOnlyCollection<System.UInt32> DataSetWriterIds { get; }
    #endregion

    #region private implementation
    private class ConsumerHeader : PacketHeader
    {

      #region constructor
      public ConsumerHeader(IBinaryDecoder reader) : base()
      {
        m_Reader = reader;
        ReadPacketHeader();
      }
      #endregion

      #region PackageHeader
      public override byte MessageCount
      {
        get { return m_MessageCount; }
      }
      public override byte PacketFlags
      {
        get; set;
      }
      public override byte ProtocolVersion
      {
        get; set;
      }
      public override Guid PublisherId
      {
        get; set;
      }
      public override byte SecurityTokenId
      {
        get; set;
      }
      public override ReadOnlyCollection<System.UInt32> DataSetWriterIds
      {
        get { return m_DataSetWriterIds; }
      }
      public override void WritePacketHeader()
      {
        throw new ApplicationException("Consumer packet is read only");
      }
      #endregion

      #region private
      private IBinaryDecoder m_Reader;
      private ReadOnlyCollection<uint> m_DataSetWriterIds;
      private byte m_MessageCount;
      private void ReadPacketHeader()
      {
        PublisherId = m_Reader.ReadGuid();
        ProtocolVersion = m_Reader.ReadByte();
        PacketFlags = m_Reader.ReadByte();
        SecurityTokenId = m_Reader.ReadByte();
        m_MessageCount = m_Reader.ReadByte();
        List<System.UInt32> _ids = new List<uint>();
        for (int i = 0; i < MessageCount; i++)
          _ids.Add(m_Reader.ReadUInt32());
        m_DataSetWriterIds = new ReadOnlyCollection<System.UInt32>(_ids);
      }
      #endregion

    }
    private class ProducerHeader : PacketHeader
    {
      #region constructor
      public ProducerHeader(IBinaryHeaderWriter writer, Guid producerId, IList<System.UInt32> dataSetWriterIds) : base()
      {
        if (writer == null)
          throw new ArgumentNullException(nameof(writer));
        PublisherId = producerId;
        PacketFlags = Convert.ToByte(PacketFlagsDefinitions.PacketFlagsMessageType.RegularMessages);
        ProtocolVersion = CommonDefinitions.ProtocolVersion;
        SecurityTokenId = 0;
        DataSetWriterIds = new ReadOnlyCollection<uint>(dataSetWriterIds);
        MessageCount = Convert.ToByte(DataSetWriterIds.Count);
        ushort _packageLength = Convert.ToUInt16(m_PackageHeaderLength + dataSetWriterIds.Count * 4);
        m_HeaderWriter = new HeaderWriter(writer, _packageLength);
      }
      #endregion

      #region PackageHeader
      /// <summary>
      /// Gets or sets the number of messages contained in the packet.
      /// </summary>
      /// <value>The message count.</value>
      public override byte MessageCount
      {
        get;
      }
      /// <summary>
      /// Gets or sets the message flags.
      /// </summary>
      /// <value>The message flags.</value>
      public override byte PacketFlags
      {
        get; set;
      }
      /// <summary>
      /// Gets or sets the protocol version.
      /// </summary>
      /// <value>The protocol version.</value>
      public override byte ProtocolVersion
      {
        get; set;
      }
      /// <summary>
      /// Gets or sets the identifier of producer that sends the data.
      /// </summary>
      /// <value>The <see cref="Guid" /> representing the producer.</value>
      public override Guid PublisherId
      {
        get; set;
      }
      /// <summary>
      /// Gets or sets the security token identifier.
      /// </summary>
      /// <value>The security token identifier.</value>
      public override byte SecurityTokenId
      {
        get; set;
      }
      public override ReadOnlyCollection<System.UInt32> DataSetWriterIds
      {
        get;
      }
      /// <summary>
      /// Synchronizes this instance content with the header.
      /// </summary>
      public override void WritePacketHeader()
      {
        m_HeaderWriter. WriteHeader(WriteHeader);
      }
      private void WriteHeader(IBinaryHeaderWriter m_Writer, ushort dataLength)
      {
        Debug.Assert(DataSetWriterIds != null);
        Debug.Assert(DataSetWriterIds.Count == MessageCount);
        m_Writer.Write(PublisherId);
        m_Writer.Write(ProtocolVersion);
        m_Writer.Write(PacketFlags);
        m_Writer.Write(SecurityTokenId);
        m_Writer.Write(MessageCount);
        if (MessageCount == 0)
          return;
        for (int i = 0; i < DataSetWriterIds.Count; i++)
          m_Writer.Write(DataSetWriterIds[i]);
      }
      #endregion

      #region private
      //vars
      private HeaderWriter m_HeaderWriter;
      private const ushort m_PackageHeaderLength = 20;
      #endregion

    }
    #endregion

  }

}

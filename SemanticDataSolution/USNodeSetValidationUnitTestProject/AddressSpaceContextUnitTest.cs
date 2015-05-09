﻿
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using UAOOI.SemanticData.UANodeSetValidation;
using UAOOI.SemanticData.UANodeSetValidation.XML;
using UAOOI.SemanticData.UnitTest.Helpers;


namespace UAOOI.SemanticData.UnitTest
{
  [TestClass]
  public class AddressSpaceContextUnitTest
  {
    [TestMethod]
    public void AddressSpaceContextUnitTestTestMethod()
    {
      UANodeSet _ns = TestData.CreateNodeSetModel();
      List<TraceMessage> _trace = new List<TraceMessage>();
      int _diagnosticCounter = 0;
      Assert.IsTrue(_ns.NamespaceUris.Length >= 1, "Wrong test data - NamespaceUris must contain more then 2 items");
      AddressSpaceContext _as = new AddressSpaceContext(x => { Helpers.TraceHelper.TraceDiagnostic(x, _trace, ref _diagnosticCounter); });
      Assert.IsNotNull(_as);
      ExportModelFactoryStub _md = new ExportModelFactoryStub();
      Assert.Inconclusive();
      _as.CreateInstance(_ns.NamespaceUris[0], x => x.ImportNodeSet(_ns, true), new ExportModelFactoryStub());
      Assert.IsNotNull(_md);
      Assert.AreEqual<int>(0, _trace.Where<TraceMessage>(x => x.BuildError.Focus != Focus.Diagnostic).Count<TraceMessage>());
    }
    private class ExportModelFactoryStub : IExportModelFactory
    {

      public void CreateNamespace(string uri)
      {
        throw new System.NotImplementedException();
      }

      public NodeFactory NewExportNodeFFactory<NodeFactory>() where NodeFactory : IExportNodeFactory
      {
        throw new System.NotImplementedException();
      }
    }
  }
}

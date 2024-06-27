
#if DEBUG
// WHEN THIS IS UN COMMENTED REMOVE ALL VISION REFERENCES FROM PROJECT AND CLEAN
//#define DYNAMIC
#else
#define DYNAMIC
#endif

using System;
using System.Linq;
using System.Collections.Generic;

#if DYNAMIC
using System.IO;
using System.Reflection;
#else
using Sybiz.Vision.Platform.Debtors;
using Sybiz.Vision.Platform.Debtors.Transaction;
using Sybiz.Vision.Platform.Security;
using Sybiz.Vision.Platform.Core.Enumerations;
#endif

namespace DynamicConsole
{
  internal static class VisionService
  {

#if DYNAMIC

    //REFERENCE TO ASSEMBIES TO USE
    private static Assembly platform = null;
    private static Assembly winUI = null;

    #region  Static Constructor

    static VisionService()
    {
      var platformDll = "Sybiz.Vision.Platform.dll";
      var winUIDll = "Sybiz.Vision.WinUI.dll";

      var dependantFiles = new List<string>()
      {
        "Csla.dll", //REQUIRED FOR ALL VISION
        "Sybiz.Vision.Module.DR.dll", // NEEDD TO MAKE A CALL TO A SPECIFIC BREAKPOINT HELPER EG DEBTORS MODULE
        "Sybiz.Vision.Module.Coordinator.dll" // NEEDD TO MAKE A CALL TO A SPECIFIC BREAKPOINT HELPER
      };

      var allFiles = new List<string>();
      allFiles.AddRange(dependantFiles);
      allFiles.Add(platformDll);
      allFiles.Add(winUIDll);

      var sourcePath = "";
      var executingPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

#if DEBUG
      //THIS IS WHERE YOU WILL PULL FILES IN DEBUG MODE FOR DYNAMIC
      sourcePath = @"C:\Development\SRVTFS\Vision.NET\Trunk\Vision\bin\Debug";
#else
      sourcePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Sybiz\Sybiz Vision");
#endif


      CopyFileIfVersionNewer(executingPath, sourcePath, allFiles.ToArray());
      platform = LoadAssembly(executingPath, platformDll);
      winUI = LoadAssembly(executingPath, winUIDll);
      LoadAssemblies(executingPath, dependantFiles.ToArray());
    }

    #endregion

    #region Assembly Management Helpers

    private static void CopyFileIfVersionNewer(string executionPath, string sourcePath, string[] fileNames)
    {
      for (int i = 0; i < fileNames.Length; i++)
      {
        string sourceFile = Path.Combine(sourcePath, fileNames[i]);
        string executionFile = Path.Combine(executionPath, fileNames[i]);

        var existSource = File.Exists(sourceFile);
        var existExecution = File.Exists(executionFile);
        var copyFile = existSource && existExecution == false;

        if (copyFile == false && existSource && existExecution)
        {
          var sourceVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(sourceFile).FileVersion;
          var executionVersion = System.Diagnostics.FileVersionInfo.GetVersionInfo(executionFile).FileVersion;
          copyFile = sourceVersion.CompareTo(executionVersion) > 0;
        }

        if (copyFile)
        {
          File.Copy(sourceFile, executionFile, true);
        }
      }

    }

    private static Assembly LoadAssembly(string assemblyPath, string assemblyFile)
    {
      var file = Path.Combine(assemblyPath, assemblyFile);
      if (File.Exists(file))
      {
        return Assembly.LoadFrom(file);
      }
      return default;
    }

    private static void LoadAssemblies(string assemblyPath, string[] fileNames)
    {
      for (int i = 0; i < fileNames.Length; i++)
      {
        LoadAssembly(assemblyPath, fileNames[i]);
      }
    }

    #endregion

#endif

    #region Helper Methods

#if DYNAMIC

    internal static bool LogIn(string userName, string password, string companyConnectionString, string commonConnectionString)
    {
      try
      {
        var principalType = platform.GetType("Sybiz.Vision.Platform.Security.Principal");
        var method = principalType.GetMethod("LogIn", new Type[] { typeof(string), typeof(string), typeof(string), typeof(string) });
        var canLogIn = (bool)method.Invoke(null, new object[] { userName, password, companyConnectionString, commonConnectionString });

        return canLogIn;
      }
      catch (Exception)
      {
        return false;
      }
    }

    internal static void LogOut()
    {
      try
      {
        var principalType = platform.GetType("Sybiz.Vision.Platform.Security.Principal");
        //Method in Principal
        var method = principalType.GetMethod("LogOut", new Type[] { });
        method.Invoke(null, new object[] { });
      }
      catch (Exception) { }
    }

    internal static dynamic GetCustomerInfoList()
    {
      var customerInfoListType = platform.GetType("Sybiz.Vision.Platform.Debtors.CustomerInfoList");
      //Static Method in BaseType - Use reflection to determine
      var method = customerInfoListType.BaseType.GetMethod("GetList", new Type[] { });
      return method.Invoke(null, new object[] { });
    }

    internal static dynamic NewSalesInvoice(dynamic lastInvoice)
    {
      var invoiceType = platform.GetType("Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice");
      //Static Method in BaseType the BaseType - Use reflection to determine
      var method = invoiceType.BaseType.BaseType.GetMethod("NewObject");
      return method.Invoke(null, new object[] { lastInvoice, false, false });
    }

    internal static dynamic GetTransactionType(string value)
    {
      var enumType = platform.GetType("Sybiz.Vision.Platform.Core.Enumerations.TransactionType");
      var values = Enum.GetValues(enumType);
      for (var i = 0; i < values.Length; i++)
      {
        if (values.GetValue(i).ToString().ToLower() == value.ToLower())
          return values.GetValue(i);
      }
      throw new InvalidCastException($"Unable to cast {value} to a valid TransactionType!");
    }

    internal static byte[] CreateTransactionDocument(int transactionId, dynamic transactionType, string templateDescription)
    {
      var transactionEngineType = winUI.GetType("Sybiz.Vision.WinUI.Utilities.BreakpointHelpers");
      //Method in BreakpointHelpers
      var method = transactionEngineType.GetMethod("CreateTransactionDocument");
      return (byte[])method.Invoke(null, new object[] { transactionType, transactionId, templateDescription, false });
    }

#else

    internal static bool LogIn(string userName, string password, string companyConnectionString, string commonConnectionString)
    {
      try
      {
        return Principal.LogIn(userName, password, companyConnectionString, commonConnectionString);
      }
      catch (Exception)
      {
        return false;
      }
    }

    internal static void LogOut()
    {
      Principal.LogOut();
    }

    internal static CustomerInfoList GetCustomerInfoList()
    {
      return CustomerInfoList.GetList();
    }

    internal static SalesInvoice NewSalesInvoice(SalesInvoice lastInvoice)
    {
      return SalesInvoice.NewObject(lastInvoice);
    }

    internal static TransactionType GetTransactionType(string value)
    {
      var values = Enum.GetValues(typeof(TransactionType));
      for (var i = 0; i < values.Length; i++)
      {
        if (values.GetValue(i).ToString().ToLower() == value.ToLower())
          return (TransactionType)values.GetValue(i);
      }
      throw new InvalidCastException($"Unable to cast {value} to a valid TransactionType!");
    }

    internal static byte[] CreateTransactionDocument(int transactionId, TransactionType transactionType, string templateDescription)
    {
      return Sybiz.Vision.WinUI.Utilities.BreakpointHelpers.CreateTransactionDocument(transactionType, transactionId, templateDescription, false);
    }

#endif

    #endregion
  }
}

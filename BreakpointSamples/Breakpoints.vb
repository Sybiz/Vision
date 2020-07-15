Public Class Breakpoints

  'Scenario: Automatically allow the addition of documents to new sales invoices
  'Prerequisities: None
  Public Sub AfterSalesInvoiceNew(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)

    'Create the DevExpress XtraOpenFileDialog using our NewOpenFileDialog function (optional parameters are filter, multiselect and default extension) as per comment below, running code only provided to allow compiliation 
    'Using dlg As DevExpress.XtraEditors.XtraOpenFileDialog = Sybiz.Vision.WinUI.Utilities.FormFunctions.NewOpenFileDialog(filter:="All Files (*.*)|*.*", multi:=True)
    Using dlg As New OpenFileDialog()
      'Add each file that was selected to the documents of the sales invoice
      If dlg.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
        For Each document As String In dlg.FileNames
          transaction.Documents.AddNew(dlg.FileName)
        Next
      End If
    End Using

  End Sub

  'Scenario: Enforce that the invoice quantity cannot be zero on sales invoices
  'Prerequisities: None
  Public Sub BeforeSalesInvoiceSave(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)

    Dim foundZeroQuantity As Boolean = False

    'Iterate through each sales invoice line in the sales invoice to determine if a zero invoice quantity line exists
    For Each line As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine In transaction.Lines
      If line.QuantityInvoice = 0 Then
        foundZeroQuantity = True
      End If
    Next

    If foundZeroQuantity Then
      'Set to true to cancel the save
      e.Cancel = True
      MessageBox.Show("Cancelled Save: Zero Invoice Quantity Line on Sales Invoice", "Breakpoints")
    End If

  End Sub

  'Scenario: Enforce a minimum spend of $100 on sales invoices
  'Prerequisities: None
  Public Sub BeforeSalesInvoiceProcess(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCancelEventArgs)

    If transaction.Total < 100D Then
      'Set to true to cancel the process
      e.Cancel = True
      MessageBox.Show("Cancelled Process: Invoice Under $100", "Breakpoints")
    End If

  End Sub

  'Scenario: Assign any case files on the sales invoice to the current user after saving the sales invoice
  'Prerequisities: Attached case files and edit rights to case files
  Public Sub AfterSalesInvoiceSave(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)

    'Check that there are case files on the sales invoice
    If transaction.CaseFilesExist Then
      'Iterate through each of the case files
      For Each storeCaseFile As Sybiz.Vision.Platform.ContactManagement.CaseFileStore In transaction.CaseFiles
        'Get the case file object from the case file store object
        Dim objectCaseFile As Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile = Sybiz.Vision.Platform.ContactManagement.Transaction.CaseFile.GetObject(storeCaseFile.Id)
        'No need to do anything if the case file user is the current user
        If objectCaseFile.User <> Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId Then
          'Assign the current user to the case file
          objectCaseFile.User = Sybiz.Vision.Platform.Security.Principal.CurrentPrincipal().UserId
          'Save the case file
          objectCaseFile = objectCaseFile.Save()
        End If
      Next
    End If

  End Sub

  'Scenario: Email the sales invoice documents to the specified email address after processing the sales invoice (e.g. sales representative)
  'Prerequisities: Sales invoice transaction settings (e.g. email preview)
  Public Sub AfterSalesInvoiceProcess(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)

    'Source: Transaction to generate documents
    'CompanyTemplates: Determines whether company templates are used
    'UserTemplates: Determines whether user templates are used
    'PrintHandler: Handler for printing documents (can be nothing to not print)
    'PreviewHandler: Handler for print previewing documents (can be nothing to not print preview)
    'GenerateHandler: Handler for generating documents
    'EmailHandler: Handler for emailing documents (can be nothing to not email)
    'EmailPreviewHandler: Handler for email previewing documents (can be nothing to not email preview)
    'Subject (Optional): Specify a subject for emailing documents
    'Body (Optional): Specify a body for emailing documents
    'AlternateAccountId (Optional): Specify an alternate account (e.g. customer, supplier) for emailing documents
    'AlternateEmailAddress (Optional): Specify an alternate email address for emailing documents
    'Notes: Handlers for printing and emailing can be swapped with other handlers if desired (e.g. use the email handler for an email preview)

    Dim subject As String = System.String.Empty
    Dim body As String = System.String.Empty

    'Get the subject and body from the relevant email template
    Sybiz.Vision.WinUI.Utilities.FormFunctions.GetEmailSubjectAndBody(transaction, subject, body)
    Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(transaction, True, True, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PrintTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PreviewTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailPreviewTransaction, Subject:=subject, Body:=body, AlternateEmailAddress:="email@email.com")

  End Sub

  'Scenario: Email the sales invoice documents to an alternative customer (e.g. head office / branch office situations) after processing the sales invoice
  'Prerequisities: Sales invoice transaction settings (e.g. email preview)
  Public Sub AfterSalesInvoiceProcess2(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)

    'Determine if there is a difference in the order, delivery and invoice customers
    If transaction.OrderCustomer <> transaction.DeliveryCustomer OrElse transaction.DeliveryCustomer <> transaction.InvoiceCustomer Then
      'Stores the value of the alternate customer
      Dim alternateAccount As Integer = 0

      'Check if it is the order or the delivery that is different
      If transaction.OrderCustomer <> transaction.InvoiceCustomer Then
        alternateAccount = transaction.OrderCustomer
      Else
        alternateAccount = transaction.DeliveryCustomer
      End If

      'See above for explanation of parameters
      Sybiz.Vision.Platform.Core.Transaction.TransactionDocumentEngine.ProduceTransactionDocument(transaction, True, True, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PrintTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.PreviewTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.ExportTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailTransaction, AddressOf Sybiz.Vision.WinUI.Utilities.FormFunctions.EmailPreviewTransaction, AlternateAccountId:=alternateAccount)
    End If

  End Sub

  'Scenario: Automatically add a sales template to the sales invoice if it exists
  'Prerequisities: None
  Public Sub SalesInvoiceCustomerChanged(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointEventArgs)

    'Use the scalar command to get the top sales template for the customer on the sales invoice
    Dim templateId As Integer = Sybiz.Vision.Platform.Core.Data.ScalarCommand.Execute(Of Integer)(String.Format("SELECT TOP 1 SalesTemplateId FROM [dr].[SalesTemplate] WHERE CustomerId = {0}", transaction.Customer))
    'Cast the sales transaction to an ISalesTransaction
    Dim iTransaction As Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransaction = DirectCast(transaction, Sybiz.Vision.Platform.Debtors.Transaction.ISalesTransaction)

    'Only add the template document if one was returned from the scalar command
    If templateId > 0 Then
      iTransaction.AddTemplateDocument(templateId)
    End If

  End Sub

  'Scenario: Prevent lines with identical types and items from being added to sales invoices
  'Prerequisities: None
  Public Sub SalesInvoiceCellValueChanged(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellValueChangedEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))

    'Check for the item field
    If e.FieldName = "Account" Then
      'Iterate through each sales invoice line in the sales invoice
      For Each line As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine In transaction.Lines
        'Check the line being added against the currently iterated sales invoice line
        If e.Line.Id <> line.Id AndAlso e.Line.AccountType = line.AccountType AndAlso e.Line.Account = line.Account Then
          'Remove the line being added if the type and item is identical
          MessageBox.Show("Removing Line: Existing Line with Item and Account", "Breakpoints")
          transaction.Lines.Remove(e.Line)
          Exit For
        End If
      Next
    End If

  End Sub

  'Scenario: Define a custom ribbon button
  'Prerequisities: None
  Public Sub SalesInvoiceCustomRibbonButtonRegister(sender As System.Object, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonRegisterEventArgs)

    'Key: Used to identify the button in the CustomRibbonButtonClick breakpoint
    'Description: Sets the description
    'Style: Sets the style
    'ToolTipTitle: Sets the tooltip title
    'ToolTipDescription: Sets the tooltip description
    'Glyph: Sets the small image
    'LargeGlyph: Sets the large image
    'BeginGroup: Determines whether to start a new group
    'RibbonPageIndex: Determines the page of the button (i.e. home = 0, actions = 1, options = 2)

    e.Register.RegisterCustomRibbonButton("GoogleMaps", "Google Maps", Sybiz.Vision.Platform.Admin.Breakpoints.CustomRibbonButtonStyle.Default, "Google Maps", "Shows the delivery address on google maps", Sybiz.Vision.WinUI.My.Resources.Resources.BLANK16, Sybiz.Vision.WinUI.My.Resources.Resources.BLANK32, True, 0)

  End Sub

  'Scenario: Show the delivery address for the customer on the sales invoice in google maps
  'Prerequisities: Custom ribbon button with the key "GoogleMaps" (see above)
  Public Sub SalesInvoiceCustomRibbonButtonClick(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)

    'Check the key for the button that was clicked
    If e.Key = "GoogleMaps" AndAlso transaction.Customer > 0 Then

      'Get the default delivery address for the customer on the sales invoice
      Dim customer As Sybiz.Vision.Platform.Debtors.CustomerDetailInfo = Sybiz.Vision.Platform.Debtors.CustomerDetailInfo.GetObject(transaction.Customer)
      Dim deliveryAddress As Sybiz.Vision.Platform.Debtors.DeliveryAddressDetailInfo = customer.DeliveryAddress.DefaultAddress

      If Not System.String.IsNullOrWhiteSpace(deliveryAddress.Street) Then
        'Launch google maps using the default delivery address
        System.Diagnostics.Process.Start(System.String.Format("https://www.google.com/maps/search/?api=1&query={0}+{1}+{2}", deliveryAddress.Street, deliveryAddress.Suburb, deliveryAddress.PostCode))
      Else
        'Need a delivery address to launch google maps
        MessageBox.Show("No Delivery Address", "Breakpoints")
      End If

    Else
      'Need a customer to get the delivery address to launch google maps
      MessageBox.Show("No Customer", "Breakpoints")
    End If

  End Sub

  'Scenario: Disable the tax code field on sales invoices to enforce the sales tax code on products
  'Prerequisities: None
  Public Sub SalesInvoiceDisableCustomCellRepository(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))

    'Check for the tax code field
    If e.FieldName = "TaxCode" Then
      'Set to true to disable the tax code field
      e.Handled = True
    End If

  End Sub

  'Scenario: Enable the custom repository on the quantity invoice field on sales invoices
  'Prerequisities: None
  Public Sub SalesInvoiceUseCustomCellRepository(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))

    'Check for the quantity invoice field
    If e.FieldName = "QuantityInvoice" Then
      'Set to true to enable the custom repository
      e.Handled = True
    End If

  End Sub

  'Scenario: Use the custom repository on the quantity invoice field to enter the quantity with a DevExpress XtraInputBox
  'Prerequisities: None
  Public Sub SalesInvoiceUseCustomRepositoryButtonClick(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCellRepositoryEventArgs(Of Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoiceLine))

    'Create and show the DevExpress XtraInputBox with the existing quantity invoice
    Dim input As String = InputBox("Enter Quantity Invoice:", "Breakpoints", e.Line.QuantityInvoice)
    Dim quantity As Decimal = 0D

    If String.IsNullOrWhiteSpace(input) Then
      'Do Nothing - Cancel Clicked
    ElseIf Not String.IsNullOrWhiteSpace(input) AndAlso Decimal.TryParse(input, quantity) Then
      e.Line.QuantityInvoice = quantity
    Else
      MessageBox.Show("Not a Decimal Value", "Breakpoints")
    End If

  End Sub

  'Scenario: Create a workflow to create a new supplier and product and then use them on a new purchase order
  'Prerequisities: None
  Public Sub ExternalApplicationCustomRibbonButtonClick(e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)

    If e.Key = "BreakpointKey" Then
      'Show a new supplier form
      Dim formSupplier As Sybiz.Vision.Module.Coordinator.VisionDialogResult = Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().CR.ShowSupplierForm(e.Form, 0, Nothing, Nothing)

      'Check if a supplier was created
      If formSupplier.Id > 0 Then
        'Show a new product form
        Dim formProduct As Sybiz.Vision.Module.Coordinator.VisionDialogResult = Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication().IC.ShowProductForm(e.Form, 0, Nothing, Nothing)

        'Check if a product was created
        If formProduct.Id > 0 Then
          'Create a new purchase order with the supplier
          Dim newOrder As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrder = Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrder.NewObject(Nothing, True, True)
          newOrder.Supplier = formSupplier.Id
          newOrder.Reference = "Breakpoints Demo"

          'Create a new purchase order line with the product
          Dim theLine As Sybiz.Vision.Platform.Creditors.Transaction.PurchaseOrderLine = newOrder.Lines.AddNew(Sybiz.Vision.Platform.Core.Enumerations.PurchaseLineType.IC)
          theLine.Account = formProduct.Id
          theLine.Location = 1
          theLine.TaxCode = 1

          'Save the purchase order
          newOrder = newOrder.Save()
          Dim newOrderId = newOrder.Id

          'Open the purchase order
          Sybiz.Vision.Module.Coordinator.VisionApplication.GetApplication.CR.ShowPurchaseOrderForm(e.Form, newOrder.Id, newOrder.Supplier)
        End If

      End If

    End If

  End Sub

End Class
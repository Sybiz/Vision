Public Class OpenAddressInGoogleMapsWithCustomRibbonButton

    'Scenario: Define a custom ribbon button
    'Prerequisities: None
    'Breakpoint: SalesInvoiceCustomRibbonButtonRegister

    Public Sub Invoke(sender As System.Object, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonRegisterEventArgs)
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
    'Breakpoint: SalesInvoiceCustomRibbonButtonClick

    Public Sub Invoke(transaction As Sybiz.Vision.Platform.Debtors.Transaction.SalesInvoice, e As Sybiz.Vision.Platform.Admin.Breakpoints.BreakpointCustomRibbonButtonClickEventArgs)
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
                MessageBox.Show("No delivery address", "Breakpoints")
            End If

        Else
            'Need a customer to get the delivery address to launch google maps
            MessageBox.Show("No customer", "Breakpoints")
        End If
    End Sub

End Class
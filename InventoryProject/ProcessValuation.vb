'Title:         Inventory Valuation
'Date:          4-7-15
'Author:        Terry Holmes

'Description:   This Application will give a complete corporate inventory valuation

Option Strict On

Public Class ProcessValuation

    'Setting up the global variables
    Private ThePartNumberDataSet As PartNumberDataSet
    Private ThePartNumberDataTier As PartNumberDataTier
    Private WithEvents ThePartNumberBindingSource As BindingSource

    Private TheClevelandMaterialDataSet As ClevelandMaterialDataSet
    Private TheClevelandMaterialDataTier As InventoryProjectDataTier
    Private WithEvents TheClevelandMaterialBindingSource As BindingSource

    Private TheClevelandCableDataSet As ClevelandCableDataSet
    Private TheClevelandCableDataTier As InventoryProjectDataTier
    Private WithEvents TheClevelandCableBindingSource As BindingSource

    Private TheElmiraMaterialDataSet As ElmiraMaterialDataSet
    Private TheElmiraMaterialDataTier As InventoryProjectDataTier
    Private WithEvents TheElmiraMaterialBindingSource As BindingSource

    Private TheElmiraCableDataSet As ElmiraCableDataSet
    Private TheElmiraCableDataTier As InventoryProjectDataTier
    Private WithEvents TheElmiraCableBindingSource As BindingSource

    Private TheJamestownMaterialDataSet As JamestownMaterialDataSet
    Private TheJamestownMaterialDataTier As InventoryProjectDataTier
    Private WithEvents TheJamestownMaterialBindingSource As BindingSource

    Private TheJamestownCableDataSet As JamestownCableDataSet
    Private TheJamestownCableDataTier As InventoryProjectDataTier
    Private WithEvents TheJamestownCableBindingSource As BindingSource

    Private TheMilwaukeeMaterialDataSet As MilwaukeMaterialDataSet
    Private TheMIlwaukeeMaterialDataTier As InventoryProjectDataTier
    Private WithEvents TheMIlwaukeeMaterialBindingSource As BindingSource

    Private TheMilwaukeeCableDataSet As MilwaukeeCableDataSet
    Private TheMilwaukeeCableDataTier As InventoryProjectDataTier
    Private WithEvents TheMilwaukeeCableBindingSource As BindingSource

    Private TheSyracuseMaterialDataSet As SyracuseMaterialDataSet
    Private TheSyracuseMaterialDataTier As InventoryProjectDataTier
    Private WithEvents TheSyracuseMaterialBindingSource As BindingSource

    Private TheSyracuseCableDataSet As SyracuseCableDataSet
    Private TheSyracuseCableDataTier As InventoryProjectDataTier
    Private WithEvents TheSyracuseCableBindingSource As BindingSource

    Private TheOfficeValuationDataSet As OfficeValuationDataSet
    Private TheOfficeValuationDataTier As InventoryProjectDataTier
    Private WithEvents TheOfficeValuationBindingSource As BindingSource

    Private TheEmployeeDataSet As EmployeesDataSet
    Private TheEmployeeDataTier As EmployeeDataTier
    Private WithEvents TheEmployeeBindingSource As BindingSource

    'Setting up the structure
    Structure PartValuation
        Dim mintPartID As Integer
        Dim mstrPartNumber As String
        Dim mstrPartDescription As String
        Dim mdouPartPrice As Double
    End Structure

    Dim structPartValuation() As PartValuation
    Dim mintPartCounter As Integer
    Dim mintPartUpperLimit As Integer

    Dim mdouOfficeValuation As Double
    Private addingBoolean As Boolean = False
    Private editingBoolean As Boolean = False
    Private previousSelectedIndex As Integer

    'Setting up for the date
    Dim mdatLogDate As Date = Date.Now
    Dim mstrLogDate As String

    'Setting up for the Office Array
    Dim mintOfficeSelectedIndex() As Integer
    Dim mintOfficeCounter As Integer
    Dim mintOfficeUpperLimit As Integer
    Dim TheKeywordSearch As New KeyWordSearchClass
    Dim mstrInventoryType As String

    Private Sub SetOfficeValuationDataBindings()

        'this will bind the controls of the office valuation
        Try

            'binding the data variables
            TheOfficeValuationDataTier = New InventoryProjectDataTier
            TheOfficeValuationDataSet = TheOfficeValuationDataTier.GetOfficeValuationInformation
            TheOfficeValuationBindingSource = New BindingSource

            'Setting up the binding source
            With TheOfficeValuationBindingSource
                .DataSource = TheOfficeValuationDataSet
                .DataMember = "officevaluation"
                .MoveLast()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboValueOfficeID
                .DataSource = TheOfficeValuationBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheOfficeValuationBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'Setting the rest of the controls
            txtValue.DataBindings.Add("text", TheOfficeValuationBindingSource, "Value")
            txtValueDate.DataBindings.Add("text", TheOfficeValuationBindingSource, "Date")
            txtValueOfficeID.DataBindings.Add("text", TheOfficeValuationBindingSource, "OfficeID")
            txtValueInventoryType.DataBindings.Add("text", TheOfficeValuationBindingSource, "InventoryType")

        Catch ex As Exception

            'message to user
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        'This will close the program
        CloseProgram.ShowDialog()

    End Sub

    Private Sub ProcessValuation_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'This will set up the binding for the Parts Controls
        'Setting up local variables
        Dim intCounter As Integer
        Dim intNumberOfRecords As Integer
        Dim strValueForValidation As String

        'loading the last transaction log
        Logon.mstrLastTransactionSummary = "LOADED INVENTORY VALUATION"
        LastTransaction.Show()
        mstrLogDate = CStr(mdatLogDate)

        Try

            'Setting up the data controls
            ThePartNumberDataTier = New PartNumberDataTier
            ThePartNumberDataSet = ThePartNumberDataTier.GetPartNumberInformation
            ThePartNumberBindingSource = New BindingSource

            'Setting up the binding source
            With ThePartNumberBindingSource
                .DataSource = ThePartNumberDataSet
                .DataMember = "partnumbers"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboPartID
                .DataSource = ThePartNumberBindingSource
                .DisplayMember = "PartID"
                .DataBindings.Add("text", ThePartNumberBindingSource, "PartID", False, DataSourceUpdateMode.Never)
            End With

            'Setting up the rest the controls
            txtPartDescription.DataBindings.Add("text", ThePartNumberBindingSource, "Description")
            txtPartNumber.DataBindings.Add("Text", ThePartNumberBindingSource, "PartNumber")
            txtPartPrice.DataBindings.Add("text", ThePartNumberBindingSource, "Price")

            'Loading the structure
            intNumberOfRecords = cboPartID.Items.Count - 1
            ReDim structPartValuation(intNumberOfRecords)
            mintPartCounter = 0

            'beginning loop
            For intCounter = 0 To intNumberOfRecords

                'incrementing the combo box
                cboPartID.SelectedIndex = intCounter

                'Getting price
                strValueForValidation = txtPartPrice.Text

                If IsNumeric(strValueForValidation) Then

                    structPartValuation(intCounter).mintPartID = CInt(cboPartID.Text)
                    structPartValuation(intCounter).mstrPartDescription = txtPartDescription.Text
                    structPartValuation(intCounter).mstrPartNumber = txtPartNumber.Text
                    structPartValuation(intCounter).mdouPartPrice = CDbl(txtPartPrice.Text)
                    mintPartCounter += 1

                End If
            Next

            'Setting up the upper limit
            mintPartUpperLimit = mintPartCounter - 1
            mintPartCounter = 0
            SetOfficeDataBindings()
            SetOfficeValuationDataBindings()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
    Private Sub SetOfficeDataBindings()

        'try catch for exceptions
        'Setting local variables
        Dim intCounter As Integer
        Dim intNumberOfRecords As Integer
        Dim strLastNameForSearch As String
        Dim strLastNameFromTable As String

        'This will set up the employee data bindings
        Try

            'Setting up the data bindings
            TheEmployeeDataTier = New EmployeeDataTier
            TheEmployeeDataSet = TheEmployeeDataTier.GetEmployeesInformation
            TheEmployeeBindingSource = New BindingSource

            'Setting up the Binding Source
            With TheEmployeeBindingSource
                .DataSource = TheEmployeeDataSet
                .DataMember = "employees"
                .MoveFirst()
                .MoveLast()
            End With

            'setting up the combo box
            With cboEmployeeID
                .DataSource = TheEmployeeBindingSource
                .DisplayMember = "EmployeeID"
                .DataBindings.Add("text", TheEmployeeBindingSource, "EmployeeID", False, DataSourceUpdateMode.Never)
            End With

            'Setting up the rest of the controls
            txtEmployeeFirstName.DataBindings.Add("text", TheEmployeeBindingSource, "FirstName")
            txtEmployeeLastName.DataBindings.Add("text", TheEmployeeBindingSource, "LastName")

            'Setting up for loop
            intNumberOfRecords = cboEmployeeID.Items.Count - 1
            ReDim mintOfficeSelectedIndex(intNumberOfRecords)
            mintOfficeCounter = 0
            strLastNameForSearch = "PARTS"

            'Beginning loop
            For intCounter = 0 To intNumberOfRecords

                'Incrementing the combo box
                cboEmployeeID.SelectedIndex = intCounter

                'getting the employee last name
                strLastNameFromTable = txtEmployeeLastName.Text

                'if statements to compare findings
                If strLastNameForSearch = strLastNameFromTable Then

                    'loading up array
                    mintOfficeSelectedIndex(mintOfficeCounter) = intCounter
                    mintOfficeCounter += 1

                End If
            Next

            'Setting conditions of the combo box
            mintOfficeUpperLimit = mintOfficeCounter - 1
            mintOfficeCounter = 0
            cboEmployeeID.SelectedIndex = mintOfficeSelectedIndex(0)

        Catch ex As Exception

            'Message to user
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try
        
    End Sub
    Private Sub ClearTransactionDataBindings()

        'This will clear the transaction controls
        cboTransactionID.DataBindings.Clear()
        txtTransactionPartNumber.DataBindings.Clear()
        txtTransactionCost.DataBindings.Clear()
        txtTransactionQuantity.DataBindings.Clear()
        txtTransactionTotal.DataBindings.Clear()
        txtTransactionDescription.DataBindings.Clear()
        txtTransactionWarehouseID.DataBindings.Clear()

    End Sub
    Private Sub SetComboBoxBinding()
        'This will set the combo box bindings
        With cboValueOfficeID
            If (addingBoolean Or editingBoolean) Then
                .DataBindings!text.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
                .DropDownStyle = ComboBoxStyle.Simple
            Else
                .DataBindings!text.DataSourceUpdateMode = DataSourceUpdateMode.Never
                .DropDownStyle = ComboBoxStyle.DropDownList
            End If
        End With
    End Sub
    Private Sub SetClevelandMaterialDataBindings()
        Try

            'Setting up the data variables
            TheClevelandMaterialDataTier = New InventoryProjectDataTier
            TheClevelandMaterialDataSet = TheClevelandMaterialDataTier.GetClevelandMaterialInformation
            TheClevelandMaterialBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheClevelandMaterialBindingSource
                .DataSource = TheClevelandMaterialDataSet
                .DataMember = "clevelandmaterial"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheClevelandMaterialBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheClevelandMaterialBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheClevelandMaterialBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheClevelandMaterialBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheClevelandMaterialBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheClevelandMaterialBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheClevelandMaterialBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheClevelandMaterialBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Cleveland Material Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetClevelandCableDataBindings()
        Try

            'Setting up the data variables
            TheClevelandCableDataTier = New InventoryProjectDataTier
            TheClevelandCableDataSet = TheClevelandCableDataTier.GetClevelandCableInformation
            TheClevelandCableBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheClevelandCableBindingSource
                .DataSource = TheClevelandCableDataSet
                .DataMember = "clevelandcable"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheClevelandCableBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheClevelandCableBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheClevelandCableBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheClevelandCableBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheClevelandCableBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheClevelandCableBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheClevelandCableBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheClevelandCableBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Cleveland Cable Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetElmiraMaterialDataBindings()
        Try

            'Setting up the data variables
            TheElmiraMaterialDataTier = New InventoryProjectDataTier
            TheElmiraMaterialDataSet = TheElmiraMaterialDataTier.GetElmiraMaterialInformation
            TheElmiraMaterialBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheElmiraMaterialBindingSource
                .DataSource = TheElmiraMaterialDataSet
                .DataMember = "elmiramaterial"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheElmiraMaterialBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheElmiraMaterialBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheElmiraMaterialBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheElmiraMaterialBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheElmiraMaterialBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheElmiraMaterialBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheElmiraMaterialBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheElmiraMaterialBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Cleveland Material Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetElmiraCableDataBindings()
        Try

            'Setting up the data variables
            TheElmiraCableDataTier = New InventoryProjectDataTier
            TheElmiraCableDataSet = TheElmiraCableDataTier.GetElmiraCableInformation
            TheElmiraCableBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheElmiraCableBindingSource
                .DataSource = TheElmiraCableDataSet
                .DataMember = "elmiracable"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheElmiraCableBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheElmiraCableBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheElmiraCableBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheElmiraCableBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheElmiraCableBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheElmiraCableBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheElmiraCableBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheElmiraCableBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Cleveland Cable Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetJamestownMaterialDataBindings()
        Try

            'Setting up the data variables
            TheJamestownMaterialDataTier = New InventoryProjectDataTier
            TheJamestownMaterialDataSet = TheJamestownMaterialDataTier.GetJamestownMaterialInformation
            TheJamestownMaterialBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheJamestownMaterialBindingSource
                .DataSource = TheJamestownMaterialDataSet
                .DataMember = "jamestownmaterial"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheJamestownMaterialBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheJamestownMaterialBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheJamestownMaterialBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheJamestownMaterialBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheJamestownMaterialBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheJamestownMaterialBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheJamestownMaterialBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheJamestownMaterialBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Cleveland Material Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetJamestownCableDataBindings()
        Try

            'Setting up the data variables
            TheJamestownCableDataTier = New InventoryProjectDataTier
            TheJamestownCableDataSet = TheJamestownCableDataTier.GetJamestownCableInformation
            TheJamestownCableBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheJamestownCableBindingSource
                .DataSource = TheJamestownCableDataSet
                .DataMember = "jamestowncable"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheJamestownCableBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheJamestownCableBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheJamestownCableBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheJamestownCableBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheJamestownCableBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheJamestownCableBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheJamestownCableBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheJamestownCableBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Cleveland Cable Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetMilwaukeeMaterialDataBindings()
        Try

            'Setting up the data variables
            TheMIlwaukeeMaterialDataTier = New InventoryProjectDataTier
            TheMilwaukeeMaterialDataSet = TheMIlwaukeeMaterialDataTier.GetMilwaukeeMaterialInformation
            TheMIlwaukeeMaterialBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheMIlwaukeeMaterialBindingSource
                .DataSource = TheMilwaukeeMaterialDataSet
                .DataMember = "milwaukeematerial"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheMIlwaukeeMaterialBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheMIlwaukeeMaterialBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheMIlwaukeeMaterialBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheMIlwaukeeMaterialBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheMIlwaukeeMaterialBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheMIlwaukeeMaterialBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheMIlwaukeeMaterialBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheMIlwaukeeMaterialBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Milwaukee Material Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetMilwaukeeCableDataBindings()
        Try

            'Setting up the data variables
            TheMilwaukeeCableDataTier = New InventoryProjectDataTier
            TheMilwaukeeCableDataSet = TheMilwaukeeCableDataTier.GetMilwaukeeCableInformation
            TheMilwaukeeCableBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheMilwaukeeCableBindingSource
                .DataSource = TheMilwaukeeCableDataSet
                .DataMember = "milwaukeecable"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheMilwaukeeCableBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheMilwaukeeCableBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheMilwaukeeCableBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheMilwaukeeCableBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheMilwaukeeCableBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheMilwaukeeCableBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheMilwaukeeCableBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheMilwaukeeCableBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Milwaukee Cable Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetSyracuseMaterialDataBindings()
        Try

            'Setting up the data variables
            TheSyracuseMaterialDataTier = New InventoryProjectDataTier
            TheSyracuseMaterialDataSet = TheSyracuseMaterialDataTier.GetSyracuseMaterialInformation
            TheSyracuseMaterialBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheSyracuseMaterialBindingSource
                .DataSource = TheSyracuseMaterialDataSet
                .DataMember = "syracusematerial"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheSyracuseMaterialBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheSyracuseMaterialBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheSyracuseMaterialBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheSyracuseMaterialBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheSyracuseMaterialBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheSyracuseMaterialBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheSyracuseMaterialBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheSyracuseMaterialBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Milwaukee Material Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub SetSyracuseCableDataBindings()
        Try

            'Setting up the data variables
            TheSyracuseCableDataTier = New InventoryProjectDataTier
            TheSyracuseCableDataSet = TheSyracuseCableDataTier.GetSyracuseCableInformation
            TheSyracuseCableBindingSource = New BindingSource

            'Setting up the bindingsource
            With TheSyracuseCableBindingSource
                .DataSource = TheSyracuseCableDataSet
                .DataMember = "syracusecable"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo box
            With cboTransactionID
                .DataSource = TheSyracuseCableBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheSyracuseCableBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'setting the rest of the controls
            txtTransactionPartNumber.DataBindings.Add("text", TheSyracuseCableBindingSource, "PartNumber")
            txtTransactionCost.DataBindings.Add("text", TheSyracuseCableBindingSource, "Cost")
            txtTransactionQuantity.DataBindings.Add("Text", TheSyracuseCableBindingSource, "Quantity")
            txtTransactionTotal.DataBindings.Add("text", TheSyracuseCableBindingSource, "Total")
            txtTransactionDescription.DataBindings.Add("text", TheSyracuseCableBindingSource, "Description")
            txtTransactionWarehouseID.DataBindings.Add("Text", TheSyracuseCableBindingSource, "WarehouseID")

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check Milwaukee Cable Databindings", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        Dim strOfficeType As String
        Dim strOfficeCity As String

        'This routine will compute the report
        'Beginning Cleveland materials
        ClearTransactionDataBindings()
        SetClevelandMaterialDataBindings()
        strOfficeType = "CLEVELAND MATERIAL"
        strOfficeCity = "CLEVELAND"
        mstrInventoryType = "MATERIAL"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Cleveland Cable
        ClearTransactionDataBindings()
        SetClevelandCableDataBindings()
        strOfficeType = "CLEVELAND CABLE"
        strOfficeCity = "CLEVELAND"
        mstrInventoryType = "CABLE"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Elmira Materials
        ClearTransactionDataBindings()
        SetElmiraMaterialDataBindings()
        strOfficeType = "KENTUCKY MATERIAL"
        strOfficeCity = "KENTUCKY"
        mstrInventoryType = "MATERIAL"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Elimra Cable
        ClearTransactionDataBindings()
        SetElmiraCableDataBindings()
        strOfficeType = "KENTUCKY CABLE"
        strOfficeCity = "KENTUCKY"
        mstrInventoryType = "CABLE"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Jamestown Materials
        ClearTransactionDataBindings()
        SetJamestownMaterialDataBindings()
        strOfficeType = "JAMESTOWN MATERIAL"
        strOfficeCity = "JAMESTOWN"
        mstrInventoryType = "MATERIAL"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Jamestown Cable
        ClearTransactionDataBindings()
        SetJamestownCableDataBindings()
        strOfficeType = "JAMESTOWN CABLE"
        strOfficeCity = "JAMESTOWN"
        mstrInventoryType = "CABLE"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Milwaukee Materials
        ClearTransactionDataBindings()
        SetMilwaukeeMaterialDataBindings()
        strOfficeType = "MILWAUKEE MATERIAL"
        strOfficeCity = "MILWAUKEE"
        mstrInventoryType = "MATERIAL"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Milwaukee Cable
        ClearTransactionDataBindings()
        SetMilwaukeeCableDataBindings()
        strOfficeType = "MILWAUKEE CABLE"
        strOfficeCity = "MILWAUKEE"
        mstrInventoryType = "CABLE"
        GetValuation(strOfficeType, strOfficeCity)

        'Beginning Syracuse Materials
        'ClearTransactionDataBindings()
        'SetSyracuseMaterialDataBindings()
        'strOfficeType = "SYRACUSE MATERIAL"
        'strOfficeCity = "SYRACUSE"
        'mstrInventoryType = "MATERIAL"
        'GetValuation(strOfficeType, strOfficeCity)

        'Beginning Milwaukee Cable
        'ClearTransactionDataBindings()
        'SetSyracuseCableDataBindings()
        'strOfficeType = "SYRACUSE CABLE"
        'strOfficeCity = "SYRACUSE"
        'mstrInventoryType = "CABLE"
        'GetValuation(strOfficeType, strOfficeCity)



        If PrintDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            PrintDocument1.PrinterSettings = PrintDialog1.PrinterSettings
            BJCPrintDocument.PrinterSettings = PrintDialog1.PrinterSettings
        End If

        'Printing document
        'PrintDocument1.Print()
        BJCPrintDocument.Print()


    End Sub
    Private Sub GetValuation(ByVal strOfficeType As String, ByVal strCityLocation As String)

        'Setting local variables
        Dim intTransactionCounter As Integer
        Dim intTransactionNumberOfRecords As Integer
        Dim intTransactionSelectedIndex As Integer
        Dim intStructureCounter As Integer
        Dim douStructurePrice As Double
        Dim strPartNumberForSearch As String
        Dim strPartNumberFromTable As String
        Dim strPartDescriptionForSearch As String
        Dim strPartDescriptionFromTable As String
        Dim intTransactionQuantity As Integer
        Dim intOfficeID As Integer
        Dim blnItemNotFound As Boolean
        Dim douQuantity As Double

        'Getting ready for Loop
        intTransactionNumberOfRecords = cboTransactionID.Items.Count - 1
        mdouOfficeValuation = 0.0

        'Performing Loop
        For intTransactionCounter = 0 To intTransactionNumberOfRecords

            'setting boolean variable
            blnItemNotFound = True

            'incrementing the combo box
            cboTransactionID.SelectedIndex = intTransactionCounter

            'Getting the part number
            strPartNumberForSearch = txtTransactionPartNumber.Text

            'Getting the part description
            strPartDescriptionForSearch = txtTransactionDescription.Text

            'performing loop of structure
            For intStructureCounter = 0 To mintPartUpperLimit

                'loading variable
                strPartNumberFromTable = structPartValuation(intStructureCounter).mstrPartNumber
                strPartDescriptionFromTable = structPartValuation(intStructureCounter).mstrPartDescription

                If strPartNumberForSearch = strPartNumberFromTable Then
                    blnItemNotFound = False
                    intTransactionSelectedIndex = intTransactionCounter
                    douStructurePrice = CDbl(structPartValuation(intStructureCounter).mdouPartPrice)
                End If

                If blnItemNotFound = True Then
                    If strPartDescriptionForSearch <> "" Then
                        If strPartDescriptionForSearch = strPartDescriptionFromTable Then
                            blnItemNotFound = False
                            intTransactionSelectedIndex = intTransactionCounter
                            douStructurePrice = CDbl(structPartValuation(intStructureCounter).mdouPartPrice)
                        End If
                    End If
                End If


            Next

            'Adding the valuation
            douQuantity = CDbl(txtTransactionQuantity.Text)
            mdouOfficeValuation = mdouOfficeValuation + (douStructurePrice + douQuantity)

            txtTransactionCost.Text = CStr(douStructurePrice)
            txtTransactionTotal.Text = CStr(douStructurePrice * douQuantity)

            'Updating the valuation
            If strOfficeType = "CLEVELAND MATERIAL" Then
                UpdateClevelandMaterialsTable()
            ElseIf strOfficeType = "CLEVELAND CABLE" Then
                UpdateClevelandCableTable()
            ElseIf strOfficeType = "KENTUCKY MATERIAL" Then
                UpdateElmiraMaterialsTable()
            ElseIf strOfficeType = "KENTUCKY CABLE" Then
                UpdateElmiraCableTable()
            ElseIf strOfficeType = "JAMESTOWN MATERIAL" Then
                UpdateJamestownMaterialsTable()
            ElseIf strOfficeType = "JAMESTOWN CABLE" Then
                UpdateJamestownCableTable()
            ElseIf strOfficeType = "MILWAUKEE MATERIAL" Then
                UpdateMilwaukeeMaterialsTable()
            ElseIf strOfficeType = "MILWAUKEE CABLE" Then
                UpdateMilwaukeeCableTable()
            End If

        Next

        'Updating the record
        UpdateOfficeValuation(strCityLocation)

    End Sub
    Private Sub UpdateClevelandMaterialsTable()
        TheClevelandMaterialBindingSource.EndEdit()
        TheClevelandMaterialDataTier.UpdateClevelandMaterialDB(TheClevelandMaterialDataSet)
    End Sub
    Private Sub UpdateClevelandCableTable()
        TheClevelandCableBindingSource.EndEdit()
        TheClevelandCableDataTier.UpdateClevelandCableDB(TheClevelandCableDataSet)
    End Sub
    Private Sub UpdateElmiraMaterialsTable()
        TheElmiraMaterialBindingSource.EndEdit()
        TheElmiraMaterialDataTier.UpdateElmiraMaterialDB(TheElmiraMaterialDataSet)
    End Sub
    Private Sub UpdateElmiraCableTable()
        TheElmiraCableBindingSource.EndEdit()
        TheElmiraCableDataTier.UpdateElmiraCableDB(TheElmiraCableDataSet)
    End Sub
    Private Sub UpdateJamestownMaterialsTable()
        TheJamestownMaterialBindingSource.EndEdit()
        TheJamestownMaterialDataTier.UpdateJamestownMaterialDB(TheJamestownMaterialDataSet)
    End Sub
    Private Sub UpdateJamestownCableTable()
        TheJamestownCableBindingSource.EndEdit()
        TheJamestownCableDataTier.UpdateJamestownCableDB(TheJamestownCableDataSet)
    End Sub
    Private Sub UpdateMilwaukeeMaterialsTable()
        TheMIlwaukeeMaterialBindingSource.EndEdit()
        TheMIlwaukeeMaterialDataTier.UpdateMilwaukeeMaterialDB(TheMilwaukeeMaterialDataSet)
    End Sub
    Private Sub UpdateMilwaukeeCableTable()
        TheMilwaukeeCableBindingSource.EndEdit()
        TheMilwaukeeCableDataTier.UpdateMilwaukeeCableDB(TheMilwaukeeCableDataSet)
    End Sub
    Private Sub UpdateSyracuseMaterialsTable()
        TheSyracuseMaterialBindingSource.EndEdit()
        TheSyracuseMaterialDataTier.UpdateSyracuseMaterialDB(TheSyracuseMaterialDataSet)
    End Sub
    Private Sub UpdateSyracuseCableTable()
        TheSyracuseCableBindingSource.EndEdit()
        TheSyracuseCableDataTier.UpdateSyracuseCableDB(TheSyracuseCableDataSet)
    End Sub
    Private Sub UpdateOfficeValuation(ByVal strOfficeLocation As String)

        'Setting Local Variables
        Dim intOfficeCounter As Integer
        Dim intTransactionCounter As Integer
        Dim intTransactionNumberOfRecords As Integer
        Dim blnLocationFound As Boolean
        Dim intOfficeIDForSearch As Integer
        Dim intOfficeIDFromTable As Integer
        Dim strWarehouseType As String
        Dim douItemTotal As Double
        Dim douWarehouseTotal As Double

        'Setting up for the loop
        intTransactionNumberOfRecords = cboTransactionID.Items.Count - 1

        'Beginning first loop
        For intOfficeCounter = 0 To mintOfficeUpperLimit

            'setting initial value
            douWarehouseTotal = 0

            'Setting the combo box
            cboEmployeeID.SelectedIndex = mintOfficeSelectedIndex(intOfficeCounter)

            'Getting the warehouse type
            strWarehouseType = txtEmployeeFirstName.Text

            'Calling Keyword class
            blnLocationFound = TheKeywordSearch.FindKeyWord(strOfficeLocation, strWarehouseType)

            If blnLocationFound = False Then

                'This will place the office ID into a variable
                intOfficeIDForSearch = CInt(cboEmployeeID.Text)

                'Beginning Transaction Loop
                For intTransactionCounter = 0 To intTransactionNumberOfRecords

                    'Incrementing the combo box
                    cboTransactionID.SelectedIndex = intTransactionCounter

                    'getting id
                    intOfficeIDFromTable = CInt(txtTransactionWarehouseID.Text)

                    'Getting ready to the math
                    If intOfficeIDForSearch = intOfficeIDFromTable Then

                        douItemTotal = CDbl(txtTransactionTotal.Text)
                        douWarehouseTotal = douWarehouseTotal + douItemTotal

                    End If
                Next

                'Saving information
                With TheOfficeValuationBindingSource
                    .EndEdit()
                    .AddNew()
                End With

                addingBoolean = True
                SetComboBoxBinding()
                CreateID.Show()
                cboValueOfficeID.Text = CStr(Logon.mintCreatedTransactionID)
                txtValue.Text = CStr(douWarehouseTotal)
                txtValueOfficeID.Text = CStr(intOfficeIDForSearch)
                mstrLogDate = CStr(mdatLogDate)
                txtValueDate.Text = mstrLogDate
                txtValueInventoryType.Text = mstrInventoryType

                'saving the information
                Try

                    'saving the data
                    TheOfficeValuationBindingSource.EndEdit()
                    TheOfficeValuationDataTier.UpdateOfficeValuationDB(TheOfficeValuationDataSet)
                    addingBoolean = False
                    editingBoolean = False
                    SetComboBoxBinding()

                Catch ex As Exception

                    'message to the user
                    MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

            End If
        Next

    End Sub
    

    Private Sub btnMainMenu_Click(sender As Object, e As EventArgs) Handles btnMainMenu.Click

        'This will load the main menu
        LastTransaction.Show()
        Form1.Show()
        Me.Close()

    End Sub

    Private Sub PrintDocument1_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage

        'Setting local variables
        Dim intOfficeCounter As Integer
        Dim intOfficeNumberOfRecords As Integer
        Dim intOfficeIDForSearch As Integer
        Dim intOfficeIDFromTable As Integer
        Dim strDateForSearch As String
        Dim strDateFromTable As String
        Dim intValuationCounter As Integer
        Dim intValuationNumberOfRecords As Integer
        Dim decValueAmount As Decimal
        Dim decValueTotal As Decimal = 0
        Dim strValueAmount As String
        Dim strValueTotal As String

        'Setting up variables for the reports
        Dim PrintHeaderFont As New Font("Arial", 18, FontStyle.Bold)
        Dim PrintSubHeaderFont As New Font("Arial", 14, FontStyle.Bold)
        Dim PrintItemsFont As New Font("Arial", 10, FontStyle.Regular)
        Dim PrintX As Single = e.MarginBounds.Left
        Dim PrintY As Single = e.MarginBounds.Top
        Dim HeadingLineHeight As Single = PrintHeaderFont.GetHeight + 18
        Dim ItemLineHeight As Single = PrintItemsFont.GetHeight + 10

        'Setting up for default position
        PrintY = 100

        'Setting up for the print header
        PrintX = 150
        e.Graphics.DrawString("Blue Jay Communications Inventory Report", PrintHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight
        PrintX = 140
        e.Graphics.DrawString("Complete Inventory Valuation For:  " + mstrLogDate, PrintSubHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

        'Setting up the columns
        PrintX = 100
        e.Graphics.DrawString("Office Location", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintX = 500
        e.Graphics.DrawString("Inventory Valuation", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

        'Setting up for Loop
        intOfficeNumberOfRecords = cboEmployeeID.Items.Count - 1
        intValuationNumberOfRecords = cboValueOfficeID.Items.Count - 1
        strDateForSearch = mstrLogDate

        'Beginning for loop
        For intValuationCounter = 0 To intValuationNumberOfRecords

            'Incrementing the combo box
            cboValueOfficeID.SelectedIndex = intValuationCounter

            'Getting the date
            strDateFromTable = txtValueDate.Text

            'Checking Date
            If strDateForSearch = strDateFromTable Then

                'Getting office ID
                intOfficeIDForSearch = CInt(txtValueOfficeID.Text)

                'Beginning second loop
                For intOfficeCounter = 0 To intOfficeNumberOfRecords

                    'Incrementing the counter
                    cboEmployeeID.SelectedIndex = intOfficeCounter

                    'Getting the office id
                    intOfficeIDFromTable = CInt(cboEmployeeID.Text)

                    'If Statements to generate the report
                    If intOfficeIDForSearch = intOfficeIDFromTable Then

                        'Getting the value 
                        decValueAmount = CDec(txtValue.Text)
                        strValueAmount = Format(decValueAmount, "Currency")
                        decValueTotal = decValueTotal + decValueAmount

                        PrintX = 100
                        e.Graphics.DrawString(txtEmployeeFirstName.Text + " " + txtValueInventoryType.Text, PrintItemsFont, Brushes.Black, PrintX, PrintY)
                        PrintX = 500
                        e.Graphics.DrawString(strValueAmount, PrintItemsFont, Brushes.Black, PrintX, PrintY)
                        PrintY = PrintY + ItemLineHeight

                    End If
                Next

            End If
        Next

        'Setting up the total
        strValueTotal = Format(decValueTotal, "Currency")
        PrintY = PrintY + HeadingLineHeight
        PrintY = PrintY + HeadingLineHeight
        PrintX = 100
        e.Graphics.DrawString("Total Inventory Value", PrintHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintX = 500
        e.Graphics.DrawString(strValueTotal, PrintHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

    End Sub

    Private Sub BJCPrintDocument_PrintPage(sender As Object, e As Printing.PrintPageEventArgs) Handles BJCPrintDocument.PrintPage

        'Setting local variables
        Dim intOfficeCounter As Integer
        Dim intOfficeNumberOfRecords As Integer
        Dim intOfficeIDForSearch As Integer
        Dim intOfficeIDFromTable As Integer
        Dim strDateForSearch As String
        Dim strDateFromTable As String
        Dim intValuationCounter As Integer
        Dim intValuationNumberOfRecords As Integer
        Dim decValueAmount As Decimal
        Dim decValueTotal As Decimal = 0
        Dim strValueAmount As String
        Dim strValueTotal As String

        'Setting up variables for the reports
        Dim PrintHeaderFont As New Font("Arial", 18, FontStyle.Bold)
        Dim PrintSubHeaderFont As New Font("Arial", 14, FontStyle.Bold)
        Dim PrintItemsFont As New Font("Arial", 10, FontStyle.Regular)
        Dim PrintX As Single = e.MarginBounds.Left
        Dim PrintY As Single = e.MarginBounds.Top
        Dim HeadingLineHeight As Single = PrintHeaderFont.GetHeight + 18
        Dim ItemLineHeight As Single = PrintItemsFont.GetHeight + 10
        Dim blnItemNotFound As Boolean
        Dim strKeywordTextForComparison As String
        Dim strWarehouseType As String

        'This will allow the Key Word Search to function

        'Setting up for default position
        PrintY = 100

        'Setting up for the print header
        PrintX = 150
        e.Graphics.DrawString("Blue Jay Communications Inventory Report", PrintHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight
        PrintX = 140
        e.Graphics.DrawString("Complete Inventory Valuation For:  " + mstrLogDate, PrintSubHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

        'Setting up the columns
        PrintX = 100
        e.Graphics.DrawString("Office Location", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintX = 500
        e.Graphics.DrawString("Inventory Valuation", PrintItemsFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

        'Setting up for Loop
        intOfficeNumberOfRecords = cboEmployeeID.Items.Count - 1
        intValuationNumberOfRecords = cboValueOfficeID.Items.Count - 1
        strDateForSearch = mstrLogDate

        'Beginning for loop
        For intValuationCounter = 0 To intValuationNumberOfRecords

            'Incrementing the combo box
            cboValueOfficeID.SelectedIndex = intValuationCounter

            'Getting the date
            strDateFromTable = txtValueDate.Text

            'Checking Date
            If strDateForSearch = strDateFromTable Then

                'Getting office ID
                intOfficeIDForSearch = CInt(txtValueOfficeID.Text)

                'Beginning second loop
                For intOfficeCounter = 0 To intOfficeNumberOfRecords

                    blnItemNotFound = True

                    'Incrementing the counter
                    cboEmployeeID.SelectedIndex = intOfficeCounter

                    'Getting the office id
                    intOfficeIDFromTable = CInt(cboEmployeeID.Text)

                    'If Statements to generate the report
                    If intOfficeIDForSearch = intOfficeIDFromTable Then

                        'Getting ready to call the key word search
                        strKeywordTextForComparison = "JH"
                        strWarehouseType = txtEmployeeFirstName.Text

                        blnItemNotFound = TheKeywordSearch.FindKeyWord(strKeywordTextForComparison, strWarehouseType)

                        If blnItemNotFound = False Then

                            'Getting the value 
                            decValueAmount = CDec(txtValue.Text)
                            strValueAmount = Format(decValueAmount, "Currency")
                            decValueTotal = decValueTotal + decValueAmount

                            PrintX = 100
                            e.Graphics.DrawString(txtEmployeeFirstName.Text + " " + txtValueInventoryType.Text, PrintItemsFont, Brushes.Black, PrintX, PrintY)
                            PrintX = 500
                            e.Graphics.DrawString(strValueAmount, PrintItemsFont, Brushes.Black, PrintX, PrintY)
                            PrintY = PrintY + ItemLineHeight

                        End If
                    End If
                Next

            End If
        Next

        'Setting up the total
        strValueTotal = Format(decValueTotal, "Currency")
        PrintY = PrintY + HeadingLineHeight
        PrintY = PrintY + HeadingLineHeight
        PrintX = 100
        e.Graphics.DrawString("Total Inventory Value", PrintHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintX = 500
        e.Graphics.DrawString(strValueTotal, PrintHeaderFont, Brushes.Black, PrintX, PrintY)
        PrintY = PrintY + HeadingLineHeight

    End Sub
End Class
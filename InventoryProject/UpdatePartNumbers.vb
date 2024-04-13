'Title:         Update Part Numbers
'Date:          4-8-15
'Author:        Terry Holmes

'Description:   This will update the part number and cost

Option Strict On

Public Class UpdatePartNumbers

    'Setting up the global variables
    Private ThePartNumberDataSet As PartNumberDataSet
    Private ThePartNumberDataTier As PartNumberDataTier
    Private WithEvents ThePartNumberBindingSource As BindingSource

    Private TheCostEstimatorPartNumbersDataSet As CostEstimatorPartNumbersDataSet
    Private TheCostEstimatorPartNumbersDataTier As InventoryProjectDataTier
    Private WithEvents TheCostEstimatorPartNumbersBindingSource As BindingSource

    'Setting up the Part Structure
    Structure PartNumbers
        Dim mintPartID As Integer
        Dim mstrPartNumber As String
        Dim mstrDescription As String
        Dim mdouPrice As Double
    End Structure

    Dim structPartNumbers() As PartNumbers
    Dim mintPartCounter As Integer
    Dim mintPartUpperLimit As Integer

    'Setting up structure for new part numbers
    Dim mstrNewPartNumber() As String
    Dim mintNewPartCounter As Integer
    Dim mintNewPartUpperLimit As Integer

    Private addingBoolean As Boolean = False
    Private editingBoolean As Boolean = False
    Private previousSelectedIndex As Integer

    Private Sub btnMainMenu_Click(sender As Object, e As EventArgs) Handles btnMainMenu.Click

        'This will show the main menu
        LastTransaction.Show()
        Form1.Show()
        Me.Close()

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        'This will close the program
        CloseProgram.ShowDialog()

    End Sub

    Private Sub UpdatePartNumbers_Load(sender As Object, e As EventArgs) Handles MyBase.Load


        'Loading up the variables
        Dim intCounter As Integer
        Dim intNumberOfRecords As Integer
        Dim strValueForValidation As String

        Try

            ThePartNumberDataTier = New PartNumberDataTier
            ThePartNumberDataSet = ThePartNumberDataTier.GetPartNumberInformation
            ThePartNumberBindingSource = New BindingSource

            TheCostEstimatorPartNumbersDataTier = New InventoryProjectDataTier
            TheCostEstimatorPartNumbersDataSet = TheCostEstimatorPartNumbersDataTier.GetCostEstimatorPartNumbersInformation
            TheCostEstimatorPartNumbersBindingSource = New BindingSource

            'setting up the binding sources
            With ThePartNumberBindingSource
                .DataSource = ThePartNumberDataSet
                .DataMember = "partnumbers"
                .MoveFirst()
                .MoveLast()
            End With

            With TheCostEstimatorPartNumbersBindingSource
                .DataSource = TheCostEstimatorPartNumbersDataSet
                .DataMember = "costestimatorpartnumbers"
                .MoveFirst()
                .MoveLast()
            End With

            'Setting up the combo boxes
            With cboPartID
                .DataSource = ThePartNumberBindingSource
                .DisplayMember = "PartID"
                .DataBindings.Add("text", ThePartNumberBindingSource, "PartID", False, DataSourceUpdateMode.Never)
            End With

            With cboCostPartID
                .DataSource = TheCostEstimatorPartNumbersBindingSource
                .DisplayMember = "PartID"
                .DataBindings.Add("text", TheCostEstimatorPartNumbersBindingSource, "PartID", False, DataSourceUpdateMode.Never)
            End With

            'Setting the rest of the controls
            txtCost.DataBindings.Add("Text", TheCostEstimatorPartNumbersBindingSource, "Cost")
            txtCostDescription.DataBindings.Add("text", TheCostEstimatorPartNumbersBindingSource, "Description")
            txtCostPartNumber.DataBindings.Add("text", TheCostEstimatorPartNumbersBindingSource, "PartNumber")
            txtPartDescription.DataBindings.Add("text", ThePartNumberBindingSource, "Description")
            txtPartNumber.DataBindings.Add("text", ThePartNumberBindingSource, "PartNumber")
            txtPartPrice.DataBindings.Add("text", ThePartNumberBindingSource, "Price")

            'Loading the structure
            intNumberOfRecords = cboPartID.Items.Count - 1
            ReDim structPartNumbers(intNumberOfRecords)
            mintPartCounter = 0

            'beginning loop
            For intCounter = 0 To intNumberOfRecords

                'incrementing the combo box
                cboPartID.SelectedIndex = intCounter

                'Getting price
                strValueForValidation = txtPartPrice.Text

                If IsNumeric(strValueForValidation) Then

                    structPartNumbers(intCounter).mintPartID = CInt(cboPartID.Text)
                    structPartNumbers(intCounter).mstrDescription = txtPartDescription.Text
                    structPartNumbers(intCounter).mstrPartNumber = txtPartNumber.Text
                    structPartNumbers(intCounter).mdouPrice = CDbl(txtPartPrice.Text)
                    mintPartCounter += 1

                End If
            Next

            'Setting up the upper limit
            mintPartUpperLimit = mintPartCounter - 1
            mintPartCounter = 0

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        'Setting up local variables
        Dim intCostCounter As Integer
        Dim intCostUpperLimit As Integer
        Dim strPartNumberForSearch As String
        Dim strPartNumberFromTable As String
        Dim intPartStructureCounter As Integer
        Dim intPartArrayCounter As Integer
        Dim blnItemNotFound As Boolean
        Dim blnArrayNotSet As Boolean = True


        'Setting up for the loop
        intCostUpperLimit = cboCostPartID.Items.Count - 1
        mintNewPartCounter = 0
        ReDim mstrNewPartNumber(intCostUpperLimit)
        mintNewPartUpperLimit = 0

        'Beginning for loop
        For intCostCounter = 0 To intCostUpperLimit

            'setting boolean variable
            blnItemNotFound = True

            'incrementing the combo counter
            cboCostPartID.SelectedIndex = intCostCounter

            'Getting part number
            strPartNumberForSearch = txtCostPartNumber.Text

            'Beginning second loop
            For intPartStructureCounter = 0 To mintPartUpperLimit

                'getting part number
                strPartNumberFromTable = structPartNumbers(intPartStructureCounter).mstrPartNumber

                If strPartNumberForSearch = strPartNumberFromTable Then

                    'Setting boolean variable
                    blnItemNotFound = False

                End If
            Next

            'If statement to check the array
            If blnItemNotFound = True Then

                If blnArrayNotSet = False Then
                    For intPartArrayCounter = 0 To mintNewPartUpperLimit
                        strPartNumberFromTable = mstrNewPartNumber(intPartArrayCounter)
                        If strPartNumberForSearch = strPartNumberFromTable Then

                            'Setting boolean variable
                            blnItemNotFound = False

                        End If
                    Next
                End If

            End If

            If blnItemNotFound = True Then

                'Adding the new record
                With ThePartNumberBindingSource
                    .EndEdit()
                    .AddNew()
                End With

                'Setting variables
                addingBoolean = True
                SetComboBoxBinding()
                CreatePartID.Show()
                cboPartID.Text = CStr(Logon.mintPartID)
                txtPartDescription.Text = txtCostDescription.Text
                txtPartNumber.Text = txtCostPartNumber.Text
                txtPartPrice.Text = txtCost.Text
                mstrNewPartNumber(mintNewPartUpperLimit) = txtCostPartNumber.Text
                If blnArrayNotSet = False Then
                    mintNewPartUpperLimit += 1
                End If
                blnArrayNotSet = False

                Try
                    ThePartNumberBindingSource.EndEdit()
                    ThePartNumberDataTier.UpdatePartNumberDB(ThePartNumberDataSet)
                    addingBoolean = False
                    SetComboBoxBinding()
                Catch ex As Exception
                    MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try

            End If

        Next

    End Sub
    Private Sub SetComboBoxBinding()
        'This will set the combo box bindings
        With cboPartID
            If (addingBoolean Or editingBoolean) Then
                .DataBindings!text.DataSourceUpdateMode = DataSourceUpdateMode.OnValidation
                .DropDownStyle = ComboBoxStyle.Simple
            Else
                .DataBindings!text.DataSourceUpdateMode = DataSourceUpdateMode.Never
                .DropDownStyle = ComboBoxStyle.DropDownList
            End If
        End With
    End Sub
End Class
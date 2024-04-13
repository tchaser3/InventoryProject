'Title:         Create ID
'Date:          4-8-15
'Author:        Terry Holmes

'Description:   This will create a new id

Option Strict On

Public Class CreateID

    'setting up the data variables
    Private TheCreateIDDataSet As CreateIDDataSet
    Private TheCreateIDDataTier As InventoryProjectDataTier
    Private WithEvents TheCreateIDBindingSource As BindingSource

    Private Sub CreateID_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Setting local variables
        Dim intNewID As Integer

        Try

            'Setting data variable
            TheCreateIDDataTier = New InventoryProjectDataTier
            TheCreateIDDataSet = TheCreateIDDataTier.GetCreateIDInformation
            TheCreateIDBindingSource = New BindingSource

            'Setting up the binding source
            With TheCreateIDBindingSource
                .DataSource = TheCreateIDDataSet
                .DataMember = "createid"
                .MoveFirst()
                .MoveLast()
            End With

            'setting up the combo box
            With cboTransactionID
                .DataSource = TheCreateIDBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", TheCreateIDBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'Setting up the rest of the controls
            txtCreatedTransactionID.DataBindings.Add("text", TheCreateIDBindingSource, "CreatedTransactionID")

            'Creating the new id'
            intNewID = CInt(txtCreatedTransactionID.Text)
            intNewID += 1
            Logon.mintCreatedTransactionID = intNewID
            txtCreatedTransactionID.Text = CStr(intNewID)

            'saving the new id
            TheCreateIDBindingSource.EndEdit()
            TheCreateIDDataTier.UpdateCreateIDDB(TheCreateIDDataSet)

            Me.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
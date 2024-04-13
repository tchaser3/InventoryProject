'Title:         Create Part ID
'Date:          4-8-15
'Author:        Terry Holmes

'Description:   This will create a part id

Option Strict On

Public Class CreatePartID

    Private ThePartNumberIDDataSet As PartNumberIDDataSet
    Private ThePartNumberIDDataTier As PartNumberIDDataTier
    Private WithEvents ThePartNumberIDBindingSource As BindingSource

    Private Sub CreatePartID_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        'Setting local variable
        Dim intNewID As Integer

        Try

            'setting the data variables
            ThePartNumberIDDataTier = New PartNumberIDDataTier
            ThePartNumberIDDataSet = ThePartNumberIDDataTier.GetPartNumberIDInformation
            ThePartNumberIDBindingSource = New BindingSource

            'Setting up the bindingsource
            With ThePartNumberIDBindingSource
                .DataSource = ThePartNumberIDDataSet
                .DataMember = "PartNumberID"
                .MoveFirst()
                .MoveLast()
            End With

            'setting up the bindingsource
            With cboTransactionID
                .DataSource = ThePartNumberIDBindingSource
                .DisplayMember = "TransactionID"
                .DataBindings.Add("text", ThePartNumberIDBindingSource, "TransactionID", False, DataSourceUpdateMode.Never)
            End With

            'Setting up the rest of the controls
            txtPartID.DataBindings.Add("text", ThePartNumberIDBindingSource, "PartID")

            'Creating the new id
            intNewID = CInt(txtPartID.Text)

            intNewID += 1
            txtPartID.Text = CStr(intNewID)
            Logon.mintPartID = CInt(intNewID)

            'saving the id
            ThePartNumberIDBindingSource.EndEdit()
            ThePartNumberIDDataTier.UpdateDB(ThePartNumberIDDataSet)

            Me.Close()


        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try

    End Sub
End Class
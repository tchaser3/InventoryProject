'Title:     Part Number ID Data Tier
'Date:      7/11/13
'Author:    Terry Holmes

Option Strict On

Public Class PartNumberIDDataTier

    'Setting up modular variables
    Private aPartNumberIDTableAdapter As PartNumberIDDataSetTableAdapters.PartNumberIDTableAdapter

    Private aPartNumberIDDataSet As PartNumberIDDataSet

    Public Function GetPartNumberIDInformation() As PartNumberIDDataSet

        'Setting up the Datatier
        Try
            aPartNumberIDDataSet = New PartNumberIDDataSet
            aPartNumberIDTableAdapter = New PartNumberIDDataSetTableAdapters.PartNumberIDTableAdapter
            aPartNumberIDTableAdapter.Fill(aPartNumberIDDataSet.PartNumberID)
            Return aPartNumberIDDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aPartNumberIDDataSet
        End Try
    End Function

    Public Sub UpdateDB(ByVal aPartNumberIDDataSet As PartNumberIDDataSet)

        'This will update the database
        Try
            aPartNumberIDTableAdapter.Update(aPartNumberIDDataSet.PartNumberID)
        Catch ex As Exception

        End Try
    End Sub

End Class

'Title:         Inventory Project Data Tier
'Date:          4-7-15
'Author:        Terry Holmes

'Description:   This is the data tier for this application

Option Strict On

Public Class InventoryProjectDataTier

    'Setting up the data sources
    Private aClevelandCableDataSetTableAdapaters As ClevelandCableDataSetTableAdapters.clevelandcableTableAdapter
    Private aClevelandCableDataSet As ClevelandCableDataSet

    Private aClevelandMaterialDataSetTableAdpaters As ClevelandMaterialDataSetTableAdapters.clevelandmaterialTableAdapter
    Private aClevelandMaterialDataSet As ClevelandMaterialDataSet

    Private aCreateIDDataSetTableAdapters As CreateIDDataSetTableAdapters.createidTableAdapter
    Private aCreateIDDataSet As CreateIDDataSet

    Private aElmiraCableDataSetTableAdapters As ElmiraCableDataSetTableAdapters.elmiracableTableAdapter
    Private aElmiraCableDataSet As ElmiraCableDataSet

    Private aElmiraMaterialDataSetTableAdapters As ElmiraMaterialDataSetTableAdapters.elmiramaterialTableAdapter
    Private aElmiraMaterialDataSet As ElmiraMaterialDataSet

    Private aJamestownCableDataSetTableAdapters As JamestownCableDataSetTableAdapters.jamestowncableTableAdapter
    Private aJamestownCableDataSet As JamestownCableDataSet

    Private aJamestownMaterialDataSetTableAdapters As JamestownMaterialDataSetTableAdapters.jamestownmaterialTableAdapter
    Private aJamestownMaterialDataSet As JamestownMaterialDataSet

    Private aMilwaukeeCableDataSetTableAdapters As MilwaukeeCableDataSetTableAdapters.milwaukeecableTableAdapter
    Private aMilwaukeeCableDataSet As MilwaukeeCableDataSet

    Private aMilwaukeeMaterialDataSetTableAdapters As MilwaukeMaterialDataSetTableAdapters.milwaukeematerialTableAdapter
    Private aMilwaukeeMaterialDataSet As MilwaukeMaterialDataSet

    Private aOfficeValuationDataSetTableAdapters As OfficeValuationDataSetTableAdapters.officevaluationTableAdapter
    Private aOfficeValuationDataSet As OfficeValuationDataSet

    Private aSyracuseCableDataSetTableAdapters As SyracuseCableDataSetTableAdapters.syracusecableTableAdapter
    Private aSyracuseCableDataSet As SyracuseCableDataSet

    Private aSyracuseMaterialDataSetTableAdapters As SyracuseMaterialDataSetTableAdapters.syracusematerialTableAdapter
    Private aSyracuseMaterialDataSet As SyracuseMaterialDataSet

    Private aCostEstimatorPartNumbersDataSetTableAdapters As CostEstimatorPartNumbersDataSetTableAdapters.costestimatorpartnumbersTableAdapter
    Private aCostEstimatorPartNumbersDataSet As CostEstimatorPartNumbersDataSet

    Public Function GetJamestownCableInformation() As JamestownCableDataSet

        'Setting up the Datatier
        Try
            aJamestownCableDataSet = New JamestownCableDataSet
            aJamestownCableDataSetTableAdapters = New JamestownCableDataSetTableAdapters.jamestowncableTableAdapter
            aJamestownCableDataSetTableAdapters.Fill(aJamestownCableDataSet.jamestowncable)
            Return aJamestownCableDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aJamestownCableDataSet
        End Try
    End Function

    Public Sub UpdateJamestownCableDB(ByVal aJamestownCableDataSet As JamestownCableDataSet)

        'This will update Syracuse database
        Try
            aJamestownCableDataSetTableAdapters.Update(aJamestownCableDataSet.jamestowncable)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetJamestownMaterialInformation() As JamestownMaterialDataSet

        'Setting up the Datatier
        Try
            aJamestownMaterialDataSet = New JamestownMaterialDataSet
            aJamestownMaterialDataSetTableAdapters = New JamestownMaterialDataSetTableAdapters.jamestownmaterialTableAdapter
            aJamestownMaterialDataSetTableAdapters.Fill(aJamestownMaterialDataSet.jamestownmaterial)
            Return aJamestownMaterialDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aJamestownMaterialDataSet
        End Try
    End Function

    Public Sub UpdateJamestownMaterialDB(ByVal aJamestownMaterialDataSet As JamestownMaterialDataSet)

        'This will update the database
        Try
            aJamestownMaterialDataSetTableAdapters.Update(aJamestownMaterialDataSet.jamestownmaterial)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetCostEstimatorPartNumbersInformation() As CostEstimatorPartNumbersDataSet

        'Setting up the Datatier
        Try
            aCostEstimatorPartNumbersDataSet = New CostEstimatorPartNumbersDataSet
            aCostEstimatorPartNumbersDataSetTableAdapters = New CostEstimatorPartNumbersDataSetTableAdapters.costestimatorpartnumbersTableAdapter
            aCostEstimatorPartNumbersDataSetTableAdapters.Fill(aCostEstimatorPartNumbersDataSet.costestimatorpartnumbers)
            Return aCostEstimatorPartNumbersDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aCostEstimatorPartNumbersDataSet
        End Try
    End Function

    Public Sub UpdateCostEstimatorPartNumbersDB(ByVal aCostEstimatorPartNumbersDataSet As CostEstimatorPartNumbersDataSet)

        'This will update Syracuse database
        Try
            aCostEstimatorPartNumbersDataSetTableAdapters.Update(aCostEstimatorPartNumbersDataSet.costestimatorpartnumbers)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetSyracuseCableInformation() As SyracuseCableDataSet

        'Setting up the Datatier
        Try
            aSyracuseCableDataSet = New SyracuseCableDataSet
            aSyracuseCableDataSetTableAdapters = New SyracuseCableDataSetTableAdapters.syracusecableTableAdapter
            aSyracuseCableDataSetTableAdapters.Fill(aSyracuseCableDataSet.syracusecable)
            Return aSyracuseCableDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aSyracuseCableDataSet
        End Try
    End Function

    Public Sub UpdateSyracuseCableDB(ByVal aSyracuseCableDataSet As SyracuseCableDataSet)

        'This will update Syracuse database
        Try
            aSyracuseCableDataSetTableAdapters.Update(aSyracuseCableDataSet.syracusecable)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetSyracuseMaterialInformation() As SyracuseMaterialDataSet

        'Setting up the Datatier
        Try
            aSyracuseMaterialDataSet = New SyracuseMaterialDataSet
            aSyracuseMaterialDataSetTableAdapters = New SyracuseMaterialDataSetTableAdapters.syracusematerialTableAdapter
            aSyracuseMaterialDataSetTableAdapters.Fill(aSyracuseMaterialDataSet.syracusematerial)
            Return aSyracuseMaterialDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aSyracuseMaterialDataSet
        End Try
    End Function

    Public Sub UpdateSyracuseMaterialDB(ByVal aSyracuseMaterialDataSet As SyracuseMaterialDataSet)

        'This will update the database
        Try
            aSyracuseMaterialDataSetTableAdapters.Update(aSyracuseMaterialDataSet.syracusematerial)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetMilwaukeeCableInformation() As MilwaukeeCableDataSet

        'Setting up the Datatier
        Try
            aMilwaukeeCableDataSet = New MilwaukeeCableDataSet
            aMilwaukeeCableDataSetTableAdapters = New MilwaukeeCableDataSetTableAdapters.milwaukeecableTableAdapter
            aMilwaukeeCableDataSetTableAdapters.Fill(aMilwaukeeCableDataSet.milwaukeecable)
            Return aMilwaukeeCableDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aMilwaukeeCableDataSet
        End Try
    End Function

    Public Sub UpdateMilwaukeeCableDB(ByVal aMilwaukeeCableDataSet As MilwaukeeCableDataSet)

        'This will update the database
        Try
            aMilwaukeeCableDataSetTableAdapters.Update(aMilwaukeeCableDataSet.milwaukeecable)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetMilwaukeeMaterialInformation() As MilwaukeMaterialDataSet

        'Setting up the Datatier
        Try
            aMilwaukeeMaterialDataSet = New MilwaukeMaterialDataSet
            aMilwaukeeMaterialDataSetTableAdapters = New MilwaukeMaterialDataSetTableAdapters.milwaukeematerialTableAdapter
            aMilwaukeeMaterialDataSetTableAdapters.Fill(aMilwaukeeMaterialDataSet.milwaukeematerial)
            Return aMilwaukeeMaterialDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aMilwaukeeMaterialDataSet
        End Try
    End Function

    Public Sub UpdateMilwaukeeMaterialDB(ByVal aMilwaukeeMaterialDataSet As MilwaukeMaterialDataSet)

        'This will update the database
        Try
            aMilwaukeeMaterialDataSetTableAdapters.Update(aMilwaukeeMaterialDataSet.milwaukeematerial)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetOfficeValuationInformation() As OfficeValuationDataSet

        'Setting up the Datatier
        Try
            aOfficeValuationDataSet = New OfficeValuationDataSet
            aOfficeValuationDataSetTableAdapters = New OfficeValuationDataSetTableAdapters.officevaluationTableAdapter
            aOfficeValuationDataSetTableAdapters.Fill(aOfficeValuationDataSet.officevaluation)
            Return aOfficeValuationDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aOfficeValuationDataSet
        End Try
    End Function

    Public Sub UpdateOfficeValuationDB(ByVal aOfficeValuationDataSet As OfficeValuationDataSet)

        'This will update the database
        Try
            aOfficeValuationDataSetTableAdapters.Update(aOfficeValuationDataSet.officevaluation)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Public Function GetCreateIDInformation() As CreateIDDataSet

        'Setting up the Datatier
        Try
            aCreateIDDataSet = New CreateIDDataSet
            aCreateIDDataSetTableAdapters = New CreateIDDataSetTableAdapters.createidTableAdapter
            aCreateIDDataSetTableAdapters.Fill(aCreateIDDataSet.createid)
            Return aCreateIDDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aCreateIDDataSet
        End Try
    End Function

    Public Sub UpdateCreateIDDB(ByVal aCreateIDDataSet As CreateIDDataSet)

        'This will update the database
        Try
            aCreateIDDataSetTableAdapters.Update(aCreateIDDataSet.createid)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetElmiraCableInformation() As ElmiraCableDataSet

        'Setting up the Datatier
        Try
            aElmiraCableDataSet = New ElmiraCableDataSet
            aElmiraCableDataSetTableAdapters = New ElmiraCableDataSetTableAdapters.elmiracableTableAdapter
            aElmiraCableDataSetTableAdapters.Fill(aElmiraCableDataSet.elmiracable)
            Return aElmiraCableDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aElmiraCableDataSet
        End Try
    End Function

    Public Sub UpdateElmiraCableDB(ByVal aElmiraCableDataSet As ElmiraCableDataSet)

        'This will update the database
        Try
            aElmiraCableDataSetTableAdapters.Update(aElmiraCableDataSet.elmiracable)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetElmiraMaterialInformation() As ElmiraMaterialDataSet

        'Setting up the Datatier
        Try
            aElmiraMaterialDataSet = New ElmiraMaterialDataSet
            aElmiraMaterialDataSetTableAdapters = New ElmiraMaterialDataSetTableAdapters.elmiramaterialTableAdapter
            aElmiraMaterialDataSetTableAdapters.Fill(aElmiraMaterialDataSet.elmiramaterial)
            Return aElmiraMaterialDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aElmiraMaterialDataSet
        End Try
    End Function

    Public Sub UpdateElmiraMaterialDB(ByVal aElmiraMaterialDataSet As ElmiraMaterialDataSet)

        'This will update the database
        Try
            aElmiraMaterialDataSetTableAdapters.Update(aElmiraMaterialDataSet.elmiramaterial)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetClevelandMaterialInformation() As ClevelandMaterialDataSet

        'Setting up the Datatier
        Try
            aClevelandMaterialDataSet = New ClevelandMaterialDataSet
            aClevelandMaterialDataSetTableAdpaters = New ClevelandMaterialDataSetTableAdapters.clevelandmaterialTableAdapter
            aClevelandMaterialDataSetTableAdpaters.Fill(aClevelandMaterialDataSet.clevelandmaterial)
            Return aClevelandMaterialDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aClevelandMaterialDataSet
        End Try
    End Function

    Public Sub UpdateClevelandMaterialDB(ByVal aClevelandMaterialDataSet As ClevelandMaterialDataSet)

        'This will update the database
        Try
            aClevelandMaterialDataSetTableAdpaters.Update(aClevelandMaterialDataSet.clevelandmaterial)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Function GetClevelandCableInformation() As ClevelandCableDataSet

        'Setting up the Datatier
        Try
            aClevelandCableDataSet = New ClevelandCableDataSet
            aClevelandCableDataSetTableAdapaters = New ClevelandCableDataSetTableAdapters.clevelandcableTableAdapter
            aClevelandCableDataSetTableAdapaters.Fill(aClevelandCableDataSet.clevelandcable)
            Return aClevelandCableDataSet

        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Check", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return aClevelandCableDataSet
        End Try
    End Function

    Public Sub UpdateClevelandCableDB(ByVal aClevelandCableDataSet As ClevelandCableDataSet)

        'This will update the database
        Try
            aClevelandCableDataSetTableAdapaters.Update(aClevelandCableDataSet.clevelandcable)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Please Correct", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

End Class

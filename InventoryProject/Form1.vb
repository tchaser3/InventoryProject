'Title:         Inventory Valuation
'Date:          4-7-15
'Author:        Terry Holmes

'Description:   This Application will give a complete corporate inventory valuation

Option Strict On

Public Class Form1


    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

        'This will close the program
        CloseProgram.ShowDialog()

    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Logon.mstrLastTransactionSummary = "LOADED INVENTORY VALUATION"

    End Sub
   
    Private Sub btnProcess_Click(sender As Object, e As EventArgs) Handles btnProcess.Click

        'This will launch the Part Valuation
        LastTransaction.Show()
        ProcessValuation.Show()
        Me.Close()

    End Sub

    Private Sub btnAbout_Click(sender As Object, e As EventArgs) Handles btnAbout.Click

        'About box
        About.Show()

    End Sub

    
    Private Sub btnUpdatePartNumbers_Click(sender As Object, e As EventArgs) Handles btnUpdatePartNumbers.Click

        'This will update the part numbers
        LastTransaction.Show()
        UpdatePartNumbers.Show()
        Me.Close()

    End Sub
End Class

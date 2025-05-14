

Imports MySql.Data.MySqlClient

Public Class frmAddUpdateStaff

    Private Sub GetStaffID()
        Try
            sqL = "SELECT StaffID FROM STAFF ORDER BY StaffID DESC"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader()

            If dr.Read = True Then
                lblProductNo.Text = dr("StaffID") + 1
            Else
                lblProductNo.Text = 1
            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub AddStaff()
        Try
            sqL = "INSERT INTO STAFF(Lastname, Firstname, MI, Street, Barangay, City, Province, ContactNo, Username, Role, UPassword) VALUES('" & txtLastname.Text & "', '" & txtFirstname.Text & "', '" & txtMI.Text & "', '" & txtStreet.Text & "', '" & txtBarangay.Text & "', '" & txtCity.Text & "', '" & txtProvince.Text & "', '" & txtContractNo.Text & "', '" & txtUsername.Text & "', '" & txtRole.Text & "', '" & txtPassword.Text & "')"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            cmd.ExecuteNonQuery()
            MsgBox("New staff successfully added.", MsgBoxStyle.Information, "Add Staff")
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub


    Private Sub UpdateStaff()
        Try
            sqL = "Update STAFF SET Lastname = '" & txtLastname.Text & "', Firstname = '" & txtFirstname.Text & "', MI = '" & txtMI.Text & "', Street= '" & txtStreet.Text & "', Barangay = '" & txtBarangay.Text & "', City = '" & txtCity.Text & "', Province = '" & txtProvince.Text & "', ContactNo = '" & txtContractNo.Text & "', Username ='" & txtUsername.Text & "', Role = '" & txtRole.Text & "', UPassword = '" & txtPassword.Text & "' WHERE StaffID = '" & frmStaff.ListView1.FocusedItem.Text & "'"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            cmd.ExecuteNonQuery()
            MsgBox("Staff record successfully updated", MsgBoxStyle.Information, "Update Staff")

        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub LoadUpdateStaff()
        Try
            sqL = "SELECT * FROM STAFF WHERE StaffID = '" & frmStaff.ListView1.FocusedItem.Text & "'"
            ConnDB()
            cmd = New MySqlCommand(sqL, conn)
            dr = cmd.ExecuteReader

            If dr.Read = True Then
                lblProductNo.Text = dr("StaffID")
                txtLastname.Text = dr("lastname")
                txtFirstname.Text = dr("Firstname")
                txtMI.Text = dr("MI")
                txtStreet.Text = dr("Street")
                txtBarangay.Text = dr("barangay")
                txtCity.Text = dr("City")
                txtProvince.Text = dr("Province")
                txtContractNo.Text = dr("ContactNo")
                txtUsername.Text = dr("username")
                txtRole.Text = dr("Role")
                txtPassword.Text = dr("UPassword")
                txtConfirmPWD.Text = dr("UPassword")

            End If
        Catch ex As Exception
            MsgBox(ex.ToString)
        Finally
            cmd.Dispose()
            conn.Close()
        End Try
    End Sub

    Private Sub ClearFields()
        lblProductNo.Text = ""
        txtLastname.Text = ""
        txtFirstname.Text = ""
        txtMI.Text = ""
        txtStreet.Text = ""
        txtBarangay.Text = ""
        txtCity.Text = ""
        txtProvince.Text = ""
        txtContractNo.Text = ""
        txtUsername.Text = ""
        txtRole.Text = ""
        txtPassword.Text = ""
        txtConfirmPWD.Text = ""
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub frmAddUpdateStaff_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If frmStaff.adding = True Then
            lblTitle.Text = "Adding New Staff"
            ClearFields()
            GetStaffID()
        Else
            lblTitle.Text = "Updating Staff"
            LoadUpdateStaff()

        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If frmStaff.adding = True Then
            AddStaff()
            Me.Close()
        Else
            UpdateStaff()
            frmStaff.LoadStaffs()
            Me.Close()
        End If
    End Sub
End Class
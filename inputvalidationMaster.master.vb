Imports System.Data
Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Partial Class inputvalidationMaster
    Inherits System.Web.UI.MasterPage


    'Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

    '    Captcha1 = New MSCaptcha.CaptchaControl
    'End Sub


    ' LOGIN BUTTON
    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click


        Dim connAuthenticate As New SqlConnection
        connAuthenticate.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings("cis491ConnectionString").ConnectionString
        connAuthenticate.Open()
        Dim strSQL As String = "SELECT * FROM Students WHERE Username = '" & tbUsername.Text & "' and Password = '" & tbPassword.Text & "'"
        Dim drAuthenticate As SqlDataReader
        Dim cmdAuthenticate As New SqlCommand(strSQL, connAuthenticate)
        drAuthenticate = cmdAuthenticate.ExecuteReader()

        captchavalidator.Validate()

        If captchavalidator.IsValid Then

            Captcha1.ValidateCaptcha(txtcaptcha.Text)

            If Captcha1.UserValidated Then

                lblLogin.ForeColor = System.Drawing.Color.Green

                lblLogin.Text = "Valid Captcha!"

                reqName.Validate()

                If reqName.IsValid Then
                    reqPass.Validate()
                    If reqPass.IsValid Then
                        MessageLabel.Text = ""




                        If drAuthenticate.Read() Then
                            lblLogin.Text = "Welcome! "
                            sqlDSGradesUser.SelectCommand = "SELECT * FROM Grades WHERE StudentID = " & drAuthenticate.Item("StudentID")
                            sqlDSGradesUser.DataBind()
                            gvGradesUser.DataBind()
                            sqlDSStudentsAll.SelectCommand = "SELECT * FROM Students"
                            gvStudentsAll.DataBind()
                            sqlDSGradesAll.SelectCommand = "SELECT * FROM Grades"
                            gvGradesAll.DataBind()
                            gvGradesUser.Visible = True
                            gvStudentsAll.Visible = False
                            gvGradesAll.Visible = False
                            lblGradesUser.Visible = True
                            lblStudentsAll.Visible = False
                            lblGradesAll.Visible = False
                            lblSQLStatement.Visible = True
                            lblSQL.Text = strSQL
                        Else
                            lblLogin.Text = "Login Failed."
                        End If


                    End If

                End If

            Else
                lblLogin.ForeColor = System.Drawing.Color.Red

                lblLogin.Text = "InValid Captcha"
            End If

        Else

            lblLogin.Text = "Login Failed."



        End If










    End Sub

    ' RESET BUTTON
    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        tbUsername.Text = ""
        tbPassword.Text = ""
        lblLogin.Text = ""
        lblSQL.Text = ""
        MessageLabel.Text = ""
        MessageLabel2.Text = ""
        gvGradesUser.Visible = False
        gvStudentsAll.Visible = False
        gvGradesAll.Visible = False
        lblGradesUser.Visible = False
        lblStudentsAll.Visible = False
        lblGradesAll.Visible = False
        lblSQLStatement.Visible = False

    End Sub

    'VALIDATION
    Protected Sub SubmitButton_Click(sender As Object, e As EventArgs)

        ' Determine which button was clicked.
        Select Case (CType(sender, Button)).ID

            Case "blacklistbutton"
                MessageLabel.Text = ""
                MessageLabel2.Text = ""

                ' Validate only the controls used for the state query.
                blacklistuser.Validate()

                ' Take the appropriate action if the controls pass validation.
                If blacklistuser.IsValid Then

                    blacklistpass.Validate()
                    If blacklistpass.IsValid Then
                        MessageLabel.Text = "Thanks for being ethical."
                    End If

                End If

            Case "whitelistbutton"
                MessageLabel.Text = ""
                MessageLabel2.Text = ""

                ' Validate only the controls used for the state query.
                whitelistuser.Validate()

                ' Take the appropriate action if the controls pass validation.
                If whitelistuser.IsValid Then

                    whitelistpass.Validate()
                    If whitelistpass.IsValid Then
                        MessageLabel.Text = "Thanks for being ethical."
                    End If

                End If

            Case "sanitizationbutton"
                MessageLabel.Text = ""
                MessageLabel2.Text = ""
                MessageLabel.Text = "User (sanitized): " & CleanInput(tbUsername.Text)
                MessageLabel2.Text = "Pass (sanitized): " & CleanInput(tbPassword.Text)


            Case "htmlbutton"
                MessageLabel.Text = ""
                MessageLabel2.Text = ""
                MessageLabel.Text = "User (encoded): " & Server.HtmlEncode(tbUsername.Text) & " (Look at source code)"
                MessageLabel2.Text = "Pass (encoded): " & Server.HtmlEncode(tbPassword.Text) & " (Look at source code)"
                'literaluser.Mode = LiteralMode.Encode
                'literaluser.Text = tbUsername.Texts


        End Select

    End Sub

    'SANITIZATION
    Function CleanInput(strIn As String) As String
        ' Replace invalid characters with empty strings.
        Try
            Return Regex.Replace(strIn, "[^\w\.@-]", "")
            ' If we timeout when replacing invalid characters, 
            ' we should return String.Empty.
        Catch e As System.TimeoutException
            Return String.Empty
        End Try

    End Function





End Class





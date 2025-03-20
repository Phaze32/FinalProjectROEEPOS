<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.picGraph = New System.Windows.Forms.PictureBox
        Me.btnGo = New System.Windows.Forms.Button
        Me.txtSymbols = New System.Windows.Forms.TextBox
        Me.label1 = New System.Windows.Forms.Label
        CType(Me.picGraph, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'picGraph
        '
        Me.picGraph.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.picGraph.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.picGraph.Location = New System.Drawing.Point(12, 41)
        Me.picGraph.Name = "picGraph"
        Me.picGraph.Size = New System.Drawing.Size(379, 211)
        Me.picGraph.TabIndex = 7
        Me.picGraph.TabStop = False
        '
        'btnGo
        '
        Me.btnGo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnGo.Location = New System.Drawing.Point(347, 12)
        Me.btnGo.Name = "btnGo"
        Me.btnGo.Size = New System.Drawing.Size(44, 23)
        Me.btnGo.TabIndex = 6
        Me.btnGo.Text = "Go"
        Me.btnGo.UseVisualStyleBackColor = True
        '
        'txtSymbols
        '
        Me.txtSymbols.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSymbols.Location = New System.Drawing.Point(67, 14)
        Me.txtSymbols.Name = "txtSymbols"
        Me.txtSymbols.Size = New System.Drawing.Size(274, 20)
        Me.txtSymbols.TabIndex = 5
        Me.txtSymbols.Text = "MCI, DIS, COKE, PEP"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(12, 17)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(49, 13)
        Me.label1.TabIndex = 4
        Me.label1.Text = "Symbols:"
        '
        'Form1
        '
        Me.AcceptButton = Me.btnGo
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(403, 264)
        Me.Controls.Add(Me.picGraph)
        Me.Controls.Add(Me.btnGo)
        Me.Controls.Add(Me.txtSymbols)
        Me.Controls.Add(Me.label1)
        Me.Name = "Form1"
        Me.Text = "howto_net_graph_stock_history"
        CType(Me.picGraph, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents picGraph As System.Windows.Forms.PictureBox
    Private WithEvents btnGo As System.Windows.Forms.Button
    Private WithEvents txtSymbols As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label

End Class

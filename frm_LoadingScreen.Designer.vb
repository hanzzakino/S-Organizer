<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_LoadingScreen
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_LoadingScreen))
        Me.ProgressBar_main = New System.Windows.Forms.ProgressBar()
        Me.lbl_LOADING = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ProgressBar_main
        '
        Me.ProgressBar_main.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.ProgressBar_main.ForeColor = System.Drawing.Color.SpringGreen
        Me.ProgressBar_main.Location = New System.Drawing.Point(25, 160)
        Me.ProgressBar_main.Name = "ProgressBar_main"
        Me.ProgressBar_main.Size = New System.Drawing.Size(450, 20)
        Me.ProgressBar_main.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar_main.TabIndex = 0
        '
        'lbl_LOADING
        '
        Me.lbl_LOADING.AutoSize = True
        Me.lbl_LOADING.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbl_LOADING.ForeColor = System.Drawing.Color.MidnightBlue
        Me.lbl_LOADING.Location = New System.Drawing.Point(25, 200)
        Me.lbl_LOADING.MaximumSize = New System.Drawing.Size(400, 0)
        Me.lbl_LOADING.Name = "lbl_LOADING"
        Me.lbl_LOADING.Size = New System.Drawing.Size(81, 17)
        Me.lbl_LOADING.TabIndex = 1
        Me.lbl_LOADING.Text = "Initializing..."
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Product Sans", 36.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.ForeColor = System.Drawing.Color.MidnightBlue
        Me.Label1.Location = New System.Drawing.Point(14, 25)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(288, 61)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "S Organizer"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Khmer UI", 22.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(19, 90)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(251, 35)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "PLYMTH Software"
        '
        'frm_LoadingScreen
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightBlue
        Me.ClientSize = New System.Drawing.Size(500, 250)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.lbl_LOADING)
        Me.Controls.Add(Me.ProgressBar_main)
        Me.Cursor = System.Windows.Forms.Cursors.WaitCursor
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frm_LoadingScreen"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "frm_LoadingScreen"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ProgressBar_main As System.Windows.Forms.ProgressBar
    Friend WithEvents lbl_LOADING As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class

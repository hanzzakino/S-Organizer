Public Class frm_MAIN

    Dim SUBJECT_LIST As New List(Of List(Of String))
    Dim TASK_LIST_SUBJECTID As New List(Of String)

    Dim colr_lightest As Color = Color.Snow
    Dim colr_lighter As Color = Color.Aquamarine
    Dim colr_light As Color = Color.Azure
    Dim colr_medium As Color = Color.LightBlue
    Dim colr_mediumlight As Color = Color.PowderBlue
    Dim colr_dark As Color = Color.SteelBlue
    Dim colr_darker As Color = Color.DarkSlateGray
    Dim colr_darkest As Color = Color.MidnightBlue

    'Form initialaztion and loading screen
    Private Sub frm_MAIN_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'gdExa1EnSC
        'sql3404642
        '"sql3.freesqldatabase.com", "sql3404642", "gdExa1EnSC", "sql3404642"

        frm_LoadingScreen.Show()
        frm_LoadingScreen.ProgressBar_main.Value = 0

        frm_LoadingScreen.lbl_LOADING.Text = "Initializing Database Connection..."
        Application.DoEvents()
        init_DBCONNECTION()
        frm_LoadingScreen.ProgressBar_main.Value = 40


        '''' TEST '''''

        'updateSubject("TEST1", "UPDATED TEST", "MONDAY", "8", "9")

        '''''''''''''''


        frm_LoadingScreen.lbl_LOADING.Text = "Initializing Subjects..."
        Application.DoEvents()
        init_SUBJECTLIST(True)
        'Progress bar value is incremented inside the init_SUBJECTLIST()
        'frm_LoadingScreen.ProgressBar_main.Value = 70

        frm_LoadingScreen.lbl_LOADING.Text = "Initializing Notes..."
        Application.DoEvents()
        initNOTELIST()
        frm_LoadingScreen.ProgressBar_main.Value = 80

        frm_LoadingScreen.lbl_LOADING.Text = "Initializing Menu and Panels..."
        Application.DoEvents()

        lbl_LOADING.Parent = panel_WINDOW
        panel_CONTROLBAR.Parent = panel_WINDOW
        splitCon_MAIN.Parent = panel_WINDOW

        selButton(btn_SUBJECTS)
        openPanel(panel_MAIN_SUBJECTS)

        'changeTHEME("dark")

        frm_LoadingScreen.ProgressBar_main.Value = 100


        date_DEADLINEDATE.Value = Now

        frm_LoadingScreen.Close()

        Me.WindowState = FormWindowState.Maximized



        

    End Sub


    'init SUBJECT PANEL and TASK PANEL
    Public Sub init_SUBJECTLIST(ByVal ShowProgress As Boolean)
        'RESET
        SUBJECT_LIST = getSubjectList()
        flwpanel_SUBJECTS_DRAWER.Controls.Clear()
        listbx_REMOVESUBJECTS.Items.Clear()
        TASK_LIST_SUBJECTID.Clear()
        listbx_TASKLIST2.Items.Clear()
        cmbx_SUBJECTNAME.Items.Clear()

        Dim pending_task_count As Integer = 0

        'RESET TABLE
        tblpanel_SCHED.Controls.Clear()
        resetSCHEDULETABLE()

        'For loading screen
        If ShowProgress Then
            frm_LoadingScreen.ProgressBar_main.Value = 40
        End If

        'SUBJECT ITEM PANELS
        Dim SUBJECT_ITEMS As New List(Of Panel)

        If SUBJECT_LIST.Count = 0 Then
            Dim empty_sub As New Label
            empty_sub.Text = "Please Add Subjects"
            empty_sub.Width = 400
            empty_sub.Height = 400
            empty_sub.TextAlign = ContentAlignment.MiddleCenter
            empty_sub.ForeColor = colr_darker
            empty_sub.Font = New Font("Arial", 14)
            flwpanel_SUBJECTS_DRAWER.Controls.Add(empty_sub)
        End If

        For Each subject In SUBJECT_LIST
            'For loading screen
            If ShowProgress Then
                frm_LoadingScreen.lbl_LOADING.Text = "Initializing Subjects..." + subject(0)
                Application.DoEvents()
            End If

            ''''''FOR SUBJECT LIST PANEL''''''
            'subject is a list of strings where subject(0) is the subject_id and subject(1) is te subject_name
            Dim subj_panel As New Panel()
            subj_panel.Height = 200
            subj_panel.Width = 450
            subj_panel.BackColor = colr_light

            Dim brder_panel As New Panel
            brder_panel.Parent = subj_panel
            brder_panel.Height = 5
            brder_panel.Width = 450
            brder_panel.Top = 0
            brder_panel.Left = 0
            brder_panel.BackColor = colr_darker

            '' Test''
            subj_panel.Name = subject(0)
            Dim btn_EDITSUB As New Button()
            btn_EDITSUB.Parent = subj_panel
            btn_EDITSUB.Text = "Edit"
            btn_EDITSUB.Top = 170
            btn_EDITSUB.Left = 390
            btn_EDITSUB.Height = 20
            btn_EDITSUB.Width = 50
            btn_EDITSUB.FlatStyle = FlatStyle.Flat
            btn_EDITSUB.BackColor = colr_light
            btn_EDITSUB.ForeColor = colr_darkest
            btn_EDITSUB.Font = New Font("Verdana", 6, FontStyle.Bold)
            btn_EDITSUB.TabStop = False

            AddHandler btn_EDITSUB.Click, AddressOf editSub_Click

            '' Test''


            Dim subj_label As New Label()
            subj_label.Parent = subj_panel
            subj_label.AutoSize = True
            subj_label.Font = New Font("Verdana", 11, FontStyle.Bold)
            subj_label.Top = 13
            subj_label.Left = 5
            subj_label.Anchor = AnchorStyles.Top + AnchorStyles.Left
            subj_label.ForeColor = colr_darkest
            subj_label.MaximumSize = subj_panel.Size

            Dim subj_tasks_list As New ListBox
            subj_tasks_list.Parent = subj_panel
            subj_tasks_list.Top = 45
            subj_tasks_list.Left = 15
            subj_tasks_list.Height = 150
            subj_tasks_list.Width = 250
            subj_tasks_list.BorderStyle = BorderStyle.None
            subj_tasks_list.SelectionMode = SelectionMode.None
            subj_tasks_list.ForeColor = colr_darkest
            subj_tasks_list.SelectionMode = SelectionMode.None
            subj_tasks_list.TabStop = False

            Dim subj_sched_list As New ListBox
            subj_sched_list.Parent = subj_panel
            subj_sched_list.Top = 45
            subj_sched_list.Left = 270
            subj_sched_list.Height = 150
            subj_sched_list.Width = 150
            subj_sched_list.BorderStyle = BorderStyle.None
            subj_sched_list.SelectionMode = SelectionMode.None
            subj_sched_list.ForeColor = colr_darkest
            subj_sched_list.BackColor = colr_light
            subj_sched_list.Font = New Font("Verdana", 10, FontStyle.Bold)
            subj_sched_list.SelectionMode = SelectionMode.None
            subj_sched_list.TabStop = False


            For Each sched In getScheduleString(subject(0))
                subj_sched_list.Items.Add(sched(0))
                subj_sched_list.Items.Add(sched(1) + "-" + sched(2))
            Next

            ''''''FOR SUBJECT LIST PANEL''''''

            ''''''FOR TASK LIST PANEL''''''
            Dim lvg As New ListViewGroup
            lvg.Header = subject(0) + " - " + subject(1)
            listbx_TASKLIST2.Groups.Add(lvg)

            If getSUBJECTTASKS(subject(0)).Count = 0 Then
                subj_tasks_list.Items.Add("No Task")
            End If


            For Each task In getSUBJECTTASKS(subject(0))
                subj_tasks_list.Items.Add(task(0) + " - " + task(1)) 'Added task
                Try
                    If Long.Parse(task(3)) < Now.Ticks Then
                        Dim lvi As New ListViewItem
                        lvi.Text = Date.Parse(task(2)).ToString("MMMM dd, yyyy") + " - " + task(5) + " - " + task(0) + " - " + task(1) + " - " + task(4)
                        lvi.BackColor = Color.FromArgb(255, 205, 205)
                        lvi.ForeColor = Color.FromArgb(55, 5, 5)
                        lvi.Group = lvg
                        listbx_TASKLIST2.Items.Add(lvi)
                    Else
                        Dim lvi As New ListViewItem
                        lvi.Text = Date.Parse(task(2)).ToString("MMMM dd, yyyy") + " - " + task(5) + " - " + task(0) + " - " + task(1) + " - " + task(4)
                        lvi.Group = lvg
                        listbx_TASKLIST2.Items.Add(lvi)
                    End If
                Catch ex As Exception
                    Dim lvi As New ListViewItem
                    lvi.Text = task(5) + " - " + task(0) + " - " + task(1) + " - " + task(4)
                    lvi.Group = lvg
                    listbx_TASKLIST2.Items.Add(lvi)
                End Try
                TASK_LIST_SUBJECTID.Add(task(6)) 'Added task ID

                pending_task_count += 1
            Next

            ''''''FOR TASK LIST PANEL''''''

            ''ADDING THE SUBJECT PANEL TO THE LIST OF PANELS
            subj_label.Text = subject(0) + " - " + subject(1)
            SUBJECT_ITEMS.Add(subj_panel)

            ''SUBJECT NAME COMBOBOX IN 'ADD TASK'
            cmbx_SUBJECTNAME.Items.Add(subject(1).ToString)

            ''LIST BOX OF SUBJECTS FOR 'REMOVE SUBJECTS'
            listbx_REMOVESUBJECTS.Items.Add(subject(0))

            ''FOR LOADING SCREEN
            If ShowProgress Then
                frm_LoadingScreen.ProgressBar_main.Value += (30 / SUBJECT_LIST.Count)
            End If


            ''''''FOR SCHED TABLE PANEL''''''
            tblpanel_SCHED.RowCount += 1

            Dim subj_schd_lbl As New Label
            subj_schd_lbl.Text = subject(0)
            subj_schd_lbl.TextAlign = ContentAlignment.MiddleCenter
            subj_schd_lbl.Dock = DockStyle.Fill
            subj_schd_lbl.Font = New Font("Verdana", 12, FontStyle.Bold)

            Dim sched_row As RowStyle = tblpanel_SCHED.RowStyles(tblpanel_SCHED.RowCount - 2)
            sched_row.SizeType = SizeType.Percent
            sched_row.Height = 80 / (SUBJECT_LIST.Count)

            tblpanel_SCHED.RowStyles.Add(New RowStyle(sched_row.SizeType, sched_row.Height))
            tblpanel_SCHED.Controls.Add(subj_schd_lbl, 0, tblpanel_SCHED.RowCount - 2)


            For Each sch In getScheduleString(subject(0))
                Dim sch_day As New Label
                sch_day.BackColor = colr_lighter
                sch_day.TextAlign = ContentAlignment.MiddleCenter
                sch_day.Dock = DockStyle.Fill
                sch_day.Font = New Font("Arial", 9)
                sch_day.Text = sch(1) + "-" + sch(2)
                If sch(0) = "MONDAY" Then
                    tblpanel_SCHED.Controls.Add(sch_day, 1, tblpanel_SCHED.RowCount - 2)
                ElseIf sch(0) = "TUESDAY" Then
                    tblpanel_SCHED.Controls.Add(sch_day, 2, tblpanel_SCHED.RowCount - 2)
                ElseIf sch(0) = "WEDNESDAY" Then
                    tblpanel_SCHED.Controls.Add(sch_day, 3, tblpanel_SCHED.RowCount - 2)
                ElseIf sch(0) = "THURSDAY" Then
                    tblpanel_SCHED.Controls.Add(sch_day, 4, tblpanel_SCHED.RowCount - 2)
                ElseIf sch(0) = "FRIDAY" Then
                    tblpanel_SCHED.Controls.Add(sch_day, 5, tblpanel_SCHED.RowCount - 2)
                ElseIf sch(0) = "SATURDAY" Then
                    tblpanel_SCHED.Controls.Add(sch_day, 6, tblpanel_SCHED.RowCount - 2)
                Else
                    tblpanel_SCHED.Controls.Add(sch_day, 7, tblpanel_SCHED.RowCount - 2)
                End If
            Next
            ''''''FOR SCHED TABLE PANEL''''''
        Next

        For Each pane In SUBJECT_ITEMS
            flwpanel_SUBJECTS_DRAWER.Controls.Add(pane)
        Next

        '''' Test Archive ''''
        Dim lvg_arch As New ListViewGroup
        lvg_arch.Header = "Done"
        listbx_TASKLIST2.Groups.Add(lvg_arch)

        For Each subject_a In SUBJECT_LIST
            For Each arch_task In getSUBJECTTASKSARCHIVE(subject_a(0))
                Dim lvi_A As New ListViewItem
                lvi_A.Text = arch_task(5) + " - " + arch_task(0) + " - " + arch_task(1) + " - " + arch_task(4)
                lvi_A.Font = New Font("Verdana", 10, FontStyle.Strikeout)
                lvi_A.Group = lvg_arch
                listbx_TASKLIST2.Items.Add(lvi_A)
                TASK_LIST_SUBJECTID.Add(arch_task(6)) 'Added task ID
            Next
        Next
        '''' Test Archive ''''


        Dim exc_panel As New Panel()
        exc_panel.Height = 200
        exc_panel.Width = 450
        flwpanel_SUBJECTS_DRAWER.Controls.Add(exc_panel)
        lbl_NTASKS.Text = pending_task_count.ToString + " tasks..."
    End Sub


    'Reset SCHEDULE TABLE items
    Public Sub resetSCHEDULETABLE()
        Me.tblpanel_SCHED.AutoScroll = True
        Me.tblpanel_SCHED.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.[Single]
        Me.tblpanel_SCHED.ColumnCount = 8
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.63038!))
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62423!))
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62423!))
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62423!))
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62423!))
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62423!))
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62423!))
        Me.tblpanel_SCHED.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.62423!))
        Me.tblpanel_SCHED.Controls.Add(Me.Label18, 1, 0)
        Me.tblpanel_SCHED.Controls.Add(Me.Label19, 2, 0)
        Me.tblpanel_SCHED.Controls.Add(Me.Label20, 3, 0)
        Me.tblpanel_SCHED.Controls.Add(Me.Label21, 4, 0)
        Me.tblpanel_SCHED.Controls.Add(Me.Label22, 5, 0)
        Me.tblpanel_SCHED.Controls.Add(Me.Label23, 6, 0)
        Me.tblpanel_SCHED.Controls.Add(Me.Label24, 7, 0)
        Me.tblpanel_SCHED.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tblpanel_SCHED.Location = New System.Drawing.Point(0, 0)
        Me.tblpanel_SCHED.Name = "tblpanel_SCHED"
        Me.tblpanel_SCHED.RowCount = 2
        Me.tblpanel_SCHED.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25.0!))
        Me.tblpanel_SCHED.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tblpanel_SCHED.Size = New System.Drawing.Size(496, 380)
        Me.tblpanel_SCHED.TabIndex = 0
    End Sub


    'init NOTES LIST
    Public Sub initNOTELIST()

        For Each ntitle In getNOTESLIST()
            listbx_NOTES.Items.Add(ntitle)
        Next

    End Sub


    Public Sub showResizeControl(ByVal tr As Boolean)
        If tr Then
            panel_WINDOW.Top = 3
            panel_WINDOW.Left = 3
            panel_WINDOW.Height = Me.Height - 6
            panel_WINDOW.Width = Me.Width - 6
        ElseIf Not tr Then
            panel_WINDOW.Top = 0
            panel_WINDOW.Left = 0
            panel_WINDOW.Height = Me.Height
            panel_WINDOW.Width = Me.Width
        End If
    End Sub


    ''''CONTROL BAR''''
    Private Sub btn_CLOSE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_CLOSE.Click
        Me.Close()
    End Sub
    Private Sub btn_MINIMIZE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_MINIMIZE.Click
        Me.WindowState = FormWindowState.Minimized
    End Sub
    Private Sub btn_MAXMIN_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_MAXMIN.Click
        If Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
            showResizeControl(True)
        Else
            Me.WindowState = FormWindowState.Maximized
            showResizeControl(False)
        End If
    End Sub

    'Initial Variables'
    Dim initx As Double = 0
    Dim inity As Double = 0
    Dim initmx As Double = 0
    Dim initmy As Double = 0
    Dim mdown As Boolean = False

    Dim initrx As Double = 0
    Dim initry As Double = 0
    Dim initmrx As Double = 0
    Dim initmry As Double = 0
    Dim mouse_Bottom As Boolean = False
    Dim mouse_Right As Boolean = False
    Dim mouse_Top As Boolean = False
    Dim mouse_Left As Boolean = False
    Dim mouse_RTopCorner As Boolean = False
    Dim mouse_LTopCorner As Boolean = False
    Dim mouse_RBotCorner As Boolean = False
    Dim mouse_LBotCorner As Boolean = False
    Dim mrdown As Boolean = False

    'RESIZE'
    Private Sub panel_RESIZECONTROL_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles panel_RESIZECONTROL.MouseHover
        If (MousePosition.X - Me.Location.X) > (Me.Width - 15) And (MousePosition.Y - Me.Location.Y) < (Me.Height - 15) And (MousePosition.Y - Me.Location.Y) > (15) Then
            Cursor = Cursors.SizeWE
        ElseIf (MousePosition.X - Me.Location.X) < (15) And (MousePosition.Y - Me.Location.Y) < (Me.Height - 15) And (MousePosition.Y - Me.Location.Y) > (15) Then
            Cursor = Cursors.SizeWE
        ElseIf (MousePosition.Y - Me.Location.Y) > (Me.Height - 15) And (MousePosition.X - Me.Location.X) < (Me.Width - 15) And (MousePosition.X - Me.Location.X) > (15) Then
            Cursor = Cursors.SizeNS
        ElseIf (MousePosition.Y - Me.Location.Y) < (15) And (MousePosition.X - Me.Location.X) < (Me.Width - 15) And (MousePosition.X - Me.Location.X) > (15) Then
            Cursor = Cursors.SizeNS
        ElseIf (MousePosition.X - Me.Location.X) < 15 And (MousePosition.Y - Me.Location.Y) < 15 Then
            Cursor = Cursors.SizeNWSE
            'Top left
        ElseIf (MousePosition.X - Me.Location.X) > (Me.Width - 15) And (MousePosition.Y - Me.Location.Y) > (Me.Height - 15) Then
            Cursor = Cursors.SizeNWSE
            'Bot right
        ElseIf (MousePosition.X - Me.Location.X) > (Me.Width - 15) And (MousePosition.Y - Me.Location.Y) < 15 Then
            Cursor = Cursors.SizeNESW
            'Top right
        ElseIf (MousePosition.X - Me.Location.X) < 15 And (MousePosition.Y - Me.Location.Y) > (Me.Height - 15) Then
            Cursor = Cursors.SizeNESW
            'Bot left
        Else
            Cursor = Cursors.SizeAll
        End If

    End Sub
    Private Sub panel_RESIZECONTROL_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles panel_RESIZECONTROL.MouseLeave
        Cursor = Cursors.Default
    End Sub
    Private Sub panel_RESIZECONTROL_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel_RESIZECONTROL.MouseDown
        initrx = Me.Width
        initry = Me.Height
        initmrx = MousePosition.X
        initmry = MousePosition.Y

        initx = Me.Location.X
        inity = Me.Location.Y

        If (MousePosition.X - Me.Location.X) > (Me.Width - 15) And (MousePosition.Y - Me.Location.Y) < (Me.Height - 15) And (MousePosition.Y - Me.Location.Y) > (15) Then
            Cursor = Cursors.SizeWE
            mouse_Right = True
        ElseIf (MousePosition.X - Me.Location.X) < (15) And (MousePosition.Y - Me.Location.Y) < (Me.Height - 15) And (MousePosition.Y - Me.Location.Y) > (15) Then
            Cursor = Cursors.SizeWE
            mouse_Left = True
        ElseIf (MousePosition.Y - Me.Location.Y) > (Me.Height - 15) And (MousePosition.X - Me.Location.X) < (Me.Width - 15) And (MousePosition.X - Me.Location.X) > (15) Then
            Cursor = Cursors.SizeNS
            mouse_Bottom = True
        ElseIf (MousePosition.Y - Me.Location.Y) < (15) And (MousePosition.X - Me.Location.X) < (Me.Width - 15) And (MousePosition.X - Me.Location.X) > (15) Then
            Cursor = Cursors.SizeNS
            mouse_Top = True
        ElseIf (MousePosition.X - Me.Location.X) < 15 And (MousePosition.Y - Me.Location.Y) < 15 Then
            Cursor = Cursors.SizeNWSE
            mouse_LTopCorner = True
            'Top left
        ElseIf (MousePosition.X - Me.Location.X) > (Me.Width - 15) And (MousePosition.Y - Me.Location.Y) > (Me.Height - 15) Then
            Cursor = Cursors.SizeNWSE
            mouse_RBotCorner = True
            'Bot right
        ElseIf (MousePosition.X - Me.Location.X) > (Me.Width - 15) And (MousePosition.Y - Me.Location.Y) < 15 Then
            Cursor = Cursors.SizeNESW
            mouse_RTopCorner = True
            'Top right
        ElseIf (MousePosition.X - Me.Location.X) < 15 And (MousePosition.Y - Me.Location.Y) > (Me.Height - 15) Then
            Cursor = Cursors.SizeNESW
            mouse_LBotCorner = True
            'Bot left
        Else
            Cursor = Cursors.SizeAll
        End If
        mrdown = True
    End Sub
    Private Sub panel_RESIZECONTROL_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel_RESIZECONTROL.MouseMove
        If mouse_RBotCorner And Me.WindowState = FormWindowState.Normal Then
            Me.Width = MousePosition.X - initmrx + initrx
            Me.Height = MousePosition.Y - initmry + initry
        ElseIf mouse_Right And Me.WindowState = FormWindowState.Normal Then
            Me.Width = MousePosition.X - initmrx + initrx
        ElseIf mouse_Bottom And Me.WindowState = FormWindowState.Normal Then
            Me.Height = MousePosition.Y - initmry + initry
        ElseIf mouse_Left And Me.WindowState = FormWindowState.Normal Then
            Me.Width = initrx - (MousePosition.X - initmrx)
            If Not Me.Width <= Me.MinimumSize.Width Then
                Me.Left = initx + (MousePosition.X - initmrx)
            End If
        ElseIf mouse_Top And Me.WindowState = FormWindowState.Normal Then
            Me.Height = initry - (MousePosition.Y - initmry)
            If Not Me.Height <= Me.MinimumSize.Height Then
                Me.Top = inity + (MousePosition.Y - initmry)
            End If
        ElseIf mouse_LBotCorner And Me.WindowState = FormWindowState.Normal Then
            Me.Width = initrx - (MousePosition.X - initmrx)
            If Not Me.Width <= Me.MinimumSize.Width Then
                Me.Left = initx + (MousePosition.X - initmrx)
            End If
            Me.Height = MousePosition.Y - initmry + initry
        ElseIf mouse_LTopCorner And Me.WindowState = FormWindowState.Normal Then
            Me.Width = initrx - (MousePosition.X - initmrx)
            If Not Me.Width <= Me.MinimumSize.Width Then
                Me.Left = initx + (MousePosition.X - initmrx)
            End If
            Me.Height = initry - (MousePosition.Y - initmry)
            If Not Me.Height <= Me.MinimumSize.Height Then
                Me.Top = inity + (MousePosition.Y - initmry)
            End If
        ElseIf mouse_RTopCorner And Me.WindowState = FormWindowState.Normal Then
            Me.Width = MousePosition.X - initmrx + initrx
            Me.Height = initry - (MousePosition.Y - initmry)
            If Not Me.Height <= Me.MinimumSize.Height Then
                Me.Top = inity + (MousePosition.Y - initmry)
            End If
        End If
    End Sub
    Private Sub panel_RESIZECONTROL_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel_RESIZECONTROL.MouseUp
        mrdown = False
        mouse_Bottom = False
        mouse_Top = False
        mouse_Left = False
        mouse_Right = False
        mouse_LBotCorner = False
        mouse_LTopCorner = False
        mouse_RBotCorner = False
        mouse_RTopCorner = False
        Cursor = Cursors.Default
    End Sub
    'RESIZE'

    'PANEL DRAG'
    Private Sub panel_CONTROLBAR_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel_CONTROLBAR.MouseDown
        initx = Me.Location.X
        inity = Me.Location.Y
        initmx = MousePosition.X
        initmy = MousePosition.Y
        mdown = True
    End Sub
    Private Sub panel_CONTROLBAR_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel_CONTROLBAR.MouseMove
        If mdown And Me.WindowState = FormWindowState.Normal Then
            Me.Left = MousePosition.X - initmx + initx
            Me.Top = MousePosition.Y - initmy + inity
        ElseIf mdown And Me.WindowState = FormWindowState.Maximized Then
            Me.WindowState = FormWindowState.Normal
            showResizeControl(True)
            Me.Left = MousePosition.X - initmx + initx
            Me.Top = MousePosition.Y - initmy + inity
        End If
    End Sub
    Private Sub panel_CONTROLBAR_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel_CONTROLBAR.MouseUp
        If MousePosition.Y < 5 And Me.WindowState = FormWindowState.Normal Then
            Me.WindowState = FormWindowState.Maximized
            showResizeControl(False)
        End If
        mdown = False
    End Sub
    'PANEL DRAG'

    Private Sub frm_MAIN_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged

        updateTILESIZE()
    End Sub
    Public Sub updateTILESIZE()
        Dim tile_size As New Size()
        tile_size.Width = listbx_TASKLIST2.Width - 50
        tile_size.Height = 50
        listbx_TASKLIST2.TileSize = tile_size
    End Sub

    Private Sub tblpanel_SCHED_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tblpanel_SCHED.SizeChanged
        updateSCHEDFONTSIZE()
    End Sub
    Public Sub updateSCHEDFONTSIZE()
        Dim fnt_size_proportion As Double = (250 / 109359)
        Dim tbl_size As Double = ((tblpanel_SCHED.Width * 0.116) * ((tblpanel_SCHED.Height - 50) / (SUBJECT_LIST.Count + 1)))
        Dim fnt_size As Double = (fnt_size_proportion * tbl_size)
        If fnt_size > 14 Then
            fnt_size = 14
        ElseIf fnt_size < 4 Then
            fnt_size = 4
        End If
        For label_item As Integer = 0 To (tblpanel_SCHED.Controls.Count - 1)
            If label_item < 7 Or Not label_item Mod 2 = 0 Then
                Continue For
            End If
            tblpanel_SCHED.Controls.Item(label_item).Font = New Font("Arial", fnt_size)
        Next
    End Sub



    ''''CONTROL BAR''''


    ''''MENU PANEL''''
    'Panel Menu Launch Buttons
    Private Sub btn_SUBJECTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SUBJECTS.Click
        selButton(btn_SUBJECTS)
        openPanel(panel_MAIN_SUBJECTS)

    End Sub
    Private Sub btn_TASK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TASK.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()
        selButton(btn_TASK)
        openPanel(panel_MAIN_TASKS)
        updateTILESIZE()
        lbl_LOADING.Visible = False
    End Sub
    Private Sub btn_SCHEDULES_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SCHEDULES.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()
        updateSCHEDFONTSIZE()
        selButton(btn_SCHEDULES)
        openPanel(panel_MAIN_SCHEDS)
        lbl_LOADING.Visible = False
    End Sub
    Private Sub btn_NOTES_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_NOTES.Click
        selButton(btn_NOTES)
        openPanel(panel_MAIN_NOTES)
    End Sub

    'Panel Nav metohds
    Public Sub openPanel(ByRef o_panel As Panel)
        panel_MAIN_NOTES.Visible = False
        panel_MAIN_SCHEDS.Visible = False
        panel_MAIN_SUBJECTS.Visible = False
        panel_MAIN_TASKS.Visible = False
        o_panel.Visible = True
    End Sub
    Public Sub selButton(ByRef o_btn As Button)
        btn_NOTES.BackColor = colr_dark
        btn_NOTES.ForeColor = colr_light
        btn_SUBJECTS.BackColor = colr_dark
        btn_SUBJECTS.ForeColor = colr_light
        btn_SCHEDULES.BackColor = colr_dark
        btn_SCHEDULES.ForeColor = colr_light
        btn_TASK.BackColor = colr_dark
        btn_TASK.ForeColor = colr_light
        o_btn.BackColor = colr_medium
        o_btn.ForeColor = colr_dark
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - " + o_btn.Text
    End Sub
    ''''MENU PANEL''''


    ''''SUBJECTS PANEL''''
    Dim addingSUBJ As Boolean = True
    Dim savedSUBJ As Boolean = False

    'Subject Panel Buttons
    Private Sub btn_OPENADDSUBJECT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OPENADDSUBJECT.Click
        btn_OPENADDSUBJECT.Visible = False
        btn_OPENREMOVESUBJECT.Visible = False
        btn_ADDSUBJECTBACK.Visible = True

        panel_REMOVESUBJECT.Visible = False
        panel_flwpanelContainer.Visible = False
        panel_ADDSUBJECT.Visible = True
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Subjects - Add"

        txt_SUBJECTID.Focus()
    End Sub
    Private Sub btn_ADDSUBJECTBACK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_ADDSUBJECTBACK.Click
        If String.IsNullOrWhiteSpace(txt_SUBJECTID.Text) And String.IsNullOrWhiteSpace(txt_SUBJECTNAME.Text) Then
            savedSUBJ = True
        End If

        If Not savedSUBJ Then
            If MessageBox.Show("Discard changes in Subject Details?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                addingSUBJ = True
                savedSUBJ = False

                btn_ADDSUBJECT.Text = "Add"
                txt_SUBJECTID.Enabled = True


                btn_OPENADDSUBJECT.Visible = True
                btn_ADDSUBJECTBACK.Visible = False
                btn_OPENREMOVESUBJECT.Visible = True

                panel_flwpanelContainer.Visible = True
                panel_ADDSUBJECT.Visible = False
                panel_REMOVESUBJECT.Visible = False
                Me.lbl_CTRL_TITLE.Text = "Student Organizer - Subjects"

                txt_SUBJECTID.Clear()
                txt_SUBJECTNAME.Clear()
            End If
        Else
            addingSUBJ = True
            savedSUBJ = False

            btn_ADDSUBJECT.Text = "Add"
            txt_SUBJECTID.Enabled = True


            btn_OPENADDSUBJECT.Visible = True
            btn_ADDSUBJECTBACK.Visible = False
            btn_OPENREMOVESUBJECT.Visible = True

            panel_flwpanelContainer.Visible = True
            panel_ADDSUBJECT.Visible = False
            panel_REMOVESUBJECT.Visible = False
            Me.lbl_CTRL_TITLE.Text = "Student Organizer - Subjects"

            txt_SUBJECTID.Clear()
            txt_SUBJECTNAME.Clear()
        End If

        
    End Sub
    Private Sub btn_OPENREMOVESUBJECT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OPENREMOVESUBJECT.Click
        btn_OPENREMOVESUBJECT.Visible = False
        btn_OPENADDSUBJECT.Visible = False
        btn_ADDSUBJECTBACK.Visible = True

        panel_ADDSUBJECT.Visible = False
        panel_flwpanelContainer.Visible = False
        panel_REMOVESUBJECT.Visible = True
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Subjects - Remove"
    End Sub

    'Add Subject button
    Private Sub btn_ADDSUBJECT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ADDSUBJECT.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()

        If addingSUBJ Then
            ''ADD''
            If String.IsNullOrWhiteSpace(txt_SUBJECTID.Text) Then
                MessageBox.Show("Empty Field", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf txt_SUBJECTID.Text.Contains(" ") Then
                MessageBox.Show("Remove spaces in Subject Subject ID", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf String.IsNullOrWhiteSpace(txt_SUBJECTNAME.Text) Then
                MessageBox.Show("Empty Field", "Invalid Subject Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf time_SCHEDFROM.Value.Hour = time_SCHEDTO.Value.Hour And time_SCHEDFROM.Value.Minute >= time_SCHEDTO.Value.Minute Then
                MessageBox.Show("Invalid Time PeriodA", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf time_SCHEDFROM.Value.Hour > time_SCHEDTO.Value.Hour Then
                MessageBox.Show("Invalid Time PeriodB", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Dim o_str As String = addSUBJECT(txt_SUBJECTID.Text, txt_SUBJECTNAME.Text)

                If o_str.Equals("Subject Code already Exist") Then

                    If MessageBox.Show("Subject Code already Exist, Add the new schedule to the existing subject instead?", "Subject Exist", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                        addSubjSched(txt_SUBJECTID.Text, cmbx_DAY.SelectedItem.ToString, toDECIMALHOUR(time_SCHEDFROM.Value), toDECIMALHOUR(time_SCHEDTO.Value))
                        init_SUBJECTLIST(False)
                        txt_SUBJECTID.Clear()
                        txt_SUBJECTNAME.Clear()
                        savedSUBJ = True
                        MessageBox.Show("Schedule Added", "Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If

                Else
                    addSubjSched(txt_SUBJECTID.Text, cmbx_DAY.SelectedItem.ToString, toDECIMALHOUR(time_SCHEDFROM.Value), toDECIMALHOUR(time_SCHEDTO.Value))
                    init_SUBJECTLIST(False)
                    txt_SUBJECTID.Clear()
                    txt_SUBJECTNAME.Clear()
                    savedSUBJ = True
                    MessageBox.Show(o_str, "Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If
            ''ADD''
        Else
            ''UPDATE''
            If String.IsNullOrWhiteSpace(txt_SUBJECTID.Text) Then
                MessageBox.Show("Empty Field", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf txt_SUBJECTID.Text.Contains(" ") Then
                MessageBox.Show("Remove spaces in Subject Subject ID", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf String.IsNullOrWhiteSpace(txt_SUBJECTNAME.Text) Then
                MessageBox.Show("Empty Field", "Invalid Subject Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf time_SCHEDFROM.Value.Hour = time_SCHEDTO.Value.Hour And time_SCHEDFROM.Value.Minute >= time_SCHEDTO.Value.Minute Then
                MessageBox.Show("Invalid Time PeriodA", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf time_SCHEDFROM.Value.Hour > time_SCHEDTO.Value.Hour Then
                MessageBox.Show("Invalid Time PeriodB", "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                updateSubject(txt_SUBJECTID.Text, txt_SUBJECTNAME.Text, cmbx_DAY.SelectedItem.ToString, toDECIMALHOUR(time_SCHEDFROM.Value), toDECIMALHOUR(time_SCHEDTO.Value))
                init_SUBJECTLIST(False)
                savedSUBJ = True
                MessageBox.Show("Schedule Updated", "Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            ''UPADTE''
        End If

        lbl_LOADING.Visible = False
    End Sub

    'Remove Subject button
    Private Sub btn_REMOVESUBJECTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_REMOVESUBJECTS.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()

        If listbx_REMOVESUBJECTS.SelectedItems.Count > 0 Then
            If MessageBox.Show("Delete Selected Subject(s)?", "Remove Subjects", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                For Each subject In listbx_REMOVESUBJECTS.SelectedItems
                    removeSUBJECT(subject.ToString())
                Next
                init_SUBJECTLIST(False)
                MessageBox.Show("Subject(s) removed", "Subjects", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Select Subjects to Delete", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        lbl_LOADING.Visible = False
    End Sub

    'Edit Subject click
    Protected Sub editSub_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim thisButton As Button = sender
        Console.WriteLine(thisButton.Parent.Name + "clicked")

        addingSUBJ = False

        Dim selected_SUBJECT As List(Of List(Of String)) = getScheduleString(thisButton.Parent.Name)

        btn_ADDSUBJECT.Text = "Save"
        txt_SUBJECTID.Enabled = False
        txt_SUBJECTID.Text = thisButton.Parent.Name
        txt_SUBJECTNAME.Text = getSubjectName(thisButton.Parent.Name)
        cmbx_DAY.Text = selected_SUBJECT(0).Item(0)
        time_SCHEDFROM.Value = Date.Parse(selected_SUBJECT(0).Item(1))
        time_SCHEDTO.Value = Date.Parse(selected_SUBJECT(0).Item(2))


        btn_OPENADDSUBJECT.Visible = False
        btn_OPENREMOVESUBJECT.Visible = False
        btn_ADDSUBJECTBACK.Visible = True

        panel_REMOVESUBJECT.Visible = False
        panel_flwpanelContainer.Visible = False
        panel_ADDSUBJECT.Visible = True
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Subjects - Add"

    End Sub

    Private Sub txt_SUBJECTNAME_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SUBJECTNAME.TextChanged
        savedSUBJ = False
    End Sub
    Private Sub txt_SUBJECTID_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_SUBJECTID.TextChanged
        savedSUBJ = False
    End Sub

    ''''SUBJECTS PANEL''''



    ''''TASK PANEL''''
    Dim addingTASK As Boolean = True
    Dim selectedTASK As New List(Of String)
    Dim savedTask As Boolean = False
    Dim isArchived As Boolean = False

    'Subject Panel Buttons
    Private Sub btn_OPENADDTASK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OPENADDTASK.Click
        btn_OPENADDTASK.Visible = False
        btn_BACKTASK.Visible = True

        panel_ADDTASK.Visible = True
        panel_TASKLISTPANEL.Visible = False
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Task - Add"

        cmbx_SUBJECTNAME.Focus()
    End Sub
    Private Sub btn_BACKTASK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_BACKTASK.Click
        If String.IsNullOrWhiteSpace(txt_TASKNAME.Text) Then
            savedTask = True
        End If


        If Not savedTask Then
            If MessageBox.Show("Discard changes in Task Details?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                addingTASK = True
                savedTask = False
                btn_ADDNEWTASK.Text = "Add"
                cmbx_SUBJECTNAME.Text = "<Select Subject>"
                txt_TASKNAME.Clear()
                txt_TASKDESC.Clear()
                cmbx_TASKTERM.ResetText()

                btn_OPENADDTASK.Visible = True
                btn_BACKTASK.Visible = False

                panel_ADDTASK.Visible = False
                panel_TASKLISTPANEL.Visible = True
                Me.lbl_CTRL_TITLE.Text = "Student Organizer - Task"
            End If
        Else
            addingTASK = True
            savedTask = False
            btn_ADDNEWTASK.Text = "Add"
            cmbx_SUBJECTNAME.Text = "<Select Subject>"
            txt_TASKNAME.Clear()
            txt_TASKDESC.Clear()
            cmbx_TASKTERM.ResetText()

            btn_OPENADDTASK.Visible = True
            btn_BACKTASK.Visible = False

            panel_ADDTASK.Visible = False
            panel_TASKLISTPANEL.Visible = True
            Me.lbl_CTRL_TITLE.Text = "Student Organizer - Task"
        End If


        



    End Sub

    'Add/update Task Button
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ADDNEWTASK.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()

        Dim date_DEADLINE As New Date()

        If addingTASK Then
            ''Add task''
            date_DEADLINE = New Date(date_DEADLINEDATE.Value.Year, date_DEADLINEDATE.Value.Month, date_DEADLINEDATE.Value.Day, date_DEADLINETIME.Value.Hour, date_DEADLINETIME.Value.Minute, 0)
            If cmbx_SUBJECTNAME.SelectedItem = Nothing Then
                MessageBox.Show("Select Subject", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf String.IsNullOrWhiteSpace(cmbx_SUBJECTNAME.SelectedItem.ToString) Then
                MessageBox.Show("Select Subject", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf Not subjectExist(cmbx_SUBJECTNAME.SelectedItem.ToString) Then
                MessageBox.Show("Subject selected does not exist", "Invalid Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf String.IsNullOrWhiteSpace(txt_TASKNAME.Text) Then
                MessageBox.Show("Please Enter a Task name", "Invalid Task Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf cb_NODEADLINE.Checked = False And date_DEADLINE.Ticks < Now.Ticks Then
                MessageBox.Show("Invalid Deadline", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If cb_NODEADLINE.Checked Then
                    If cmbx_TASKTERM.SelectedItem = Nothing Then
                        addTASK(getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, "", "", "", "0")
                    Else
                        addTASK(getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, "", "", cmbx_TASKTERM.SelectedItem.ToString, "0")
                    End If
                Else
                    If cmbx_TASKTERM.SelectedItem = Nothing Then
                        addTASK(getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, date_DEADLINE.ToString, date_DEADLINE.Ticks, "", "0")
                    Else
                        addTASK(getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, date_DEADLINE.ToString, date_DEADLINE.Ticks, cmbx_TASKTERM.SelectedItem.ToString, "0")
                    End If
                End If


                init_SUBJECTLIST(False)
                MessageBox.Show("New Task Added - " + txt_TASKNAME.Text + " Deadline: " + date_DEADLINE.ToString, "Task Added", MessageBoxButtons.OK, MessageBoxIcon.Information)
                cmbx_SUBJECTNAME.Text = "<Select Subject>"
                txt_TASKNAME.Clear()
                txt_TASKDESC.Clear()
                cmbx_TASKTERM.ResetText()
            End If
            ''Add task''
        Else
            ''Update task''
            date_DEADLINE = New Date(date_DEADLINEDATE.Value.Year, date_DEADLINEDATE.Value.Month, date_DEADLINEDATE.Value.Day, date_DEADLINETIME.Value.Hour, date_DEADLINETIME.Value.Minute, 0)
            If cmbx_SUBJECTNAME.SelectedItem = Nothing Then
                MessageBox.Show("Select Subject", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf String.IsNullOrWhiteSpace(cmbx_SUBJECTNAME.SelectedItem.ToString) Then
                MessageBox.Show("Select Subject", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf Not subjectExist(cmbx_SUBJECTNAME.SelectedItem.ToString) Then
                MessageBox.Show("Subject selected does not exist", "Invalid Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf String.IsNullOrWhiteSpace(txt_TASKNAME.Text) Then
                MessageBox.Show("Please Enter a Task name", "Invalid Task Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf cb_NODEADLINE.Checked = False And date_DEADLINE.Ticks < Now.Ticks Then
                MessageBox.Show("Invalid Deadline", "Invalid Date", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                If cb_NODEADLINE.Checked Then
                    If cmbx_TASKTERM.SelectedItem = Nothing Then
                        updateTASK(selectedTASK(0), getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, "", "", "", "0")
                    Else
                        updateTASK(selectedTASK(0), getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, "", "", cmbx_TASKTERM.SelectedItem.ToString, "0")
                    End If
                Else
                    If cmbx_TASKTERM.SelectedItem = Nothing Then
                        updateTASK(selectedTASK(0), getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, date_DEADLINE.ToString, date_DEADLINE.Ticks, "", "0")
                    Else
                        updateTASK(selectedTASK(0), getSubjectID(cmbx_SUBJECTNAME.SelectedItem.ToString), txt_TASKNAME.Text, txt_TASKDESC.Text, date_DEADLINE.ToString, date_DEADLINE.Ticks, cmbx_TASKTERM.SelectedItem.ToString, "0")
                    End If
                End If

                init_SUBJECTLIST(False)
                MessageBox.Show("Task Updated - " + txt_TASKNAME.Text + " Deadline: " + date_DEADLINE.ToString, "Task Updated", MessageBoxButtons.OK, MessageBoxIcon.Information)
                savedTask = True

            End If
            
            ''Update task''
        End If


            lbl_LOADING.Visible = False
    End Sub

    'Archive Task button
    Private Sub btn_ARCHIVETASK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ARCHIVETASK.Click

        lbl_LOADING.Visible = True
        Application.DoEvents()

        If listbx_TASKLIST2.SelectedItems.Count > 0 Then

            If Not isArchived Then
                If MessageBox.Show("Arhive Selected Task(s)?", "Archive Taks", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    For Each task_id In listbx_TASKLIST2.SelectedIndices
                        archiveTASK(CInt(TASK_LIST_SUBJECTID(task_id)))
                    Next
                    init_SUBJECTLIST(False)
                    MessageBox.Show("Tasks arhived", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                If MessageBox.Show("Unarhive Selected Task(s)?", "Unarhive Taks", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    For Each task_id In listbx_TASKLIST2.SelectedIndices
                        unarchiveTASK(CInt(TASK_LIST_SUBJECTID(task_id)))
                    Next
                    init_SUBJECTLIST(False)
                    MessageBox.Show("Tasks unarhived", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

        Else
            MessageBox.Show("Select Task to Archive/Unarchive", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If


        lbl_LOADING.Visible = False
    End Sub

    'Remove Task Button
    Private Sub btn_DELETETASK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_DELETETASK.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()

        If listbx_TASKLIST2.SelectedItems.Count > 0 Then
            If MessageBox.Show("Delete Selected Task(s)?", "Remove Taks", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                For Each task_id In listbx_TASKLIST2.SelectedIndices
                    removeTASK(CInt(TASK_LIST_SUBJECTID(task_id)))
                Next
                init_SUBJECTLIST(False)
                MessageBox.Show("Tasks Deleted", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Select Task to Delete", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
        lbl_LOADING.Visible = False
    End Sub

    'No Deadline Checkbox
    Private Sub cb_NODEADLINE_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_NODEADLINE.CheckedChanged
        If cb_NODEADLINE.Checked Then
            date_DEADLINEDATE.Enabled = False
            date_DEADLINETIME.Enabled = False
        Else
            date_DEADLINEDATE.Enabled = True
            date_DEADLINETIME.Enabled = True
        End If
    End Sub

    Private Sub listbx_TASKLIST2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles listbx_TASKLIST2.DoubleClick
        selectedTASK = getTASK(TASK_LIST_SUBJECTID(listbx_TASKLIST2.SelectedIndices.Item(0)).ToString)
        addingTASK = False
        btn_ADDNEWTASK.Text = "Save"
        cmbx_SUBJECTNAME.Text = getSubjectName(selectedTASK(1))
        txt_TASKNAME.Text = selectedTASK(2)
        txt_TASKDESC.Text = selectedTASK(3)
        cmbx_TASKTERM.Text = selectedTASK(6)

        If Not String.IsNullOrWhiteSpace(selectedTASK(5)) Then
            date_DEADLINEDATE.Value = New Date(Long.Parse(selectedTASK(5)))
            date_DEADLINETIME.Value = New Date(Long.Parse(selectedTASK(5)))
        Else
            cb_NODEADLINE.CheckState = CheckState.Checked
        End If

        btn_OPENADDTASK.Visible = False
        btn_BACKTASK.Visible = True

        panel_ADDTASK.Visible = True
        panel_TASKLISTPANEL.Visible = False
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Task - Edit"

        cmbx_SUBJECTNAME.Focus()
    End Sub

    Private Sub txt_TASKNAME_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txt_TASKNAME.TextChanged
        savedTask = False
    End Sub

    Private Sub listbx_TASKLIST2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles listbx_TASKLIST2.SelectedIndexChanged
        Try
            If listbx_TASKLIST2.SelectedItems.Item(0).Group.ToString() = "Done" Then
                btn_ARCHIVETASK.Text = "Unarchive"
                isArchived = True
            Else
                btn_ARCHIVETASK.Text = "Archive"
                isArchived = False
            End If
        Catch ex As Exception
            btn_ARCHIVETASK.Text = "Archive"
            isArchived = False
        End Try
    End Sub

    ''''TASK PANEL''''


    ''''NOTES PANEL''''
    Private Sub btn_OPENADDNOTE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_OPENADDNOTE.Click
        btn_OPENADDNOTE.Visible = False
        btn_NOTESBACK.Visible = True

        If String.IsNullOrWhiteSpace(txt_NOTETITLE.Text) Then
            txt_NOTETITLE.Text = "<Note Title>"
        End If

        panel_NOTESLIST.Visible = False
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Notes - Add"

        txt_NOTETITLE.Focus()
    End Sub

    Private Sub btn_NOTESBACK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_NOTESBACK.Click
        btn_OPENADDNOTE.Visible = True
        btn_NOTESBACK.Visible = False

        panel_NOTESLIST.Visible = True
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Notes"

        txt_NOTETITLE.Clear()
        rtxt_NOTECONTENT.Clear()


        listbx_NOTES.Items.Clear()
        initNOTELIST()

    End Sub

    'Open Note
    Private Sub listbx_NOTES_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles listbx_NOTES.SelectedIndexChanged

        If Not listbx_NOTES.SelectedItem = Nothing Then
            txt_NOTETITLE.Text = listbx_NOTES.SelectedItem.ToString
            rtxt_NOTECONTENT.Text = getNoteContent(listbx_NOTES.SelectedItem.ToString)

            btn_OPENADDNOTE.PerformClick()
        End If

    End Sub

    'Save Note
    Private Sub btn_SAVENOTE_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_SAVENOTE.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()

        If String.IsNullOrWhiteSpace(txt_NOTETITLE.Text) Or txt_NOTETITLE.Text = "<Note Title>" Then
            MessageBox.Show("Add a Title", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            If noteExist(txt_NOTETITLE.Text) Then
                If MessageBox.Show("Overwrite " + txt_NOTETITLE.Text + "?", "Note", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    updateNote(txt_NOTETITLE.Text, rtxt_NOTECONTENT.Text)
                    initNOTELIST()
                    MessageBox.Show("Note updated", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            Else
                addNote(txt_NOTETITLE.Text, rtxt_NOTECONTENT.Text)
                initNOTELIST()
                MessageBox.Show("Note Added", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        End If

        lbl_LOADING.Visible = False
    End Sub

    'Delete Note
    Private Sub btn_DELETENOTE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_DELETENOTE.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()

        If noteExist(txt_NOTETITLE.Text) Then
            If MessageBox.Show("Delete " + txt_NOTETITLE.Text + "?", "Note", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                removeNote(txt_NOTETITLE.Text)
                txt_NOTETITLE.Clear()
                rtxt_NOTECONTENT.Clear()
                initNOTELIST()
                MessageBox.Show("Note deleted", "Note", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            txt_NOTETITLE.Clear()
            rtxt_NOTECONTENT.Clear()

        End If

        lbl_LOADING.Visible = False
    End Sub
    ''''NOTES PANEL''''


    ''''Form Enter Key event''''
    ''ADD SUBJECT
    Private Sub txt_SUBJECTID_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SUBJECTID.KeyDown
        If e.KeyCode = 13 Then
            If String.IsNullOrWhiteSpace(txt_SUBJECTID.Text) Then
                MessageBox.Show("Empty Field", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf txt_SUBJECTID.Text.Contains(" ") Then
                MessageBox.Show("Remove spaces in Subject Subject ID", "Invalid ID", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                txt_SUBJECTNAME.Focus()
            End If
        End If
    End Sub
    Private Sub txt_SUBJECTNAME_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_SUBJECTNAME.KeyDown
        If e.KeyCode = 13 Then
            If String.IsNullOrWhiteSpace(txt_SUBJECTNAME.Text) Then
                MessageBox.Show("Empty Field", "Invalid Subject Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                cmbx_DAY.Focus()
            End If
        End If
    End Sub
    Private Sub cmbx_DAY_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbx_DAY.KeyDown
        If e.KeyCode = 13 Then
            time_SCHEDFROM.Focus()
        End If
    End Sub
    Private Sub time_SCHEDFROM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles time_SCHEDFROM.KeyDown
        If e.KeyCode = 13 Then
            time_SCHEDTO.Focus()
        End If
    End Sub
    Private Sub time_SCHEDTO_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles time_SCHEDTO.KeyDown
        If e.KeyCode = 13 Then
            btn_ADDSUBJECT.PerformClick()
        End If
    End Sub
    Private Sub listbx_REMOVESUBJECTS_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles listbx_REMOVESUBJECTS.KeyDown
        If e.KeyCode = Keys.Delete Then
            btn_REMOVESUBJECTS.PerformClick()
        End If
    End Sub
    ''ADD TASK
    Private Sub cmbx_SUBJECTNAME_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbx_SUBJECTNAME.KeyDown
        If e.KeyCode = 13 Then
            If cmbx_SUBJECTNAME.SelectedItem = Nothing Then
                MessageBox.Show("Select Subject", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf String.IsNullOrWhiteSpace(cmbx_SUBJECTNAME.SelectedItem.ToString) Then
                MessageBox.Show("Select Subject", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
            ElseIf Not subjectExist(cmbx_SUBJECTNAME.SelectedItem.ToString) Then
                MessageBox.Show("Subject selected does not exist", "Invalid Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                txt_TASKNAME.Focus()
            End If
        End If
    End Sub
    Private Sub txt_TASKNAME_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TASKNAME.KeyDown
        If e.KeyCode = 13 Then
            If String.IsNullOrWhiteSpace(txt_TASKNAME.Text) Then
                MessageBox.Show("Please Enter a Task name", "Invalid Task Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                txt_TASKDESC.Focus()
            End If
        End If
    End Sub
    Private Sub txt_TASKDESC_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_TASKDESC.KeyDown
        If e.KeyCode = 13 Then
            cmbx_TASKTERM.Focus()
        End If
    End Sub
    Private Sub cmbx_TASKTERM_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cmbx_TASKTERM.KeyDown
        If e.KeyCode = 13 Then
            date_DEADLINEDATE.Focus()
        End If
    End Sub
    Private Sub date_DEADLINEDATE_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles date_DEADLINEDATE.KeyDown
        If e.KeyCode = 13 Then
            date_DEADLINETIME.Focus()
        End If
    End Sub
    Private Sub date_DEADLINETIME_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles date_DEADLINETIME.KeyDown
        If e.KeyCode = 13 Then
            btn_ADDNEWTASK.PerformClick()
        End If
    End Sub
    Private Sub cb_NODEADLINE_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles cb_NODEADLINE.KeyDown
        If e.KeyCode = 13 Then
            btn_ADDNEWTASK.PerformClick()
        End If
    End Sub
    Private Sub listbx_TASKLIST2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles listbx_TASKLIST2.KeyDown
        If e.KeyCode = Keys.Delete Then
            btn_DELETETASK.PerformClick()
        End If
    End Sub
    ''ADD NOTE
    Private Sub txt_NOTETITLE_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_NOTETITLE.KeyDown
        If e.KeyCode = 13 Then
            If String.IsNullOrWhiteSpace(txt_NOTETITLE.Text) Or txt_NOTETITLE.Text = "<Note Title>" Then
                MessageBox.Show("Add a Title", "Empty Field", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                rtxt_NOTECONTENT.Focus()
            End If
        End If
    End Sub
    Private Sub rtxt_NOTECONTENT_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles rtxt_NOTECONTENT.KeyDown
        If e.KeyCode = Keys.F12 Then
            btn_SAVENOTE.PerformClick()
        End If
    End Sub
    ''''Form Enter Key event''''


    Public Sub changeTHEME(ByVal theme_name As String)

        If theme_name = "dark" Then
            colr_lightest = Color.FromArgb(250, 250, 250) 'Color.Snow
            colr_lighter = Color.FromArgb(240, 240, 240) 'Color.Aquamarine
            colr_mediumlight = Color.FromArgb(220, 220, 220) 'Color.PowderBlue
            colr_light = Color.FromArgb(200, 200, 200) 'Color.Azure
            colr_medium = Color.FromArgb(180, 180, 180) 'Color.LightBlue
            colr_dark = Color.FromArgb(80, 80, 80) 'Color.SteelBlue
            colr_darkest = Color.FromArgb(25, 25, 25) 'Color.MidnightBlue


            Me.splitCon_MAIN.Panel1.BackColor = colr_medium

            Me.panel_MENU.BackColor = colr_dark

            'Me.btn_LOGOUT.BackColor = System.Drawing.Color.Transparent
            'Me.btn_LOGOUT.ForeColor = System.Drawing.Color.White

            Me.panel_ACCOUNTINFO.BackColor = colr_dark

            'Me.lbl_USER.BackColor = colr_dark
            'Me.lbl_USER.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label2.ForeColor = colr_light

            Me.btn_NOTES.ForeColor = colr_light
            Me.btn_NOTES.BackColor = colr_dark

            Me.btn_SCHEDULES.ForeColor = colr_light
            Me.btn_SCHEDULES.BackColor = colr_dark

            Me.btn_TASK.ForeColor = colr_light
            Me.btn_TASK.BackColor = colr_dark

            Me.btn_SUBJECTS.ForeColor = colr_light
            Me.btn_SUBJECTS.BackColor = colr_dark

            Me.panel_MAIN_TASKS.BackColor = colr_medium

            Me.tblpanel_SCHED.BackColor = colr_lighter

            'Me.btn_OPENADDTASK.BackColor = System.Drawing.Color.Transparent

            Me.btn_BACKTASK.ForeColor = colr_darkest

            Me.Label3.ForeColor = colr_darkest

            'Me.btn_DELETETASK.BackColor = System.Drawing.Color.LightCoral
            'Me.btn_DELETETASK.ForeColor = System.Drawing.Color.White

            Me.lbl_NTASKS.ForeColor = colr_darkest

            'Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(205, Byte), Integer))

            Me.listbx_TASKLIST2.ForeColor = colr_darkest

            'Me.btn_ARCHIVETASK.BackColor = System.Drawing.Color.CadetBlue
            'Me.btn_ARCHIVETASK.ForeColor = System.Drawing.Color.White

            Me.cb_NODEADLINE.ForeColor = colr_darkest

            'Me.btn_ADDNEWTASK.BackColor = System.Drawing.Color.LimeGreen
            'Me.btn_ADDNEWTASK.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label16.BackColor = System.Drawing.Color.Transparent
            Me.Label16.ForeColor = colr_darkest

            Me.cmbx_TASKTERM.ForeColor = colr_darkest

            Me.Label15.BackColor = System.Drawing.Color.Transparent
            Me.Label15.ForeColor = colr_darkest

            Me.Label14.BackColor = System.Drawing.Color.Transparent
            Me.Label14.ForeColor = colr_darkest

            Me.Label13.BackColor = System.Drawing.Color.Transparent
            Me.Label13.ForeColor = colr_darkest

            Me.cmbx_SUBJECTNAME.ForeColor = colr_darkest

            Me.Label12.BackColor = System.Drawing.Color.Transparent
            Me.Label12.ForeColor = colr_darkest

            Me.panel_MAIN_NOTES.BackColor = colr_medium

            'Me.btn_OPENADDNOTE.BackColor = System.Drawing.Color.Transparent

            Me.btn_NOTESBACK.ForeColor = colr_darkest

            Me.listbx_NOTES.BackColor = colr_light

            'Me.panel_NOTEEDITOR.BackColor = System.Drawing.Color.Transparent

            'Me.panel_NOTECONTENT.BackColor = System.Drawing.Color.White

            Me.Label25.ForeColor = colr_darkest

            Me.txt_NOTETITLE.BackColor = colr_medium

            'Me.btn_SAVENOTE.BackColor = System.Drawing.Color.LimeGreen
            'Me.btn_SAVENOTE.ForeColor = System.Drawing.Color.Azure

            'Me.btn_DELETENOTE.BackColor = System.Drawing.Color.LightCoral
            'Me.btn_DELETENOTE.ForeColor = System.Drawing.Color.Azure

            Me.Label5.ForeColor = colr_darkest

            Me.panel_MAIN_SCHEDS.BackColor = colr_medium

            Me.panel_SCHEDULEPANE.BackColor = System.Drawing.Color.LightCyan

            Me.Label18.BackColor = System.Drawing.Color.LightCyan

            Me.Label4.ForeColor = colr_darkest

            Me.panel_MAIN_SUBJECTS.BackColor = colr_medium

            Me.lbl_MAIN_SUBJECTS.ForeColor = colr_darkest

            'Me.btn_ADDSUBJECTBACK.ForeColor = colr_darkest

            'Me.btn_OPENREMOVESUBJECT.ForeColor = colr_darkest

            'Me.btn_OPENADDSUBJECT.BackColor = System.Drawing.Color.Transparent

            Me.flwpanel_SUBJECTS_DRAWER.BackColor = colr_medium

            Me.btn_ADDSUBJECT.BackColor = System.Drawing.Color.LimeGreen
            Me.btn_ADDSUBJECT.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label10.ForeColor = colr_darkest
            Me.Label9.ForeColor = colr_darkest
            Me.Label8.ForeColor = colr_darkest
            Me.cmbx_DAY.BackColor = System.Drawing.Color.White
            Me.Label7.ForeColor = colr_darkest
            Me.Label6.ForeColor = colr_darkest
            Me.Label1.ForeColor = colr_darkest

            'Me.txt_SUBJECTNAME.BackColor = System.Drawing.Color.White
            'Me.txt_SUBJECTID.BackColor = System.Drawing.Color.White

            'Me.btn_REMOVESUBJECTS.BackColor = System.Drawing.Color.Firebrick
            'Me.btn_REMOVESUBJECTS.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label11.BackColor = System.Drawing.Color.Transparent
            Me.Label11.ForeColor = colr_darkest

            'Me.btn_MAXMIN.BackColor = System.Drawing.Color.Transparent
            'Me.btn_MAXMIN.ForeColor = System.Drawing.Color.DarkSlateBlue

            'Me.btn_MINIMIZE.BackColor = System.Drawing.Color.Transparent
            'Me.btn_MINIMIZE.ForeColor = System.Drawing.Color.DarkSlateBlue

            Me.lbl_CTRL_TITLE.ForeColor = colr_darkest

            'Me.btn_CLOSE.BackColor = System.Drawing.Color.Transparent

            Me.lbl_LOADING.BackColor = colr_medium
            Me.lbl_LOADING.ForeColor = System.Drawing.Color.DarkSlateGray

            Me.panel_RESIZECONTROL.BackColor = colr_dark

            'Me.panel_WINDOW.BackColor = System.Drawing.Color.Transparent

            Me.BackColor = colr_medium

            Me.panel_CONTROLBAR.BackColor = colr_mediumlight

        Else
            colr_lightest = Color.Snow
            colr_lighter = Color.Aquamarine
            colr_light = Color.Azure
            colr_medium = Color.LightBlue
            colr_mediumlight = Color.PowderBlue
            colr_dark = Color.SteelBlue
            colr_darkest = Color.MidnightBlue

            Me.splitCon_MAIN.Panel1.BackColor = System.Drawing.Color.LightBlue

            Me.panel_MENU.BackColor = System.Drawing.Color.SteelBlue

            Me.btn_LOGOUT.BackColor = System.Drawing.Color.Transparent
            Me.btn_LOGOUT.ForeColor = System.Drawing.Color.White

            Me.panel_ACCOUNTINFO.BackColor = System.Drawing.Color.SteelBlue

            Me.lbl_USER.BackColor = System.Drawing.Color.SteelBlue
            Me.lbl_USER.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label2.ForeColor = System.Drawing.Color.Azure

            Me.btn_NOTES.ForeColor = System.Drawing.Color.Azure

            Me.btn_SCHEDULES.ForeColor = System.Drawing.Color.Azure

            Me.btn_TASK.ForeColor = System.Drawing.Color.Azure

            Me.btn_SUBJECTS.ForeColor = System.Drawing.Color.Azure

            Me.panel_MAIN_TASKS.BackColor = System.Drawing.Color.LightBlue

            Me.btn_OPENADDTASK.BackColor = System.Drawing.Color.Transparent

            Me.btn_BACKTASK.ForeColor = System.Drawing.Color.MidnightBlue

            Me.Label3.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_DELETETASK.BackColor = System.Drawing.Color.LightCoral
            Me.btn_DELETETASK.ForeColor = System.Drawing.Color.White

            Me.lbl_NTASKS.ForeColor = System.Drawing.Color.MidnightBlue

            Me.Panel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(205, Byte), Integer))

            Me.listbx_TASKLIST2.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_ARCHIVETASK.BackColor = System.Drawing.Color.CadetBlue
            Me.btn_ARCHIVETASK.ForeColor = System.Drawing.Color.White

            Me.cb_NODEADLINE.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_ADDNEWTASK.BackColor = System.Drawing.Color.LimeGreen
            Me.btn_ADDNEWTASK.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label16.BackColor = System.Drawing.Color.Transparent
            Me.Label16.ForeColor = System.Drawing.Color.MidnightBlue

            Me.cmbx_TASKTERM.ForeColor = System.Drawing.Color.MidnightBlue

            Me.Label15.BackColor = System.Drawing.Color.Transparent
            Me.Label15.ForeColor = System.Drawing.Color.MidnightBlue

            Me.Label14.BackColor = System.Drawing.Color.Transparent
            Me.Label14.ForeColor = System.Drawing.Color.MidnightBlue

            Me.Label13.BackColor = System.Drawing.Color.Transparent
            Me.Label13.ForeColor = System.Drawing.Color.MidnightBlue

            Me.cmbx_SUBJECTNAME.ForeColor = System.Drawing.Color.MidnightBlue

            Me.Label12.BackColor = System.Drawing.Color.Transparent
            Me.Label12.ForeColor = System.Drawing.Color.MidnightBlue

            Me.panel_MAIN_NOTES.BackColor = System.Drawing.Color.LightBlue

            Me.btn_OPENADDNOTE.BackColor = System.Drawing.Color.Transparent

            Me.btn_NOTESBACK.ForeColor = System.Drawing.Color.MidnightBlue

            Me.listbx_NOTES.BackColor = System.Drawing.Color.Azure

            Me.panel_NOTEEDITOR.BackColor = System.Drawing.Color.Transparent

            Me.panel_NOTECONTENT.BackColor = System.Drawing.Color.White

            Me.Label25.ForeColor = System.Drawing.Color.MidnightBlue

            Me.txt_NOTETITLE.BackColor = System.Drawing.Color.LightBlue

            Me.btn_SAVENOTE.BackColor = System.Drawing.Color.LimeGreen
            Me.btn_SAVENOTE.ForeColor = System.Drawing.Color.Azure

            Me.btn_DELETENOTE.BackColor = System.Drawing.Color.LightCoral
            Me.btn_DELETENOTE.ForeColor = System.Drawing.Color.Azure

            Me.Label5.ForeColor = System.Drawing.Color.MidnightBlue

            Me.panel_MAIN_SCHEDS.BackColor = System.Drawing.Color.LightBlue

            Me.panel_SCHEDULEPANE.BackColor = System.Drawing.Color.LightCyan

            Me.Label18.BackColor = System.Drawing.Color.LightCyan

            Me.Label4.ForeColor = System.Drawing.Color.MidnightBlue

            Me.panel_MAIN_SUBJECTS.BackColor = System.Drawing.Color.LightBlue

            Me.lbl_MAIN_SUBJECTS.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_ADDSUBJECTBACK.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_OPENREMOVESUBJECT.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_OPENADDSUBJECT.BackColor = System.Drawing.Color.Transparent

            Me.flwpanel_SUBJECTS_DRAWER.BackColor = System.Drawing.Color.LightBlue

            Me.btn_ADDSUBJECT.BackColor = System.Drawing.Color.LimeGreen
            Me.btn_ADDSUBJECT.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label10.ForeColor = System.Drawing.Color.MidnightBlue
            Me.Label9.ForeColor = System.Drawing.Color.MidnightBlue
            Me.Label8.ForeColor = System.Drawing.Color.MidnightBlue
            Me.cmbx_DAY.BackColor = System.Drawing.Color.White
            Me.Label7.ForeColor = System.Drawing.Color.MidnightBlue
            Me.Label6.ForeColor = System.Drawing.Color.MidnightBlue
            Me.Label1.ForeColor = System.Drawing.Color.MidnightBlue

            Me.txt_SUBJECTNAME.BackColor = System.Drawing.Color.White

            Me.txt_SUBJECTID.BackColor = System.Drawing.Color.White

            Me.btn_REMOVESUBJECTS.BackColor = System.Drawing.Color.Firebrick
            Me.btn_REMOVESUBJECTS.ForeColor = System.Drawing.Color.AliceBlue

            Me.Label11.BackColor = System.Drawing.Color.Transparent
            Me.Label11.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_MAXMIN.BackColor = System.Drawing.Color.Transparent
            Me.btn_MAXMIN.ForeColor = System.Drawing.Color.DarkSlateBlue

            Me.btn_MINIMIZE.BackColor = System.Drawing.Color.Transparent
            Me.btn_MINIMIZE.ForeColor = System.Drawing.Color.DarkSlateBlue

            Me.lbl_CTRL_TITLE.ForeColor = System.Drawing.Color.MidnightBlue

            Me.btn_CLOSE.BackColor = System.Drawing.Color.Transparent
            Me.btn_CLOSE.ForeColor = System.Drawing.Color.DarkSlateBlue

            Me.lbl_LOADING.BackColor = System.Drawing.Color.LightBlue
            Me.lbl_LOADING.ForeColor = System.Drawing.Color.DarkSlateGray

            Me.panel_RESIZECONTROL.BackColor = System.Drawing.Color.SteelBlue

            Me.panel_WINDOW.BackColor = System.Drawing.Color.Transparent

            Me.BackColor = System.Drawing.Color.LightBlue

        End If

        init_SUBJECTLIST(False)

    End Sub



    


End Class

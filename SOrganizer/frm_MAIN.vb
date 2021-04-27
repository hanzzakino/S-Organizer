Public Class frm_MAIN

    Dim SUBJECT_LIST As New List(Of List(Of String))
    Dim TASK_LIST_SUBJECTID As New List(Of String)

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
        selButton(btn_SUBJECTS)
        openPanel(panel_MAIN_SUBJECTS)
        frm_LoadingScreen.ProgressBar_main.Value = 100


        date_DEADLINEDATE.Value = Now

        frm_LoadingScreen.Close()

        Me.WindowState = FormWindowState.Maximized

        '''' TEST '''''

        '''''''''''''''

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
            empty_sub.ForeColor = Color.DarkSlateGray
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
            subj_panel.BackColor = Color.Azure

            Dim brder_panel As New Panel
            brder_panel.Parent = subj_panel
            brder_panel.Height = 5
            brder_panel.Width = 450
            brder_panel.Top = 0
            brder_panel.Left = 0
            brder_panel.BackColor = Color.SlateGray


            Dim subj_label As New Label()
            subj_label.Parent = subj_panel
            subj_label.AutoSize = True
            subj_label.Font = New Font("Verdana", 11, FontStyle.Bold)
            subj_label.Top = 13
            subj_label.Left = 5
            subj_label.Anchor = AnchorStyles.Top + AnchorStyles.Left
            subj_label.ForeColor = Color.MidnightBlue
            subj_label.MaximumSize = subj_panel.Size

            Dim subj_tasks_list As New ListBox
            subj_tasks_list.Parent = subj_panel
            subj_tasks_list.Top = 45
            subj_tasks_list.Left = 15
            subj_tasks_list.Height = 150
            subj_tasks_list.Width = 250
            subj_tasks_list.BorderStyle = BorderStyle.None
            subj_tasks_list.SelectionMode = SelectionMode.None
            subj_tasks_list.ForeColor = Color.MidnightBlue
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
            subj_sched_list.ForeColor = Color.MidnightBlue
            subj_sched_list.BackColor = Color.Azure
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
                subj_tasks_list.Items.Add(task(0) + " - " + task(1))
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
                TASK_LIST_SUBJECTID.Add(task(6))
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
                sch_day.BackColor = Color.Aquamarine
                sch_day.TextAlign = ContentAlignment.MiddleCenter
                sch_day.Dock = DockStyle.Fill
                sch_day.Font = New Font("Arial", 10)
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

        Dim exc_panel As New Panel()
        exc_panel.Height = 200
        exc_panel.Width = 450
        flwpanel_SUBJECTS_DRAWER.Controls.Add(exc_panel)

        lbl_NTASKS.Text = listbx_TASKLIST2.Items.Count.ToString + " tasks..."

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
        Else
            Me.WindowState = FormWindowState.Maximized
        End If
    End Sub

    'PANEL DRAG'
    Dim initx As Double = 0
    Dim inity As Double = 0
    Dim initmx As Double = 0
    Dim initmy As Double = 0
    Dim mdown As Boolean = False
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
        End If
    End Sub
    Private Sub panel_CONTROLBAR_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles panel_CONTROLBAR.MouseUp
        mdown = False
    End Sub
    'PANEL DRAG'
    ''''CONTROL BAR''''


    ''''MENU PANEL''''
    'Panel Menu Launch Buttons
    Private Sub btn_SUBJECTS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SUBJECTS.Click
        selButton(btn_SUBJECTS)
        openPanel(panel_MAIN_SUBJECTS)

    End Sub
    Private Sub btn_TASK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_TASK.Click
        selButton(btn_TASK)
        openPanel(panel_MAIN_TASKS)
    End Sub
    Private Sub btn_SCHEDULES_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_SCHEDULES.Click
        lbl_LOADING.Visible = True
        Application.DoEvents()
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
        btn_NOTES.BackColor = Color.SteelBlue
        btn_NOTES.ForeColor = Color.Azure
        btn_SUBJECTS.BackColor = Color.SteelBlue
        btn_SUBJECTS.ForeColor = Color.Azure
        btn_SCHEDULES.BackColor = Color.SteelBlue
        btn_SCHEDULES.ForeColor = Color.Azure
        btn_TASK.BackColor = Color.SteelBlue
        btn_TASK.ForeColor = Color.Azure
        o_btn.BackColor = Color.LightBlue
        o_btn.ForeColor = Color.SteelBlue
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - " + o_btn.Text
    End Sub
    ''''MENU PANEL''''


    ''''SUBJECTS PANEL''''
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
        btn_OPENADDSUBJECT.Visible = True
        btn_ADDSUBJECTBACK.Visible = False
        btn_OPENREMOVESUBJECT.Visible = True

        panel_flwpanelContainer.Visible = True
        panel_ADDSUBJECT.Visible = False
        panel_REMOVESUBJECT.Visible = False
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Subjects"

        txt_SUBJECTID.Clear()
        txt_SUBJECTNAME.Clear()
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
                    MessageBox.Show("Schedule Added", "Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If

            Else
                addSubjSched(txt_SUBJECTID.Text, cmbx_DAY.SelectedItem.ToString, toDECIMALHOUR(time_SCHEDFROM.Value), toDECIMALHOUR(time_SCHEDTO.Value))
                init_SUBJECTLIST(False)
                txt_SUBJECTID.Clear()
                txt_SUBJECTNAME.Clear()
                MessageBox.Show(o_str, "Subject", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

            
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
    ''''SUBJECTS PANEL''''



    ''''TASK PANEL''''
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
        btn_OPENADDTASK.Visible = True
        btn_BACKTASK.Visible = False

        panel_ADDTASK.Visible = False
        panel_TASKLISTPANEL.Visible = True
        Me.lbl_CTRL_TITLE.Text = "Student Organizer - Task"
    End Sub

    'Add Task Button
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ADDNEWTASK.Click

        lbl_LOADING.Visible = True
        Application.DoEvents()
        Dim date_DEADLINE As New Date(date_DEADLINEDATE.Value.Year, date_DEADLINEDATE.Value.Month, date_DEADLINEDATE.Value.Day, date_DEADLINETIME.Value.Hour, date_DEADLINETIME.Value.Minute, 0)
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
            txt_TASKNAME.Clear()
            txt_TASKDESC.Clear()
            cmbx_SUBJECTNAME.ResetText()
            cmbx_TASKTERM.ResetText()

        End If

        lbl_LOADING.Visible = False
    End Sub

    'Archive Task button
    Private Sub btn_ARCHIVETASK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_ARCHIVETASK.Click

        lbl_LOADING.Visible = True
        Application.DoEvents()

        If listbx_TASKLIST2.SelectedItems.Count > 0 Then
            If MessageBox.Show("Arhive Selected Task(s)?", "Remove Taks", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                For Each task_id In listbx_TASKLIST2.SelectedIndices
                    archiveTASK(CInt(TASK_LIST_SUBJECTID(task_id)))
                Next
                init_SUBJECTLIST(False)
                MessageBox.Show("Tasks arhived", "Task", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        Else
            MessageBox.Show("Select Task to Archive", "Empty Selection", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
        If e.KeyCode = 13 Then
            btn_SAVENOTE.PerformClick()
        End If
    End Sub



End Class

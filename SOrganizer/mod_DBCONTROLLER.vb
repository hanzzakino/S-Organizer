Imports MySql.Data.MySqlClient
Imports System.IO



Module mod_MYSQLDBCONTROLLER

    Dim def_CON As String = "server=localhost; user id = root; password = hanz; database = s_organizer"
    Dim dbconfig_file As String = ""
    Dim db_file_loc As String = "dbconfig.txt"
    Dim con As MySqlConnection = Nothing
    Dim db_name As String = "s_organizer"
    Dim reader As MySqlDataReader
    Dim str_version As String = ""

    Public Sub init_DBCONNECTION()
        Try
            readCONFIGFILE() ' Read server connection file
            con = New MySqlConnection(dbconfig_file) 'Initialize Connection to server
            Console.WriteLine(con.ConnectionString)
            If String.IsNullOrWhiteSpace(con.Database) Then
                create_DATABASE() 'create database in server if it doesn't exist
                'con.Dispose() ' dispose connection after creating table
                con = New MySqlConnection(dbconfig_file + "database = " + db_name + ";") ' open a new connection with the created* database
            End If

            con.Open()
            str_version = con.ServerVersion
            Console.WriteLine(str_version)
            Console.WriteLine(con.ConnectionString)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            If MessageBox.Show("Database connection failed" + vbLf + "Error: " + ex.Message + vbLf + vbLf + "Please modify the " + db_file_loc + " file in the installation folder" + vbLf + "and restart the Program", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error) = DialogResult.OK Then
                If File.Exists(db_file_loc) Then
                    Process.Start(db_file_loc)
                Else
                    MessageBox.Show("Config file doesn't exist", "Configuration error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Application.Exit()
                    End
                End If
                Application.Exit()
                End
            Else
                Application.Restart()
                End
            End If
        Finally
            con.Close()
        End Try
        'CREATE REQUIRED TABLES IF DOESNT EXIST
        create_DBTABLES()
    End Sub

    Public Sub init_DBCONNECTION(ByVal server As String, ByVal userid As String, ByVal password As String, ByVal database As String)
        Dim cust_con_string As String = "server=" + server + "; user id = " + userid + "; password = " + password + "; database = " + database + ""
        Try
            con = New MySqlConnection(cust_con_string)
            con.Open()
            str_version = con.ServerVersion
        Catch ex As Exception
            con.Close()
            Console.WriteLine(ex.Message)
            If MessageBox.Show("Database connection failed", "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error) = DialogResult.OK Then
                Application.Exit()
                End
            Else
                Application.Restart()
                End
            End If
        Finally
            con.Close()
        End Try
        'CREATE REQUIRED TABLES IF IT DOESNT EXIST
        create_DBTABLES()
    End Sub

    Public Sub readCONFIGFILE()
        If Not File.Exists(db_file_loc) Then
            Dim fileWriter As New StreamWriter(db_file_loc, True)
            fileWriter.WriteLine("# S Organizer " + frm_LoadingScreen.lbl_VERSION.Text)
            fileWriter.WriteLine("# MySQL server")
            fileWriter.WriteLine("# Database scheme 's_organizer' will be created if a database  is not specified")
            fileWriter.WriteLine("# Specify the server details here:")
            fileWriter.WriteLine("")
            fileWriter.WriteLine("server = localhost;")
            fileWriter.WriteLine("user id = root;")
            fileWriter.WriteLine("password = ;")
            fileWriter.WriteLine("database = ;")
            fileWriter.Close()
        Else
            Dim fileReader As New StreamReader(db_file_loc)
            While Not fileReader.Peek() = -1
                Dim temp_line As String = fileReader.ReadLine()
                If String.IsNullOrWhiteSpace(temp_line) Then
                    Continue While
                End If
                If Not temp_line.First = "#" Then
                    dbconfig_file &= temp_line
                End If
                Console.WriteLine(dbconfig_file)
            End While
            fileReader.Close()
        End If
    End Sub


    'Add Databse and DATA Tables if not exist

    Public Sub create_DBTABLES()
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            Dim cmd2 As New MySqlCommand()
            Dim cmd3 As New MySqlCommand()
            Dim cmd4 As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "CREATE TABLE IF NOT EXISTS subjects (`SUBJECT_ID` VARCHAR(45) NOT NULL ,`SUBJECT_NAME` VARCHAR(200) NOT NULL ,PRIMARY KEY (`SUBJECT_ID`) );"
                .Prepare()
                .ExecuteNonQuery()
            End With

            With cmd2
                .Connection = con
                .CommandText = "CREATE  TABLE IF NOT EXISTS subject_tasks (`TASK_ID` INT NOT NULL AUTO_INCREMENT , `SUBJECT_ID` VARCHAR(45) NOT NULL DEFAULT 'NO SUB' , `TASK_NAME` VARCHAR(200) NOT NULL , `TASK_DESCRIPTION` LONGTEXT NULL , `TASK_DEADLINE_DATE` VARCHAR(45) NULL , `TASK_DEADLINE_TICKS` BIGINT NULL , `TASK_TERM` VARCHAR(45) NULL , `STATUS` INT NOT NULL DEFAULT 0, PRIMARY KEY (`TASK_ID`) );"
                .Prepare()
                .ExecuteNonQuery()
            End With

            With cmd3
                .Connection = con
                .CommandText = "CREATE TABLE IF NOT EXISTS subject_schedules (`SCHED_ID` INT NOT NULL AUTO_INCREMENT ,`SUBJECT_ID` VARCHAR(50) NOT NULL ,`DAY` VARCHAR(45) NOT NULL , `TFROM` DOUBLE NOT NULL  , `TTO` DOUBLE NOT NULL , PRIMARY KEY (`SCHED_ID`));"
                .Prepare()
                .ExecuteNonQuery()
            End With

            With cmd4
                .Connection = con
                .CommandText = "CREATE TABLE IF NOT EXISTS subject_notes (`NOTE_ID` INT NOT NULL AUTO_INCREMENT ,`NOTE_TITLE` VARCHAR(300) NOT NULL DEFAULT 'Untitled Document' ,`NOTE_CONTENT` LONGTEXT NOT NULL ,PRIMARY KEY (`NOTE_ID`) );"
                .Prepare()
                .ExecuteNonQuery()
            End With

            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub

    Public Sub create_DATABASE()
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "CREATE SCHEMA IF NOT EXISTS " + db_name + ";"
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub

    'DB RETRIEVE

    Public Function getSubjectList() As List(Of List(Of String))
        Dim output As New List(Of List(Of String))

        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "SELECT * FROM subjects"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                Dim temp As New List(Of String)
                temp.Add(reader(0)) ' SUBJECT ID
                temp.Add(reader(1)) ' SUBJECT NAME
                output.Add(temp)
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try

        Return output
    End Function

    Public Function getSUBJECTTASKS(ByVal SUBJECT_ID As String) As List(Of List(Of String))
        Dim output As New List(Of List(Of String))
        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "SELECT TASK_NAME,TASK_DESCRIPTION,TASK_DEADLINE_DATE,TASK_DEADLINE_TICKS,TASK_TERM,SUBJECT_ID,TASK_ID FROM subject_tasks WHERE SUBJECT_ID = '" + SUBJECT_ID + "' AND STATUS = 0;"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                Dim temp As New List(Of String)
                temp.Add(reader(0).ToString) 'NAME
                temp.Add(reader(1).ToString) 'DESC
                temp.Add(reader(2).ToString) 'DEADLINE DATE
                temp.Add(reader(3).ToString) 'DEADLINE TICKS
                temp.Add(reader(4).ToString) 'TERM
                temp.Add(reader(5).ToString) 'SUBJECT
                temp.Add(reader(6).ToString) 'ID
                output.Add(temp)
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
        Return output

    End Function

    Public Function getSubjectID(ByVal SUBJECT_NAME As String) As String

        Dim output As String = ""

        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "SELECT SUBJECT_ID FROM subjects WHERE SUBJECT_NAME = '" + SUBJECT_NAME + "';"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                output = reader(0).ToString
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            reader.Close()
            con.Close()
            Console.WriteLine(ex.Message)
        End Try

        Return output

    End Function

    Public Function getSubjectName(ByVal SUBJECT_ID As String) As String

        Dim output As String = ""

        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "SELECT SUBJECT_NAME FROM subjects WHERE SUBJECT_ID = '" + SUBJECT_ID + "';"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                output = reader(0).ToString
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            reader.Close()
            con.Close()
            Console.WriteLine(ex.Message)
        End Try

        Return output

    End Function

    Public Function getScheduleString(ByVal SUBJECT_ID As String) As List(Of List(Of String))
        Dim output As New List(Of List(Of String))

        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "SELECT DAY,TFROM,TTO FROM subject_schedules WHERE SUBJECT_ID = '" + SUBJECT_ID + "';"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                Dim temp As New List(Of String)
                temp.Add(reader(0).ToString) ' DAY
                temp.Add(toTWHOUR(Double.Parse(reader(1)))) ' TIME FROM
                temp.Add(toTWHOUR(Double.Parse(reader(2)))) ' TIME TO
                output.Add(temp)
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            con.Close()
            Console.WriteLine(ex.Message)
        End Try

        Return output
    End Function

    Public Function getSUBJECTTASKSARCHIVE(ByVal SUBJECT_ID As String) As List(Of List(Of String))
        Dim output As New List(Of List(Of String))
        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "SELECT TASK_NAME,TASK_DESCRIPTION,TASK_DEADLINE_DATE,TASK_DEADLINE_TICKS,TASK_TERM,SUBJECT_ID,TASK_ID FROM subject_tasks WHERE SUBJECT_ID = '" + SUBJECT_ID + "' AND STATUS = 1;"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                Dim temp As New List(Of String)
                temp.Add(reader(0).ToString) 'NAME
                temp.Add(reader(1).ToString) 'DESC
                temp.Add(reader(2).ToString) 'DEADLINE DATE
                temp.Add(reader(3).ToString) 'DEADLINE TICKS
                temp.Add(reader(4).ToString) 'TERM
                temp.Add(reader(5).ToString) 'SUBJECT
                temp.Add(reader(6).ToString) 'ID
                output.Add(temp)
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
        Return output

    End Function

    Public Function getNOTESLIST() As List(Of String)
        Dim output As New List(Of String)

        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "SELECT NOTE_TITLE FROM subject_notes"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                output.Add(reader(0).ToString)
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            reader.Close()
            con.Close()
            Console.WriteLine(ex.Message)
        End Try

        Return output
    End Function

    Public Function getNoteContent(ByVal NOTE_TITLE As String) As String
        Dim output As String = ""
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "SELECT NOTE_CONTENT FROM subject_notes WHERE NOTE_TITLE='" + NOTE_TITLE + "';"
                .Prepare()
                reader = .ExecuteReader
            End With
            While reader.Read
                output = reader(0).ToString
            End While
            reader.Close()
            con.Close()
        Catch ex As Exception
            reader.Close()
            con.Close()
            Console.WriteLine(ex.Message)
        End Try
        Return output
    End Function

    Public Function noteExist(ByVal NOTE_TITLE As String) As Boolean
        Dim output As Boolean = False
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "SELECT * FROM subject_notes WHERE NOTE_TITLE='" + NOTE_TITLE + "';"
                .Prepare()
                reader = .ExecuteReader
            End With
            While reader.Read
                output = True
            End While
            reader.Close()
            con.Close()
        Catch ex As Exception
            reader.Close()
            con.Close()
            Console.WriteLine(ex.Message)
        End Try
        Return output
    End Function

    Public Function getTASK(ByVal TASK_ID As String) As List(Of String)
        Dim col As New List(Of String)


        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "SELECT * FROM subject_tasks WHERE TASK_ID = " + TASK_ID + ";"
                .Prepare()
                reader = .ExecuteReader
            End With

            While reader.Read
                col.Add(reader(0).ToString) ' ID
                col.Add(reader(1).ToString) ' SUBJECT ID
                col.Add(reader(2).ToString) ' NAME
                col.Add(reader(3).ToString) ' DESCRIPTION
                col.Add(reader(4).ToString) ' DEADLINE DATE
                col.Add(reader(5).ToString) ' DEADLINE TICKS
                col.Add(reader(6).ToString) ' TERM
            End While

            reader.Close()
            con.Close()
        Catch ex As Exception
            reader.Close()
            con.Close()
            Console.WriteLine(ex.Message)
        End Try


        Return col
    End Function

    'DB INSERT

    Public Function addSUBJECT(ByVal SUBJECT_ID As String, ByVal SUBJECT_NAME As String) As String
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "INSERT INTO subjects (`SUBJECT_ID`, `SUBJECT_NAME`) VALUES ('" + SUBJECT_ID + "', '" + SUBJECT_NAME + "');"
                .Prepare()
                .ExecuteNonQuery()
            End With

            con.Close()
        Catch ex As Exception
            con.Close()
            Console.WriteLine(ex.Message)
            Return "Subject Code already Exist"
        End Try
        Return SUBJECT_ID + " added Succesfully"

    End Function

    Public Sub addTASK(ByVal SUBJECT_ID As String, ByVal TASK_NAME As String, ByVal TASK_DESCRIPTION As String, ByVal TASK_DEADLINE_DATE As String, ByVal TASK_DEADLINE_TICKS As String, ByVal TASK_TERM As String, ByVal STATUS As String)
        Dim cmdtxt As String
        If String.IsNullOrWhiteSpace(TASK_DEADLINE_DATE) Then
            cmdtxt = "INSERT INTO subject_tasks (`SUBJECT_ID`, `TASK_NAME`, `TASK_DESCRIPTION`, `TASK_TERM`, `STATUS`) VALUES ('" + SUBJECT_ID + "', '" + TASK_NAME + "', '" + TASK_DESCRIPTION + "', '" + TASK_TERM + "', '" + STATUS + "');"
        Else
            cmdtxt = "INSERT INTO subject_tasks (`SUBJECT_ID`, `TASK_NAME`, `TASK_DESCRIPTION`, `TASK_DEADLINE_DATE`, `TASK_DEADLINE_TICKS`, `TASK_TERM`, `STATUS`) VALUES ('" + SUBJECT_ID + "', '" + TASK_NAME + "', '" + TASK_DESCRIPTION + "', '" + TASK_DEADLINE_DATE + "', '" + TASK_DEADLINE_TICKS + "', '" + TASK_TERM + "', '" + STATUS + "');"
        End If

        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = cmdtxt
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub

    Public Sub addSubjSched(ByVal SUBJECT_ID As String, ByVal DAY As String, ByVal TFROM As Double, ByVal TTO As Double)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "INSERT INTO subject_schedules (SUBJECT_ID,DAY,TFROM,TTO) VALUES('" + SUBJECT_ID + "', '" + DAY + "', '" + TFROM.ToString + "', '" + TTO.ToString + "')"
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub

    Public Sub addNote(ByVal NOTE_TITLE As String, ByVal NOTE_CONTENT As String)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "INSERT INTO subject_notes (NOTE_TITLE, NOTE_CONTENT) VALUES('" + NOTE_TITLE + "', '" + NOTE_CONTENT + "')"
                .Prepare()
                .ExecuteNonQuery()
            End With

            con.Close()
        Catch ex As Exception
            con.Close()
            Console.WriteLine(ex.Message)
        End Try
    End Sub



    'DB DELETE

    Public Sub removeSUBJECT(ByVal SUBJECT_ID As String)

        Try
            con.Open()

            Dim cmd As New MySqlCommand()
            Dim cmd2 As New MySqlCommand()
            Dim cmd3 As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "DELETE FROM subjects WHERE SUBJECT_ID = '" + SUBJECT_ID + "';"
                .Prepare()
                .ExecuteNonQuery()
            End With

            With cmd2
                .Connection = con
                .CommandText = " DELETE FROM subject_tasks WHERE SUBJECT_ID = '" + SUBJECT_ID + "';"
                .Prepare()
                .ExecuteNonQuery()
            End With

            With cmd3
                .Connection = con
                .CommandText = "DELETE FROM subject_schedules WHERE SUBJECT_ID = '" + SUBJECT_ID + "';"
                .Prepare()
                .ExecuteNonQuery()
            End With

            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try

    End Sub

    Public Sub removeTASK(ByVal TASK_ID As Integer)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "DELETE FROM subject_tasks WHERE TASK_ID = '" + TASK_ID.ToString + "';"
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub

    Public Sub removeNote(ByVal NOTE_TITLE As String)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "DELETE FROM subject_notes WHERE NOTE_TITLE = '" + NOTE_TITLE + "';"
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub


    'DB CHECK

    Public Function subjectExist(ByVal SUBJECT_NAME As String) As Boolean
        Dim output As Boolean = False

        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "SELECT * FROM subjects WHERE SUBJECT_NAME = '" + SUBJECT_NAME + "';"
                .Prepare()
                reader = .ExecuteReader
            End With
            While reader.Read
                output = True
            End While
            reader.Close()
            con.Close()
        Catch ex As Exception
            output = False
            reader.Close()
            con.Close()
            Console.WriteLine(ex.Message)
        End Try

        Return output
    End Function


    'DB UPDATE

    Public Sub archiveTASK(ByVal TASK_ID As Integer)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "UPDATE subject_tasks SET STATUS=1 WHERE TASK_ID=" + TASK_ID.ToString + ";"
                ' STATUS 1 IS ARCHIVED
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub

    Public Sub unarchiveTASK(ByVal TASK_ID As Integer)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "UPDATE subject_tasks SET STATUS=0 WHERE TASK_ID=" + TASK_ID.ToString + ";"
                ' STATUS 1 IS ARCHIVED
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            con.Close()
        End Try
    End Sub

    Public Sub updateNote(ByVal NOTE_TITLE As String, ByVal NOTE_CONTENT As String)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()

            With cmd
                .Connection = con
                .CommandText = "UPDATE subject_notes SET NOTE_CONTENT='" + NOTE_CONTENT + "' WHERE NOTE_TITLE='" + NOTE_TITLE + "';"
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            con.Close()
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub updateTASK(ByVal TASK_ID As String, ByVal SUBJECT_ID As String, ByVal TASK_NAME As String, ByVal TASK_DESCRIPTION As String, ByVal TASK_DEADLINE_DATE As String, ByVal TASK_DEADLINE_TICKS As String, ByVal TASK_TERM As String, ByVal STATUS As String)
        Dim cmdtxt As String
        If String.IsNullOrWhiteSpace(TASK_DEADLINE_DATE) Then
            cmdtxt = "UPDATE subject_tasks SET `SUBJECT_ID`= '" + SUBJECT_ID + "', `TASK_NAME`= '" + TASK_NAME + "', `TASK_DESCRIPTION`= '" + TASK_DESCRIPTION + "', `TASK_TERM`= '" + TASK_TERM + "',  `STATUS`= '" + STATUS + "',`TASK_DEADLINE_DATE`='', `TASK_DEADLINE_TICKS`=NULL WHERE TASK_ID = " + TASK_ID + ";"

        Else
            cmdtxt = "UPDATE subject_tasks SET `SUBJECT_ID`= '" + SUBJECT_ID + "', `TASK_NAME`= '" + TASK_NAME + "', `TASK_DESCRIPTION`= '" + TASK_DESCRIPTION + "', `TASK_DEADLINE_DATE`= '" + TASK_DEADLINE_DATE + "', `TASK_DEADLINE_TICKS`= '" + TASK_DEADLINE_TICKS + "', `TASK_TERM`= '" + TASK_TERM + "', `STATUS`= '" + STATUS + "' WHERE TASK_ID = " + TASK_ID + ";"

        End If
        
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = cmdtxt
                .Prepare()
                .ExecuteNonQuery()
            End With
            con.Close()
        Catch ex As Exception
            con.Close()
            Console.WriteLine(ex.Message)
        End Try
    End Sub

    Public Sub updateSubject(ByVal SUBJECT_ID As String, ByVal SUBJECT_NAME As String, ByVal DAY As String, ByVal TFROM As Double, ByVal TTO As Double)
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            With cmd
                .Connection = con
                .CommandText = "UPDATE subjects SET  `SUBJECT_NAME` = '" + SUBJECT_NAME + "' WHERE `SUBJECT_ID` = '" + SUBJECT_ID + "' ;"
                .Prepare()
                .ExecuteNonQuery()
            End With

            Dim cmd2 As New MySqlCommand()
            With cmd2
                .Connection = con
                .CommandText = "UPDATE subject_schedules SET DAY = '" + DAY + "' , TFROM = '" + TFROM.ToString + "' , TTO = '" + TTO.ToString + "' WHERE `SUBJECT_ID` = '" + SUBJECT_ID + "'"
                .Prepare()
                .ExecuteNonQuery()
            End With

            con.Close()
        Catch ex As Exception
            con.Close()
            Console.WriteLine(ex.Message)
        End Try
    End Sub


    'CONVERTIONS

    Public Function toDECIMALHOUR(ByVal hour As Date) As Double
        Return (Double.Parse(hour.ToString("HHHH"))) + (Double.Parse(hour.ToString("mm")) / 60)
    End Function

    Public Function toTWHOUR(ByVal dec As Double) As String
        Dim hour As Integer = Math.Floor(dec)
        Dim min As Double = Math.Round((dec - hour) * 60)
        Dim AMPM As String = "AM"
        If dec >= 12 Then
            AMPM = "PM"
        End If

        If hour Mod 12 = 0 Then
            hour = 12
        Else
            hour = hour Mod 12
        End If

        If min.ToString.Length = 1 Then
            Return hour.ToString + ":0" + min.ToString + " " + AMPM
        Else
            Return hour.ToString + ":" + min.ToString + " " + AMPM
        End If
    End Function







End Module


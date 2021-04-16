Imports MySql.Data.MySqlClient


Module mod_MYSQLDBCONTROLLER

    Dim str_CON As String = "server=localhost; user id = root; password = hanz; database = s_organizer"
    Dim con As MySqlConnection = Nothing
    Dim reader As MySqlDataReader
    Dim str_version As String = ""

    Public Sub init_DBCONNECTION()
        Try
            con = New MySqlConnection(str_CON)
            con.Open()
            str_version = con.ServerVersion
        Catch ex As Exception
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub create_DBTABLES()
        Try
            con.Open()
            Dim cmd As New MySqlCommand()
            Dim cmd2 As New MySqlCommand()
            Dim cmd3 As New MySqlCommand()

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


End Module


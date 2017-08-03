Imports Blackbaud.AppFx.Server
Imports Blackbaud.AppFx
'Imports Blackbaud.AppFx.UIModeling.Core
Imports System.Data.SqlClient

Public NotInheritable Class EFTDeclineBatchBusinessProcess
	Inherits AppCatalog.AppBusinessProcess

    Private _campaignType As Integer = Nothing
    Private _parameters As BizParameters = Nothing

    Private Const outputTablePrefix As String = "USR_EFTDECLINEBATCH_BUSINESSPROCESS"

    Private Class BizParameters
        Public ReadOnly BATCHNUMBER As String = String.Empty
        Public ReadOnly BATCHTYPE As Integer = Nothing

        Public Sub New(ByVal parameterSetID As Guid, ByRef requestContext As RequestContext)
            Using con As SqlConnection = New SqlConnection(requestContext.AppDBConnectionString)
                Using command As SqlCommand = con.CreateCommand()
                    Try
                        command.CommandText = "USR_USP_EFTDECLINEBATCH_PROCESS_GETPARAMETERS"

                        command.CommandType = CommandType.StoredProcedure
                        command.Parameters.AddWithValue("@ID", parameterSetID)
                        con.Open()
                        Using reader As SqlDataReader = command.ExecuteReader()
                            reader.Read()
                            Me.BATCHNUMBER = reader.GetString(reader.GetOrdinal("BATCHNUMBER"))
                            Me.BATCHTYPE = reader.GetInt32(reader.GetOrdinal("BATCHTYPE"))
                            reader.Close()
                        End Using
                        con.Close()
                    Catch
                        Throw New Exception("Unable to get parameter set found for the given Id")
                    End Try
                End Using
            End Using
        End Sub

    End Class

    Public Overrides Function StartBusinessProcess() As Blackbaud.AppFx.Server.AppCatalog.AppBusinessProcessResult
		'This is the trick to get the parameter values the user selected when starting the biz process:
		Dim batchNumberDfv As XmlTypes.DataForms.DataFormFieldValue = Nothing
		Dim batchNumber As String = String.Empty
		Dim batchTypeDfv As XmlTypes.DataForms.DataFormFieldValue = Nothing
		Dim batchType As String = String.Empty
        'Dim batchDateDfv As XmlTypes.DataForms.DataFormFieldValue = Nothing
        'Dim batchDate As String = String.Empty

        'If Me.RequestArgs.DataItem.TryGetValue("BATCHDATE", batchDateDfv) Then
        '	batchDate = CStr(batchDateDfv.Value)
        'End If

        'If Me.RequestArgs.DataItem.TryGetValue("BATCHNUMBER", batchNumberDfv) Then
        '    batchNumber = CStr(batchNumberDfv.Value)
        'End If

        'If Me.RequestArgs.DataItem.TryGetValue("BATCHTYPE", batchTypeDfv) Then
        '    batchType = CStr(batchTypeDfv.Value)
        'End If

        Me.UpdateProcessStatus(String.Format("Processing EFT Decline Batch #{0} for Batch Type of {1}", _parameters.BATCHNUMBER, _parameters.BATCHTYPE))

        Dim ProcessResults As New Blackbaud.AppFx.Server.AppCatalog.AppBusinessProcessResult

        Dim Transaction As Data.SqlClient.SqlTransaction = Nothing

		'Dim campaignType As Integer = RequestArgs.DataItem.Values.Item("CAMPAIGNTYPE").Value

		' Execute the command to populate the table
		Dim outputTableName As String = CreateOutputTable(outputTablePrefix, "_RESULTS", OutputTableColumns().ToArray())
		'Dim exceptionTableName As String = CreateOutputTable(String.Concat(outputTablePrefix, exceptionTablePrefix), "Exception", ExceptionTableColumns().ToArray())

		Dim successParam As SqlParameter
		Dim exceptionParam As SqlParameter

		Using Connection As SqlConnection = New SqlConnection(Me.RequestContext.AppDBConnectionString)
			Connection.Open()
			Using Command As SqlCommand = Connection.CreateCommand()
				With Command
					Try
						Transaction = Connection.BeginTransaction
						.Transaction = Transaction

						.CommandType = CommandType.StoredProcedure
						.CommandText = "dbo.USR_USP_EFTDECLINEBATCH_BUSINESSPROCESS"
						.CommandTimeout = Me.ProcessCommandTimeout
                        .Parameters.AddWithValue("@BATCHNUMBER", _parameters.BATCHNUMBER)               ' The sproc will get the Change Agent ID if we pass in null
                        .Parameters.AddWithValue("@BATCHTYPE", _parameters.BATCHTYPE)       ' The table to insert the results of conversion process
                        '.Parameters.AddWithValue("exceptionTableName", exceptionTableName)	' The table to insert exceptions and kids put on the waiting list

                        successParam = New SqlParameter("successCount", SqlDbType.Int)		' The number of kids registered
						successParam.Direction = ParameterDirection.Output
						.Parameters.Add(successParam)

						exceptionParam = New SqlParameter("exceptionCount", SqlDbType.Int)	' The number of failed conversions
						exceptionParam.Direction = ParameterDirection.Output
						.Parameters.Add(exceptionParam)

						.ExecuteNonQuery()

						Transaction.Commit()

						If successParam.Value IsNot Nothing AndAlso IsNumeric(successParam.Value) Then
							ProcessResults.NumberSuccessfullyProcessed = CInt(successParam.Value)
						Else
							ProcessResults.NumberSuccessfullyProcessed = 7777777	' We didn't get a value.  Unfortunately, I can't set this to -1, so I picked a value that we shouldn't ever have.
						End If

						If exceptionParam.Value IsNot Nothing AndAlso IsNumeric(exceptionParam.Value) Then
							ProcessResults.NumberOfExceptions = CInt(exceptionParam.Value)
						Else
							ProcessResults.NumberOfExceptions = 7777777				' We didn't get a value.  Unfortunately, I can't set this to -1, so I picked a value that we shouldn't ever have.
						End If

					Catch ex As Exception
						Transaction.Rollback()

						Throw ex
					End Try


				End With

			End Using
		End Using

		Return ProcessResults

	End Function

	Private Function OutputTableColumns() As List(Of AppCatalog.TableColumn)
		'
		'- store in table:
		' - sponsorlookupid
		' - D2CAMPAIGNMESSAGE

		Dim columns As New List(Of AppCatalog.TableColumn)
		columns.Add(New AppCatalog.TableColumn("SPONSORLOOKUPID", SqlDbType.NVarChar, 10))
		columns.Add(New AppCatalog.TableColumn("BATCHDATE", SqlDbType.Date))
		columns.Add(New AppCatalog.TableColumn("BATCHTYPE", SqlDbType.Int))
		columns.Add(New AppCatalog.TableColumn("INTERACTIONSTATUS", SqlDbType.NVarChar, 1000))
		columns.Add(New AppCatalog.TableColumn("INTERACTIONCREATEDCOUNT", SqlDbType.Int))
		columns.Add(New AppCatalog.TableColumn("NOINTERACTIONCOUNT", SqlDbType.Int))
		columns.Add(New AppCatalog.TableColumn("BATCHNUMBER", SqlDbType.NVarChar, 150))

		Return columns

	End Function

    Public Overrides Sub Validate()
        MyBase.Validate()
        Me.UpdateProcessStatus("inside of Validate() method")

        'get the parameters for this run:
        _parameters = New BizParameters(RequestArgs.ParameterSetID, Me.RequestContext)
        If _parameters Is Nothing Then
            Throw New Exception("No parameters found with the given parameter!")
        End If

    End Sub

    'Private Sub DisplayPrompt(ByVal message As String)
    '    Me.DisplayPrompt(message)
    '    'Me.Prompts.Add(New UIPrompt() With {
    '    '  .Text = message,
    '    '  .ButtonStyle = UIPromptButtonStyle.Ok})
    'End Sub

    'optional overrides
    'Public Overrides Sub CheckForSimultaneousRuns()
    '    MyBase.CheckForSimultaneousRuns()
    'End Sub

    'Public Overrides Function GetAppLockNameForParameterSet() As String
    '    Return MyBase.GetAppLockNameForParameterSet()
    'End Function

End Class
